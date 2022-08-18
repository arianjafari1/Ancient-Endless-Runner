using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// Script created 08/06/22 by Malachi
    /// 
    /// 08/06/22 - added movement between lanes
    ///          - implemented new Unity Input System
    /// 14/06/22 - added upwards jump movement
    ///          - changed MoveTowards in lane movement to Translate to prevent input queueing
    /// 15/06/22 - added downwards jump movement
    /// 17/06/22 - added comments
    ///          - added failsafe in case jump force cannot reach jump height
    ///          - serialized gravity to give designers more freedom
    /// 19/06/22 - created slide animation, attempted to implement
    /// 20/06/22 - slide animation plays when it's supposed to initially (requires fix)
    /// 21/06/22 - removed slide having a toggle which caused animation to only work once
    /// 22/06/22 - swapped animator for an animation array
    ///          - replaced bools with Animation.play() functions
    ///          - initial attempt at staggering (requires slight rework of tilemanager class)
    /// 24/06/22 - set up the debug button so i can test things (such as tripping/dieing) before 
    ///            their respective triggers have been created.
    ///          - Staggering mechanic implemented.
    ///          - Staggering function added to the debug button.
    /// 27/06/22 - switched values of left and right lanes to accomodate for flipped camera
    /// 28/06/22 - added a bouldermovement object and switches stagger bool when the player trips
    /// 29/06/22 - added a cancelInvoke option so the movement speed returning can be cancelled by other scripts
    ///          - switched movement decrease on staggering to multiplying by a decimal rather than 
    ///            subtracting a fixed amount
    /// 05/07/22 - Changed "jump height" to be addition on top of jump height, instead of being "the jump reaches this height"
    /// 12/07/22 - Created PlayerStates enum to aid other scripts with functions which would change functionality based
    ///            on if the player was running, staggering or dead
    ///          - Created PlayAnimation function so animations can be played by other scripts
    ///          - Removed switchPlayerStagger variables linked to boulder movement and replaced with PlayerState
    /// 13/07/22 - added player input disable for after player death (the flat player could still move left/right/jump/slide)
    /// 15/07/22 - moved setDeathState to the player instead of the boulder
    /// 20/07/22 - [ARIAN] moved tile speed being set to 0 when the player dies to GameState
    /// 22/07/22 - moved showing gameover screen to when boulder goes off screen so player gets to see the death animation properly
    ///          - added gamestate to player rather than linking it to gameover screen
    /// 
    /// 23/07/22 - [OAKLEY] Added some animations          
    /// 23/07/22 - reinstated overwritten code causing animations to not play properly (slide, flatten lost functionality)
    /// 26/07/22 - added variables for powered up jump
    ///          - added function to switch between normal and powered jump
    /// 27/07/22 - added invincible player state to be used by obstacle class
    /// 29/07/22 - added more animation states, switched several to bools instead of triggers to fix them up
    /// 01/08/22 - removed turning animations and replaced them with physical model animations as it was causing issues with
    ///            other animations not triggering when they should
    ///          - added several more animations
    /// 02/08/22 - added look behind mechanic, which can randomly trigger if the boulder is close enough
    ///          - changed it so you can no longer jump/slide while stumbling
    ///          - Made stumbling on an object midair pull you towards the ground
    /// 03/08/22 - added a bool for isSliding, which is set to true in the "slide" animation (the one that shrinks the 
    ///            player hitbox) so that the player cannot queue another slide while already sliding
    /// 04/08/22 - [Arian] called in a function for the pause menu in a function made by Malachi that checks if escape key has been pressed
    /// 10/08/22 - attempted a different way of using the jump powerup as framerate gets obliterated if you jump too many
    ///            times while the powerup is active. Attempt was to stop values changing whilst you are midair, but that 
    ///            turned out to not be the problem. Superjump will probably have to be removed unless the reason for this
    ///            bug is found, as there seems to be nothing in the code that is causing it.
    /// 11/08/22 - changed superjump to a leap forwards rather than increasing height to combat framerate issues
    /// 12/08/22 - added audiomanager and several sounds
    /// 15/08/22 - made sure fixedupdate doesnt run during cutscene
    /// 16/08/22 - [Oakley] Added variables for powerup effects
    /// 17/08/22 - fixed bug where slide cancelling with superjump didnt revert speed back
    /// 18/08/22 - added more sounds
    ///          - turned off the debug button
    /// 
    /// </summary>
    private PauseMenu pauseMenu; //reference to PauseMenu script
    
    [SerializeField] private float horizontalSpeed;
    public enum Lanes
    {
        left = 3,
        center = 0,
        right = -3
    };


    [SerializeField] private GameObject player;
    private GameState gameState;
    public enum PlayerStates
    {
        running,
        staggered,
        dead
    };
    private PlayerStates currentPlayerState;
    public PlayerStates getPlayerState
    {
        get
        {
            return currentPlayerState;
        }
        set
        {
            currentPlayerState = value;
        }
    }

    private MovementInputActions movementInputActions;
    private InputAction left;
    private InputAction right;
    private InputAction jump;
    private InputAction slide;
    private InputAction pauseButton;
    private InputAction debugButton;

    private Lanes currentLane;
    public Lanes getLane
    {
        get
        {
            return currentLane;
        }
    }

    [Tooltip("Changes rate of acceleration (must be negative)")]
    [SerializeField] private float gravity = -9.8f;
    private float currentJumpVelocity;
    private float groundHeight;
    private float currentJumpForce;
    private float currentMaxJumpHeight;

    [Tooltip("How quickly the player jumps")]
    [SerializeField] private float jumpForce;
    [Tooltip("How high the player jumps")]
    [SerializeField] private float maxJumpHeight;
    [Tooltip("Force of the powered up jump")]
    [SerializeField] private float poweredJumpForce; //obsolete
    [Tooltip("Height of the powered up jump")]
    [SerializeField] private float poweredJumpHeight; //obsolete

    //checks used for powerups
    private bool shieldActive = false;
    private bool superJumpActive = false;
    private bool willJumpBeSuper = false;
    public bool IsShieldActive
    {
        get
        {
            return shieldActive;
        }
        set
        {
            shieldActive = value;
        }
    }
    public bool IsSuperJumpActive
    {
        get
        {
            return superJumpActive;
        }
        set
        {
            superJumpActive = value;
        }
    }
    [Tooltip("multiplier of the superjump to tile movement")]
    [SerializeField] private float superJumpSpeedIncrease;
    private float speedBeforeSuperJump;

    private bool isJumping = false;
    private bool isFalling = false;

    //used for checking if the player is in the air while super jump is active in boulder movement code.
    public bool isPlayerAirborne
    {
        get
        {
            if (isJumping || isFalling)
            {
                return true;
            }
            return false;
        }
    }
    private bool isTryingToSlide = false;
    [SerializeField] private bool isSliding = false;
    [Tooltip("Acceleration modifier for jump cancels")]
    [SerializeField] private float AccelerationModifier;


    private Animation playerAnimation;
    private Animator movementAnimations;
    [SerializeField] private GameObject playerModel; //Added by Oakley to reference the player model
    //used for look behind animation
    private bool boulderIsClose = false;
    public bool IsBoulderClose
    {
        get
        {
            return boulderIsClose;
        }
        set
        {
            boulderIsClose = value;
        }
    }

    [SerializeField] TileMovement tileMovement;
    [Tooltip("Between 0-1, the reduction of speed while player staggers")]
    [SerializeField] private float staggerSpeedDecrease;
    [Tooltip("Between 0-1 but above staggerSpeedDecrease, the secondary reduction of speed decrease. Set to 1 if you dont want further decrease from initial")]
    [SerializeField] private float secondStaggerSpeedDecrease;
    [SerializeField] private float staggerDuration;
    private float speedBeforeStagger;
    [SerializeField] private BoulderMovement boulderMovement;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private AudioManager audioManager;

    private void Awake()
    {
        movementInputActions = new MovementInputActions();
        pauseMenu = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PauseMenu>();
    }
    /// <summary>
    /// These variables are used with the new Unity Input System, and add functions
    /// to each of the performed actions.
    /// </summary>
    private void OnEnable()
    {
        
        left = movementInputActions.Player.Left;
        right = movementInputActions.Player.Right;
        jump = movementInputActions.Player.Jump;
        slide = movementInputActions.Player.Slide;
        pauseButton = movementInputActions.Player.PauseGame;
        left.Enable();
        right.Enable();
        jump.Enable();
        slide.Enable();
        pauseButton.Enable();
        left.performed += GoLeft;
        right.performed += GoRight;
        jump.performed += Jump;
        slide.performed += Slide;
        pauseButton.performed += PauseButton;

        //the debug button
        //a mysterious entity that lets you test things that shouldnt
        //be able to happen yet
        debugButton = movementInputActions.Player.TheDebugButton;
        debugButton.Enable();
        debugButton.performed += DebugButtonPressed;
    }



    private void OnDisable()
    {
        left.Disable();
        right.Disable();
        jump.Disable();
        slide.Disable();
        pauseButton.Disable();
        debugButton.Disable();
    }


    /// <summary>
    /// Sets the player to always start in the center lane, and gets the y position of the player
    /// to work with during the landing function so it knows where to stop falling. also sets several
    /// variables to the corresponding components
    /// </summary>
    void Start()
    {
        currentLane = Lanes.center;
        currentPlayerState = PlayerStates.running;
        currentJumpForce = jumpForce;
        currentMaxJumpHeight = maxJumpHeight;
        groundHeight = player.transform.position.y;
        playerAnimation = gameObject.GetComponent<Animation>(); //Changing for Animator
        movementAnimations = playerModel.GetComponent<Animator>();
        gameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameState>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }



    /// <summary>
    /// FixedUpdate is used to make sure that movement is smooth and consistant, even on lower or 
    /// much higher performance devices.
    /// </summary>
    void FixedUpdate()
    {
        //no update if cutscene is playing. prevents possibility of any updating messing with the player's position/rotation
        if (gameState.CurrentGameState == GameState.gameState.beginningCutScene)
        {
            return;
        }
        int targetLane = ((int)currentLane);
        //currentPos is now essentially a relic from attempts using MoveTowards rather than translate, which resulted
        //in inputs being queued rather than performing immediately as the button was pressed.
        Vector3 currentPos = player.transform.position; 

        //Lane switching
        //If the player is in the wrong lane, the x position of the player will be in the wrong lane,
        //so it corrects the position until it reaches the x position of the lane the player has input
        //to be in.
        if (Math.Abs(targetLane - player.transform.position.x) > 0.005)
        {
            if (player.transform.position.x > targetLane)
            {
                //Vector3 targetPos = new Vector3(currentPos.x - horizontalSpeed / 100, currentPos.y, currentPos.z);
                playerModel.transform.SetPositionAndRotation(playerModel.transform.position, Quaternion.Euler(Vector3.down * 30));
                player.transform.Translate(horizontalSpeed * Time.fixedDeltaTime * Vector3.left);
                
                //movementAnimations.SetBool("TurnLeft", false);
                //movementAnimations.SetBool("TurnRight", true);

            }
            else if (player.transform.position.x < targetLane)
            {
                //Vector3 targetPos = new Vector3(currentPos.x + horizontalSpeed / 100, currentPos.y, currentPos.z);
                playerModel.transform.SetPositionAndRotation(playerModel.transform.position, Quaternion.Euler(Vector3.up * 30));
                player.transform.Translate(horizontalSpeed * Time.fixedDeltaTime * Vector3.right);
                
                //movementAnimations.SetBool("TurnRight", false);
                //movementAnimations.SetBool("TurnLeft", true);
            }

        }
        else
        {
            playerModel.transform.SetPositionAndRotation(playerModel.transform.position, Quaternion.Euler(Vector3.zero));
            //movementAnimations.SetBool("TurnLeft", false);
            //movementAnimations.SetBool("TurnRight", false);
        }

        //Jumping
        //Increases the y value while deccelerating the amount that it increases by to simulate gravity affecting
        //the jump, then switches to falling once it reaches the max height.
        if (isJumping)
        {
            //animator.SetBool("isJumping", true);
            currentJumpVelocity += gravity * Time.fixedDeltaTime;
            //Vector3 targetPos = new Vector3(currentPos.x, maxJumpHeight, currentPos.z);
            player.transform.Translate(currentJumpVelocity * Time.fixedDeltaTime * Vector3.up);
            if (player.transform.position.y >= groundHeight + (willJumpBeSuper ? poweredJumpHeight : maxJumpHeight) || currentPlayerState == PlayerStates.staggered)
            {
                movementAnimations.SetBool("Falling", true);
                movementAnimations.SetBool("Jump", false);
                isJumping = false;
                isFalling = true;
            }

            //failsafe to cancel the jump from shooting downwards if the jump height is never reached
            //gives the designer a warning in the console if jump force is too weak
            if (player.transform.position.y < groundHeight)
            {
                movementAnimations.SetBool("Jump", false);
                player.transform.position = new Vector3(currentPos.x, groundHeight, currentPos.z);
                isJumping = false;
                Debug.LogWarning("JUMP FORCE NOT STRONG ENOUGH TO REACH REQUIRED HEIGHT, FIX PLEASE");
            }
        }
        //falling from jump
        //once the player has reached the max height, it accelerates downwards until it reaches the ground
        if (isFalling)
        {
            currentJumpVelocity -= gravity * Time.fixedDeltaTime;
            //Vector3 targetPos = new Vector3(currentPos.x, groundHeight, currentPos.z);
            player.transform.Translate(currentJumpVelocity * Time.fixedDeltaTime * Vector3.down);
            if (player.transform.position.y <= groundHeight)
            {
                if (willJumpBeSuper)
                {
                    tileMovement.movementSpeedGetterSetter = speedBeforeSuperJump;
                }
                movementAnimations.SetBool("Falling", false);
                player.transform.position = new Vector3(currentPos.x, groundHeight, currentPos.z);
                currentJumpVelocity = 0;
                isFalling = false;
            }
        }

        //Sliding while in the air
        //If the player is midair when they slide, they need to become grounded first before
        //it can play the animation (so it doesnt look weird).
        //acceleration modifier means the player reaches the ground quicker than they
        //regularly would if they were just falling from a jump.
        if (isTryingToSlide)
        {
            movementAnimations.SetBool("Falling", true);
            currentJumpVelocity -= gravity * Time.fixedDeltaTime * AccelerationModifier;
            player.transform.Translate(currentJumpVelocity * Time.fixedDeltaTime * Vector3.down);
            if (player.transform.position.y <= groundHeight)
            {
                if (willJumpBeSuper)
                {
                    tileMovement.movementSpeedGetterSetter = speedBeforeSuperJump;
                }

                movementAnimations.SetBool("Jump", false);
                movementAnimations.SetBool("Falling", false);
                playerAnimation.Play("Slide");
                movementAnimations.SetTrigger("Slide");
                audioManager.PlaySound("JumpWhoosh");
                //Invoke("EndSlideAnimation", 1.1f);

                player.transform.position = new Vector3(currentPos.x, groundHeight, currentPos.z);
                currentJumpVelocity = 0;
                isTryingToSlide = false;
            }

        }

        //if (currentPlayerState == PlayerStates.staggered)
        //{
        //    movementAnimations.SetBool("Stagger", true);
        //}
        //else
        //{
        //    movementAnimations.SetBool("Stagger", false);
        //}

        if (boulderIsClose)
        {
            movementAnimations.SetTrigger("LookBehind");
        }
    }
    #region LaneSwitchFunctions
    //Switches the lane state to the one the player just inputted
    public void GoLeft(InputAction.CallbackContext context)
    {

        switch (currentLane)
        {
            case Lanes.center:
                currentLane = Lanes.left;
                audioManager.PlaySound("SideWhoosh");
                break;
            case Lanes.right:
                currentLane = Lanes.center;
                audioManager.PlaySound("SideWhoosh");
                break;
        }
    }
    public void GoRight(InputAction.CallbackContext context)
    {
        
        switch (currentLane)
        {
            case Lanes.center:
                currentLane = Lanes.right;
                audioManager.PlaySound("SideWhoosh");
                break;
            case Lanes.left:
                currentLane = Lanes.center;
                audioManager.PlaySound("SideWhoosh");
                break;
        }
    }
    #endregion
    public void Jump(InputAction.CallbackContext context)
    {
        //doesnt jump if player is already midair, or staggered
        if (isJumping || isFalling || isTryingToSlide || currentPlayerState == PlayerStates.staggered) 
        {
            return;
        }

        willJumpBeSuper = false;
        //if super jump is active, it will increase the speed of the ground as if the player does a massive leap forwards
        if (IsSuperJumpActive)
        {
            
            willJumpBeSuper = true;
            speedBeforeSuperJump = tileMovement.movementSpeedGetterSetter;
            tileMovement.movementSpeedGetterSetter *= superJumpSpeedIncrease;
        }
        playerAnimation.Play("Jump");
        movementAnimations.SetBool("Jump", true);
        audioManager.PlaySound("JumpWhoosh");
        isJumping = true;
        currentJumpVelocity = jumpForce;//willJumpBeSuper ? poweredJumpForce : jumpForce;
    }

    public void Slide(InputAction.CallbackContext context)
    {
        
        if (currentPlayerState == PlayerStates.staggered || isTryingToSlide || isSliding)
        {
            return;
        }

        else if (!isJumping && !isFalling)
        {
            //plays the slide "animation", which essentially just shrinks and lowers the 
            //collision box of the player.
            playerAnimation.Play("Slide");
            movementAnimations.SetTrigger("Slide");
            //Invoke("EndSlideAnimation", 1.1f);
            audioManager.PlaySound("JumpWhoosh");
            return;
        }

        
        
        //triggers the jump cancel if player is in the air, allowing the player to quickly slide even if midair.
        isJumping = false;
        isFalling = false;
        isTryingToSlide = true;
        
    }

    //stores the speed at the time of the stagger then decreases it slightly
    public void Stagger()
    {
        playerAnimation.Play("Stagger");
        movementAnimations.SetTrigger("Stumble");
        PlayGrunt();

        switch (currentPlayerState)
        {
            case PlayerStates.running:
                //makes sure the speed goes back to normal before applying the decrease if super jump was active
                //when they staggered
                if (isPlayerAirborne && willJumpBeSuper)
                {
                    tileMovement.movementSpeedGetterSetter = speedBeforeSuperJump;
                }

                speedBeforeStagger = tileMovement.movementSpeedGetterSetter;
                //tileMovement.movementSpeedGetterSetter -= staggerSpeedDecrease;
                tileMovement.movementSpeedGetterSetter *= staggerSpeedDecrease;
                Invoke(nameof(ReturnToSpeed), staggerDuration);
                currentPlayerState = PlayerStates.staggered;
                break;
            case PlayerStates.staggered:
                CancelInvoke();
                tileMovement.movementSpeedGetterSetter *= secondStaggerSpeedDecrease;
                Invoke(nameof(ReturnToSpeed), staggerDuration);

                break;
        }
    }

    //public void ChangeJumpPower(bool isPowered)
    //{
    //    if (isPowered)
    //    {
    //        currentMaxJumpHeight = poweredJumpHeight;
    //        currentJumpForce = poweredJumpForce;
    //    }
    //    else
    //    {
    //        currentMaxJumpHeight = maxJumpHeight;
    //        currentJumpForce = jumpForce;
    //    }
    //}

    public void SetDeathState()
    {
        //sets player state, death state, and several animation/sound triggers. also disables input
        CancelSpeedReturn();
        movementAnimations.SetTrigger("Flatten");
        movementAnimations.SetBool("IsPlayerDead", true);
        currentPlayerState = PlayerStates.dead;
        DisablePlayerInput();
        gameState.CurrentGameState = GameState.gameState.gameOver;
        audioManager.StopSound(4);
        audioManager.PlaySound("GameOverMusic");
    }

    //Once the stagger has finished, returns back to the speed it was at before
    public void ReturnToSpeed()
    {
        tileMovement.movementSpeedGetterSetter = speedBeforeStagger;
        currentPlayerState = PlayerStates.running;
    }

    //Added the ability to cancel the invoke to prevent the ground from returning to the
    //state of moving if the player dies during the stagger phase (most likely due to the boulder)
    public void CancelSpeedReturn()
    {
        CancelInvoke();
    }

    //can play an animation from another function without requiring them to have the animation component
    public void PlayAnimation(string animationName)
    {
        playerAnimation.Play(animationName);
    }
    //used to play the grunt sound in the cutscene
    public void PlayGrunt()
    {
        audioManager.PlaySound("Grunt");
    }

    //pauses the game
    private void PauseButton(InputAction.CallbackContext obj)
    {
        pauseMenu.ShowPauseMenuScreen();
    }

    //used to enable/disable player input in the tutorial and after the player dies
    public void EnablePlayerInput()
    {
        movementInputActions.Player.Enable();
    }
    public void DisablePlayerInput()
    {
        movementInputActions.Player.Disable();
    }

    //private void EndSlideAnimation()
    //{
    //    Debug.Log("Ended sliding");
    //    movementAnimations.SetBool("Slide", false);
    //}


    /// <summary>
    /// This function will change throughout development to test certain features
    /// before their triggers have been implemented.
    /// 
    /// 24/06/22 - Stagger
    /// 28/06/22 - Stagger with boulder
    /// 13/07/22 - disabling input
    ///          - back to stagger (disable works)
    /// 18/08/22 - remembered to remove stagger before publishing
    /// 
    /// </summary>
    private void DebugButtonPressed(InputAction.CallbackContext obj)
    {
        //Stagger();
        //movementInputActions.Player.Disable();
    }

}
