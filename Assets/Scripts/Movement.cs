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
    ///          - 
    ///          - Removed switchPlayerStagger variables linked to boulder movement and replaced with PlayerState
    ///          
    ///            
    /// </summary>
    [SerializeField] private float horizontalSpeed;
    private enum Lanes
        {
            left = 3,
            center = 0,
            right = -3
        };
    [SerializeField] private GameObject player;
    public enum PlayerStates
    {
        running = 2,
        staggered = 1,
        dead = 0
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
    private InputAction debugButton;

    private Lanes currentLane;

    [Tooltip("Changes rate of acceleration (must be negative)")]
    [SerializeField]private float gravity = -9.8f;
    [Tooltip("How quickly the player jumps")]
    [SerializeField] private float jumpForce;
    private float currentJumpVelocity;
    private float groundHeight;
    [Tooltip("How high the player jumps")]
    [SerializeField] private float maxJumpHeight;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isTryingToSlide = false;
    [Tooltip("Acceleration modifier for jump cancels")]
    [SerializeField] private float AccelerationModifier;


    private Animation playerAnimation;
    //[SerializeField] private Animator animator; (too much for my brain to handle, went back to simple animation component)

    [SerializeField] TileMovement tileMovement;
    [Tooltip("Between 0-1, the reduction of speed while player staggers")]
    [SerializeField] private float staggerSpeedDecrease;
    [Tooltip("Between 0-1 but above staggerSpeedDecrease, the secondary reduction of speed decrease. Set to 1 if you dont want further decrease from initial")]
    [SerializeField] private float secondStaggerSpeedDecrease;
    [SerializeField] private float staggerDuration;
    private float speedBeforeStagger;
    [SerializeField] private BoulderMovement boulderMovement;
    [SerializeField] private ScoreManager scoreManager;

    private void Awake()
    {
        movementInputActions = new MovementInputActions();
        
        
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
        left.Enable();
        right.Enable();
        jump.Enable();
        slide.Enable();
        left.performed += GoLeft;
        right.performed += GoRight;
        jump.performed += Jump;
        slide.performed += Slide;

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
        debugButton.Disable();
    }


    /// <summary>
    /// Sets the player to always start in the center lane, and gets the y position of the player
    /// to work with during the landing function so it knows where to stop falling
    /// </summary>
    void Start()
    {
        currentLane = Lanes.center;
        currentPlayerState = PlayerStates.running;
        groundHeight = player.transform.position.y;
        playerAnimation = gameObject.GetComponent<Animation>();
    }



    /// <summary>
    /// FixedUpdate is used to make sure that movement is smooth and consistant, even on lower or 
    /// much higher performance devices.
    /// </summary>
    void FixedUpdate()
    {
        int targetLane = ((int)currentLane);
        //currentPos is now virtually a relic from attempts using MoveTowards rather than translate, which resulted
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
                player.transform.Translate(horizontalSpeed * Time.fixedDeltaTime * Vector3.left);
            }
            else if (player.transform.position.x < targetLane)
            {
                //Vector3 targetPos = new Vector3(currentPos.x + horizontalSpeed / 100, currentPos.y, currentPos.z);
                player.transform.Translate(horizontalSpeed * Time.fixedDeltaTime * Vector3.right);
            }
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
            if (player.transform.position.y >= groundHeight + maxJumpHeight)
            {
                player.transform.position = new Vector3(currentPos.x, groundHeight + maxJumpHeight, currentPos.z);
                isJumping = false;
                isFalling = true;
                Debug.Log("Height reached");
            }

            //failsafe to cancel the jump from shooting downwards if the jump height is never reached
            //gives the designer a warning in the console if jump force is too weak
            if (player.transform.position.y < groundHeight)
            {
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
                player.transform.position = new Vector3(currentPos.x, groundHeight, currentPos.z);
                currentJumpVelocity = 0;
                isFalling = false;
                Debug.Log("Back to ground");
            }
        }

        //Sliding while in the air
        //If the player is midair when they slide, they need to become grounded first before
        //it can play the animation (so it doesnt look weird).
        //acceleration modifier means the player reaches the ground quicker than they
        //regularly would if they were just falling from a jump.
        if (isTryingToSlide)
        {
            currentJumpVelocity -= gravity * Time.fixedDeltaTime * AccelerationModifier;
            player.transform.Translate(currentJumpVelocity * Time.fixedDeltaTime * Vector3.down);
            if (player.transform.position.y <= groundHeight)
            {
                playerAnimation.Play("Slide");
                player.transform.position = new Vector3(currentPos.x, groundHeight, currentPos.z);
                currentJumpVelocity = 0;
                isTryingToSlide = false;
                Debug.Log("Cancelled jump into slide");
            }

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
                break;
            case Lanes.right:
                currentLane = Lanes.center;
                break;
        }
        Debug.Log(currentLane);
    }
    public void GoRight(InputAction.CallbackContext context)
    {
        
        switch (currentLane)
        {
            case Lanes.center:
                currentLane = Lanes.right;
                break;
            case Lanes.left:
                currentLane = Lanes.center;
                break;
        }
        Debug.Log(currentLane);
    }
    #endregion
    public void Jump(InputAction.CallbackContext context)
    {
        if (!isJumping && !isFalling) 
        {
            playerAnimation.Play("Jump");
            currentJumpVelocity = jumpForce;
            isJumping = true;
        }
    }

    public void Slide(InputAction.CallbackContext context)
    {
        //plays the slide "animation", which essentially just shrinks and lowers the 
        //collision box of the player.
        //TODO: Will need to be matched up with the animation once that is imported

        if (!isJumping && !isFalling)
        {
            playerAnimation.Play("Slide");
            Debug.Log("Slid");
        }
        else
        {
            //triggers the jump cancel, allowing the player to quickly slide even if midair.
            isJumping = false;
            isFalling = false;
            isTryingToSlide = true;
        }
    }

    //stores the speed at the time of the stagger then decreases it slightly
    //TODO: also will need to be matched up to the animation
    public void Stagger()
    {
        playerAnimation.Play("Stagger");
        Debug.Log("Staggered");

        switch (currentPlayerState)
        {
            case PlayerStates.running:
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

    //Once the stagger has finished, returns back to the speed it was at before
    public void ReturnToSpeed()
    {
        tileMovement.movementSpeedGetterSetter = speedBeforeStagger;
        currentPlayerState = PlayerStates.running;
        Debug.Log("Speed back up");
    }

    //Added the ability to cancel the invoke to prevent the ground from returning to the
    //state of moving if the player dies during the stagger phase (most likely due to the boulder)s
    public void CancelSpeedReturn()
    {
        CancelInvoke();
    }

    public void PlayAnimation(string animationName)
    {
        playerAnimation.Play(animationName);
    }
    

    /// <summary>
    /// This function will change throughout development to test certain features
    /// before their triggers have been implemented.
    /// 
    /// Functions tested:
    /// 24/06/22 - Stagger
    /// 28/06/22 - Stagger with boulder
    /// 
    /// </summary>
    private void DebugButtonPressed(InputAction.CallbackContext obj)
    {
        Stagger();
    }

}
