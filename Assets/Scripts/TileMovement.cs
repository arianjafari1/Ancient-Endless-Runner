using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovement : MonoBehaviour
{
    //[SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject[] tilePrefabs; //array of tiles with tiles with obstacles
    [SerializeField] private GameObject[] mediumTilePrefabs; //array of tiles with tiles with obstacles, medium difficulty
    [SerializeField] private GameObject[] hardTilePrefabs; //array of tiles with tiles with obstacles, hard difficulty
    [SerializeField] private GameObject[] environmentTilePrefabs; //array of Environment tiles
    [SerializeField] private Transform targetZ; // the target z position for where the tile should go to
    [SerializeField] private Transform targetZright; // the target z position for where the right tile should go to

    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float maxMovementSpeed = 10f; //need more test to see when the game breaks
    [SerializeField] private float speedIncreaseEverySecond = 0.001f; //speed increase every second
    
    public enum Difficulty //creating an enum for the difficulty of the game
    {
        veryEasy, //100% easy Tiles
        easy, //80% easy Tiles, 20% medium Tiles
        easyMedium, // 60% easy Tiles, 40% Medium Tiles
        mediumEasy, // 60% medium Tiles, 40% Easy Tiles
        mediumHard, // 40% medium Tiles, 20% Hard Tiles, 40% easy Tiles
        hardMedium, // 40% medium Tiles, 25% hard Tiles, 35% easy Tiles
        hard //45% medium Tiles, 30% hard Tiles, 25% easy Tiles
    }

    private Difficulty difficulty; //declaring enum type as private


    public GameObject[] TilePrefabs //getters and setters for the tile prefabs array that we will instantiate using random in the Tile Manager Script
    {
        get
        {
            return tilePrefabs;
        }
        set
        {
            tilePrefabs = value;
        }
    }

    public GameObject[] EnvironmentTiles //getters and setters for the tile prefabs array that we will instantiate using random in the Tile Manager Script
    {
        get
        {
            return environmentTilePrefabs;
        }
        set
        {
            environmentTilePrefabs = value;
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

    public Transform TargetZright //getters and setters for the tile prefab that we will instantiate in the Tile Manager Script
    {
        get
        {
            return targetZright;
        }
        set
        {
            targetZright = value;
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

    public Difficulty currentDifficultyTile //getters and setters for the enum type to be used in TileManager to determine difficulty of Tiles
    {
        get
        {
            return difficulty;
        }
        set
        {
            difficulty = value;
        }
    }



}
