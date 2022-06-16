using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    /// <summary>
    /// Code by Malachi
    /// </summary>
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private enum Lanes
        {
            left = -4,
            center = 0,
            right = 4
        };
    [SerializeField] private GameObject player;
    private MovementInputActions movementInputActions;
    private InputAction left;
    private InputAction right;
    private InputAction jump;
    private InputAction slide;

    private Lanes currentLane;

    private float gravity = -9.8f;
    [Tooltip("How quickly the player jumps")]
    [SerializeField] private float jumpForce;
    private float currentJumpVelocity;
    private float groundHeight;
    [Tooltip("How high the player's jump reaches")]
    [SerializeField] private float maxJumpHeight;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isSliding = false;

    private void Awake()
    {
        movementInputActions = new MovementInputActions();
        
        
    }
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


    // Start is called before the first frame update
    void Start()
    {
        currentLane = Lanes.center;
        groundHeight = player.transform.position.y;
    }

    void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int targetLane = ((int)currentLane);
        Vector3 currentPos = player.transform.position;

        //Lane switching
        if (Math.Abs(targetLane - player.transform.position.x) > 0.005)
        {
            if (player.transform.position.x > targetLane)
            {
                Vector3 targetPos = new Vector3(currentPos.x - horizontalSpeed / 100, currentPos.y, currentPos.z);
                player.transform.Translate(Vector3.left * horizontalSpeed / 10);
            }
            else if (player.transform.position.x < targetLane)
            {
                Vector3 targetPos = new Vector3(currentPos.x + horizontalSpeed / 100, currentPos.y, currentPos.z);
                player.transform.Translate(Vector3.right * horizontalSpeed / 10);
            }
        }

        //Jumping
        if (isJumping)
        {
            currentJumpVelocity += gravity * Time.fixedDeltaTime;
            Vector3 targetPos = new Vector3(currentPos.x, maxJumpHeight, currentPos.z);
            //player.transform.position = Vector3.MoveTowards(currentPos, targetPos, currentJumpVelocity * Time.fixedDeltaTime);
            player.transform.Translate(Vector3.up * currentJumpVelocity * Time.fixedDeltaTime);
            if (player.transform.position.y >= maxJumpHeight)
            {
                player.transform.position = new Vector3(currentPos.x, maxJumpHeight, currentPos.z);
                //currentJumpVelocity = 0;
                isJumping = false;
                isFalling = true;
                Debug.Log("Height reached");
            }
        }
        //falling from jump
        if (isFalling)
        {
            currentJumpVelocity -= gravity * Time.fixedDeltaTime;
            Vector3 targetPos = new Vector3(currentPos.x, groundHeight, currentPos.z);
            //player.transform.position = Vector3.MoveTowards(currentPos, targetPos, currentJumpVelocity * Time.fixedDeltaTime);
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
        if (isSliding)
        {
            //
        }


    }

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

    public void Jump(InputAction.CallbackContext context)
    {
        if (!isJumping && !isFalling) 
        {
            currentJumpVelocity = jumpForce;
            isJumping = true;
        }
    }

    public void Slide(InputAction.CallbackContext context)
    {
        isSliding = false;
    }




}
