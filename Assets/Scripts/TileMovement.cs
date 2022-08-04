using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TileMovement : MonoBehaviour
{
    //Code done by Arian - Start

    /// <summary>
    /// 22/06/2022 -[Arian] created TileMovement script
    ///            -[Arian] moved the movementSpeed, maxSpeed, speedIncrease to this script from TileManager script, to make them independent from the Tile objects
    ///            -[Arian] added getters and setters to the variables I just moved here, so they can be used in the TileManager
    /// 27/06/2022 -[Arian] moved the tilePrefabs GameObject to TileMovement from TileManager, and Serialized it, also added getter and setter for it so it can be used in TileManager
    /// 28/06/2022 -[Arian] made the tilePrefabs GameObject to an array of GameObjects and used it in TileManager
    /// 01/07/2022 -[Arian] move targetZ from TileManager to here, and created targetZright as well, to have to point for both environment and middle tiles to have individul lerp points
    /// 05/07/2022 -[Arian] Added a medium and a hard difficulty tile prefabs, which will change between them based on a future difficulty state that I will create in another script
    /// 06/07/2022 -[Arian] Added an array of GameObject type for the environment tiles, environmentTilePrefabs
    /// 12/07/2022 -[Arian] Created a Difficulty type variable using an enum for different difficulty states
    /// 14/07/2022 -[Arian] Moved the  Difficulty enum to GameState
    /// 01/08/2022 -[Arian] to implement Object Pooling for Tiles, I have created a foreach loop in Awake that loops through the elements of the tilePrefabs array,
    ///            instantiates the elements of the array, sets them to inactive, and adds them to the pooledObjects list
    ///            -[Arian] then I made a getter function where it loops throug the pooledObjects list and returns the pooled object if it's not active
    ///            this getter function will be used in TileManager script to "spawn" new tiles
    ///02/08/2022  -[Arian] the magnet coin system made by my teammate breaks as it didn't take into account that I would implement object pooling for the tile system later, so
    ///            at the request of the producers/designers, I have reversed back to the older less efficient system that it's proven to work, though I will
    ///            leave the code for the object pooling in the TileDestroyer, TileMovement and TileManager scripts as comments so they can still be looked at for marking
    /// </summary>


    /// Commented out Object Pooling Starts
    //public static ObjectPool<GameObject> instance;
    //private List<GameObject> pooledObjects = new List<GameObject>();
    //private int amountToPool;
    /// Commented out Object Pooling Ends


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


    /// Commented out Object Pooling Starts

    //private void Awake()
    //{

    //    foreach (GameObject tiles in tilePrefabs)
    //    {
    //        GameObject tile = Instantiate(tiles, targetZ.position, targetZ.rotation);
    //        tile.SetActive(false);
    //        pooledObjects.Add(tile);
    //    }
    //    amountToPool = pooledObjects.Count;
    //}

    //public GameObject getPooledTiles()
    //{
    //    for (int i = 0; i < amountToPool; i++)
    //    {
    //        if (!pooledObjects[i].activeInHierarchy)
    //        {

    //            return this.pooledObjects[i];

    //        }

    //    }
    //    return null;
    //}
    /// Commented out Object Pooling Ends


    public GameObject[] EasyTilePrefabs //getters and setters for the tile prefabs array that we will instantiate using random in the Tile Manager Script
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

    public GameObject[] MediumTilePrefabs //getters and setters for the tile prefabs array that we will instantiate using random in the Tile Manager Script
    {
        get
        {
            return mediumTilePrefabs;
        }
        set
        {
            mediumTilePrefabs = value;
        }
    }

    public GameObject[] HardTilePrefabs //getters and setters for the tile prefabs array that we will instantiate using random in the Tile Manager Script
    {
        get
        {
            return hardTilePrefabs;
        }
        set
        {
            hardTilePrefabs = value;
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




    //Code done by Arian - End
}
