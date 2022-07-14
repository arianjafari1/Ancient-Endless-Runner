using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    /// Code done by Arian- start
    private TileMovement tileMovement;
    private GameState gameState;

    private GameObject[] tilePrefabs;
    private int randomTile; //random Tile number
    private Vector3 targetZ;
    [SerializeField] bool tileRight;
    
    [SerializeField] private Transform tilePosition; //getting the position of tile
    private float t = 0.1f; //target point used for lerp in the fixed update to move the tile backwards

    [SerializeField] private Transform tileSpawnStart;
    [SerializeField] private Transform groundLength; //ground child object of tile


    /// Code done by Arian- end

    // Start is called before the first frame update
    private void Awake()
    {
        /// Code done by Arian- start
        tileMovement = FindObjectOfType<TileMovement>(); //reference the object with code
        gameState = FindObjectOfType<GameState>(); //reference the object with code

        tilesAndDifficulty(); //calling the function that takes care of the tiles based on the current difficulty of the game

        randomTile = Random.Range(0, tilePrefabs.Length); // take a range from 0 to the tile prefab length to determine which tile to spawn from the array randomly
        
        /// Code done by Arian- end

    }


    private void FixedUpdate()
    {
        /// Code done by Arian- start

        if (tilePosition != null) //preventing an error by first checking if transform tilePosition is not null
        {
            //combining MoveTowards and Lerp to move the tiles towards the camera, MoveTowards for constant speed, and lerp for smooth movement:
            Vector3 a = tilePosition.transform.position; //assigning Vector3 the position of tilePosition
            Vector3 b = targetZ; //assigning Vector3 b the postion of targetZ
            tilePosition.transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, t), tileMovement.movementSpeedGetterSetter * Time.fixedDeltaTime); //using movetowards and lerp to move tile backwards at constant speed
        }

        if (tileMovement.movementSpeedGetterSetter > tileMovement.maxMovementSpeedGetter + tileMovement.maxMovementSpeedGetter * 20 / 100) //check if movement speed is higher than maximum speed + 20 %
        {
            Debug.LogError("Movement speed should be lower than " + tileMovement.maxMovementSpeedGetter); //custom error message in case movementSpeed goes over the maximum speed
            return; //returning nothing
        }


        if (tileMovement.movementSpeedGetterSetter < tileMovement.maxMovementSpeedGetter) //if the movement speed is lower than 0.5 then
        {
            tileMovement.movementSpeedGetterSetter += tileMovement.speedIncreaseEverySecondGetterSetter * Time.fixedDeltaTime; //increase speed by this amount
        }
        /// Code done by Arian- end

    }


    /// Code done by Arian- start
    private void OnTriggerEnter(Collider collisionInfo) //check for collision infor
    {


        if (collisionInfo.gameObject.CompareTag("Player")) //if collision info comapre with the tile death point tag, then execute the code undeneath
        {
            Instantiate(tilePrefabs[randomTile], tileSpawnStart.position, tileSpawnStart.rotation); // use random tile to instantiate a new tile, at the tile spawn point
            Debug.Log("New Tile spawned");

        }



    }


    private void tilesAndDifficulty()
    {
        //last number in int Random.Range is exclusive
        int percentageChance = Random.Range(1, 11); //get a number between 1 and 10 and based on that determine chance so we can use it down in the
                                                    // switch statement to determine which tile should be spawned

        switch (tileRight) //switch statement takes the expression of bool tileRight to check whether the envirnment tiles or the right tiles are active
        {
            case false: //in case the envirnment tiles are inactive, then excute this
                targetZ = tileMovement.TargetZ.position; // moveTowards and lerp will drag the tiles in the middle in a straight line to targetZ

                if (gameState.currentDifficultyTile == GameState.Difficulty.veryEasy)
                {
                    tilePrefabs = tileMovement.EasyTilePrefabs;
                    
                }
                else if (gameState.currentDifficultyTile == GameState.Difficulty.easy && percentageChance <= 8) //check for the game state difficulty
                                                                                                                //then for the percentage change 
                {
                    tilePrefabs = tileMovement.EasyTilePrefabs; //then based on that select which Tile prefab array should there be a tile taken from
                    
                }
                else if (gameState.currentDifficultyTile == GameState.Difficulty.easy && percentageChance > 8)
                {
                    tilePrefabs = tileMovement.MediumTilePrefabs;
                    
                }

                if (gameState.currentDifficultyTile == GameState.Difficulty.easyMedium && percentageChance <= 6)
                {
                    tilePrefabs = tileMovement.EasyTilePrefabs;
                    
                }
                else if (gameState.currentDifficultyTile == GameState.Difficulty.easyMedium && percentageChance > 6)
                {
                    tilePrefabs = tileMovement.MediumTilePrefabs;
                    
                }

                if (gameState.currentDifficultyTile == GameState.Difficulty.mediumEasy && percentageChance <= 6)
                {
                    tilePrefabs = tileMovement.MediumTilePrefabs;
                    
                }
                else if (gameState.currentDifficultyTile == GameState.Difficulty.mediumEasy && percentageChance > 6)
                {
                    tilePrefabs = tileMovement.EasyTilePrefabs;
                    
                }

                if (gameState.currentDifficultyTile == GameState.Difficulty.mediumHard && percentageChance <= 4)
                {
                    tilePrefabs = tileMovement.MediumTilePrefabs;
                    
                }
                else if (gameState.currentDifficultyTile == GameState.Difficulty.mediumHard && percentageChance > 4 && percentageChance < 7)
                {
                    tilePrefabs = tileMovement.HardTilePrefabs;
                    
                }
                else if (gameState.currentDifficultyTile == GameState.Difficulty.mediumHard && percentageChance > 6)
                {
                    tilePrefabs = tileMovement.EasyTilePrefabs;
                    
                }

                if (gameState.currentDifficultyTile == GameState.Difficulty.hardMedium && percentageChance <= 4)
                {
                    tilePrefabs = tileMovement.MediumTilePrefabs;
                    
                }
                else if (gameState.currentDifficultyTile == GameState.Difficulty.hardMedium && percentageChance > 4 && percentageChance < 8)
                {
                    tilePrefabs = tileMovement.HardTilePrefabs;
                    
                }
                else if (gameState.currentDifficultyTile == GameState.Difficulty.hardMedium && percentageChance > 7)
                {
                    tilePrefabs = tileMovement.EasyTilePrefabs;
                    
                }

                if (gameState.currentDifficultyTile == GameState.Difficulty.hard && percentageChance <= 5)
                {
                    tilePrefabs = tileMovement.MediumTilePrefabs;
                    
                }
                else if (gameState.currentDifficultyTile == GameState.Difficulty.hard && percentageChance > 5 && percentageChance < 9)
                {
                    tilePrefabs = tileMovement.HardTilePrefabs;
                    
                }
                else if (gameState.currentDifficultyTile == GameState.Difficulty.hard && percentageChance > 8)
                {
                    tilePrefabs = tileMovement.EasyTilePrefabs;
                    
                }

                break;

            case true:
                targetZ = tileMovement.TargetZright.position; // moveTowards and lerp will drag the tiles in the on the left and right in a straight line to a new targetZ
                tilePrefabs = tileMovement.EnvironmentTiles; // and the tilePrefab array that tiles spawn from, is going to be the array for the envirnment tiles
                break;
        }
    }
    /// Code done by Arian- end



}
