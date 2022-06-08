using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
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

    private Lanes currentLane;
    private Vector2 inputDirection;

    private void Awake()
    {
        movementInputActions = new MovementInputActions();

        
    }
    private void OnEnable()
    {
        //move = movementInputActions.Player.Move;
        //move.Enable();
        left = movementInputActions.Player.Left;
        right = movementInputActions.Player.Right;
        left.Enable();
        right.Enable();
        left.performed += GoLeft;
        right.performed += GoRight;
    }
    private void OnDisable()
    {
        //move.Disable();
        left.Disable();
        right.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        currentLane = Lanes.center;
    }

    void Update()
    {
        //inputDirection = move.ReadValue<Vector2>();
        //if (inputDirection.x <= -0.5) { GoLeft(); }
        //else if (inputDirection.x >= 0.5) { GoRight(); }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int targetLane = ((int)currentLane);
        Vector3 currentPos = player.transform.position;

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






}
