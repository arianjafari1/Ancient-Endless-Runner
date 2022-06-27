using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovement : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform targetZ; // the target z position for where the tile should go to (behind the camera)

    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float maxMovementSpeed = 10f; //need more test to see when the game breaks
    [SerializeField] private float speedIncreaseEverySecond = 0.001f; //speed increase every second


    public GameObject TilePrefab //getters and setters for the tile prefab that we will instantiate in the Tile Manager Script
    {
        get
        {
            return tilePrefab;
        }
        set
        {
            tilePrefab = value;
        }
    }

    public Transform TargetZ //getters and setters for the tile prefab that we will instantiate in the Tile Manager Script
    {
        get
        {
            return targetZ;
        }
        set
        {
            targetZ = value;
        }
    }


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
