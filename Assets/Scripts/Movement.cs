using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
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
    ///
    /// </summary>
    [SerializeField] private float horizontalSpeed;
    private enum Lanes
        {
            left = -3,
            center = 0,
            right = 3
        };
    [SerializeField] private GameObject player;
    private MovementInputActions movementInputActions;
    private InputAction left;
    private InputAction right;
    private InputAction jump;
    private InputAction slide;

    private Lanes currentLane;

    [Tooltip("Changes rate of acceleration (must be negative)")]
    [SerializeField]private float gravity = -9.8f;
    [Tooltip("How quickly the player jumps")]
    [SerializeField] private float jumpForce;
    private float currentJumpVelocity;
    private float groundHeight;
    [Tooltip("How high the player's jump reaches")]
    [SerializeField] private float maxJumpHeight;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isTryingToSlide = false;
    [Tooltip("Acceleration modifier for jump cancels")]
    [SerializeField] private float AccelerationModifier;


    private Animation playerAnimation;
    //[SerializeField] private Animator animator;

    [SerializeField] TileManager tileManager;

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
    }
    private void OnDisable()
    {
        left.Disable();
        right.Disable();
        jump.Disable();
        slide.Disable();
    }


    /// <summary>
    /// Sets the player to always start in the center lane, and gets the y position of the player
    /// to work with during the landing function so it knows where to stop falling
    /// </summary>
    void Start()
    {
        currentLane = Lanes.center;
        groundHeight = player.transform.position.y;
        playerAnimation = gameObject.GetComponent<Animation>();
    }

    void Update()
    {

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
                player.transform.Translate(Vector3.left * horizontalSpeed * Time.fixedDeltaTime);
            }
            else if (player.transform.position.x < targetLane)
            {
                //Vector3 targetPos = new Vector3(currentPos.x + horizontalSpeed / 100, currentPos.y, currentPos.z);
                player.transform.Translate(Vector3.right * horizontalSpeed * Time.fixedDeltaTime);
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
            player.transform.Translate(Vector3.up * currentJumpVelocity * Time.fixedDeltaTime);
            if (player.transform.position.y >= maxJumpHeight)
            {
                player.transform.position = new Vector3(currentPos.x, maxJumpHeight, currentPos.z);
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
            player.transform.Translate(Vector3.down * currentJumpVelocity * Time.fixedDeltaTime);
            if (player.transform.position.y <= groundHeight)
            {
                player.transform.position = new Vector3(currentPos.x, groundHeight, currentPos.z);
                currentJumpVelocity = 0;
                isFalling = false;
                Debug.Log("Back to ground");
            }
        }

        //Sliding
        if (isTryingToSlide)
        {
            currentJumpVelocity -= gravity * Time.fixedDeltaTime * AccelerationModifier;
            player.transform.Translate(Vector3.down * currentJumpVelocity * Time.fixedDeltaTime);
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

    public void Stagger()
    {
        playerAnimation.Play("Stagger");
    }



}
