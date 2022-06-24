using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 0.1f;
    [SerializeField] private float maxMovementSpeed = 0.8f; //need more test to see when the game breaks
    [SerializeField] private float speedIncreaseEverySecond = 0.0001f; //speed increase every second

    public float movementSpeedGetterSetter //getters and setters for movement speed
    {
        get
        {
            return movementSpeed;
        }
        set
        {
            if (value < 0)
                movementSpeed = 0;
            else
                movementSpeed = value;
        }
    }

    public float maxMovementSpeedGetter //getters and setters for movement speed
    {
        get
        {
            return maxMovementSpeed;
        }

    }

    public float speedIncreaseEverySecondGetterSetter //getters and setters for movement speed
    {
        get
        {
            return speedIncreaseEverySecond;
        }
        set
        {
            if (value < 0)
                speedIncreaseEverySecond = 0;
            else
                speedIncreaseEverySecond = value;
        }
    }

}
