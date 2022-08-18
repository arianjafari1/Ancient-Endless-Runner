using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    /// Code done by Arian- start

    /// <summary>
    /// 12/06/2022 -[Arian] Created the TileManager Script
    ///            -[Arian] Added movement speed and targetZ position for where the Tiles should move towards
    ///            -[Arian] used move towars and lerp combined in the fixed update to insure smooth movement of the tiles
    ///            -[Arian] got the tiles to despawn when they reach the death point which is targetZ
    /// 14/06/2022 -[Arian] added a tilePrefab array to store tiles that will spawn, and also added a spawnPoint pos for the tiles
    /// 15/06/2022 -[Arian] moved the tileDeath collision checker to the TileDestroyer Script
    ///            -[Arian] added a collission checker to check whether the player is near the edge of the tile, so tiles spawn ahead
    /// 17/06/2022 -[Arian] added maxSpeed and speedIncrease variables
    ///            -[Arian] Added a LogError message in case movement speed is higher than the max speed
    ///            -[Arian] Added increase movement speed for as long as current speed is lower than maxSpeed
    /// 22/06/2022 -[Arian] Moved the movementSpeed, speedIncrease and maxSpeed variables to a newly created script, TileMovement that this scripts inherits from
    ///            -[Arian] referenced the TileMovement script so I can use its variables
    /// 27/06/2022 -[Arian] moved the tilePrefabs GameObject to TileMovement
    ///            -[Arian] multiplied movement speed in moveTowards/Lerp methods with Time.fixedDeltaTime
    /// 28/06/2022 -[Arian] added randomNumber as, randomTile, from 0 to the length of the tilePrefab array from tileMovement to use to spawn random Tiles from that array
    /// 01/07/2022 -[Arian] moved Serialized targetZ to the Tile Movement, but kept a targetZ variable as a Vector3 to be assigned to the pos of targetZ from TileMovement
    ///            -[Arian] added boolean tileRight to the script, if true, then it means they are the Environment tiles, otherwise they are the tiles in the middle
    ///            -[Arian] added if statement checking to tileRight, based on that, we assign targetZ to know where the Environment and middle tiles each go
    /// 06/07/2022 -[Arian] created an array of type GameObject for tilePrefabs, to assign to the tile Prefabs arrays from TileMovement script based on
    ///             whether it is an environment tile or a middle tile, using an if statement
    /// 07/07/2022 -[Arian] moved the randomTile (random range for getting a random number to randomly spawn tiles) to the end of the Awake function
    ///            to make sure that it gets the tile lenght after it know which Tile array it should get it from according to my previous if statements
    /// 14/07/2022 -[Arian] moved all of the if statements regarding deciding whether tilePrefabs array is the middle tiles or Environment Tiles to a function
    ///            called tilesAndDifficulties()
    ///            -[Arian] changed the if statements in tilesAndDifficulties() to a switch statement
    ///            -[Arian] added a random range from 1 to 10 in the function tilesAndDifficulties() to get a chance for which tile difficulty array it should pull from
    ///            -[Arian] using if statements I added checks in the case for middle Tiles based on the current Difficulty state from GameState script,
    ///            and the chance, so that it pulls Tiles from different difficulties array
    /// 31/07/2022 -[Arian] moved most of the checks for difficulty and chance out of the tilesAndDifficulty method and put it in its on function (checkTheDifficulty) for readablity
    ///            -[Arian] split the checks for the highest difficulty from the checkTheDifficulty method into its own method, checkTheHardDifficulty
    ///            -[Arian] moved the percentageChance declaration out of its function so it's accessible anywhere in this script
    ///            -[Arian] added switch statement in the method tilesAndDifficulty for when tileEnvironment is false to check whether the highest difficulty has been reached
    ///            , if the highest difficulty has been reached it doesn't do the the addition unnecessary checks for the lower difficulties
    ///01/08/2022  -[Arian] changed the instantiation for the Tiles in the Middle to the Object Pooling that I have done in the TileMovement
    ///            in the if statement that checks collision 'if (collisionInfo.gameObject.CompareTag("Player")) '
    ///            -[Arian] object Pooling for the Tiles in the Middle is fully working, though for now it's only pooling from the EasyTiles array
    ///02/08/2022  -[Arian] the magnet coin system made by my teammate breaks as it didn't take into account that I would implement object pooling for the tile system later, so
    ///            at the request of the producers/designers, I have reversed back to the older less efficient system that it's proven to work, though I will
    ///            leave the code for the object pooling in the TileDestroyer, TileMovement and TileManager scripts as comments so they can still be looked at for marking
    /// </summary>

    private TileMovement tileMovement;
    private GameState gameState;

    private GameObject[] tilePrefabs;
    private int randomTile; //random Tile number
    private Vector3 targetZ; // the target 
    [SerializeField] bool tileEnvironment; //bolean to check whether a tile is the tileEnvironment or the tiles in the middle

    [SerializeField] private Transform tilePosition; //getting the position of tile
    private float t = 0.1f; //target point used for lerp in the fixed update to move the tile backwards

    [SerializeField] private Transform tileSpawnStart;
    [SerializeField] private Transform groundLength; //ground child object of tile

    private int percentageChance; //I will assign this to get a number between 1 and 10 and based on that determines the chance, so we can use it down in the
                                                // switch statement of tilesAndDifficulty to determine which tile should be spawned. [16/08/22 Oakley] 

    /// Code done by Arian- end
    /* [ 15/08/22 - Oakley] Added a bunch of open variables that can be tweaked for individual weighting of difficulty chances.
                            Explainer: Every difficulty level has it's own set of weightings (integers) for easy, medium and hard chances. These weightings can be set in the
                            inspector. These weightings are then implemented into the difficulty system created by Arian. When Arian's code spawns a tile, it checks the difficulty.
                            When it gets the difficulty level it then executes that specific spawner. It takes the weightings and converts them to percentages. The code then
                            gets a random number from 1-100 (percentage chance). It then checks to see if the result is within the range for easy, if true it spawns an easy tile.
                            If false, it checks if the result is in the range for a medium tile and spawns it if true. If both of these checks are false it spawns a hard tile.
                            Some of the code here is redundant and messy due to my inexperience, but we ran out of time for Arian to clean it up for me. 
    /*/
    [SerializeField] private int veasyEasyWeight;
    [SerializeField] private int veasyMediumWeight;
    [SerializeField] private int veasyHardWeight;
    private int veasyTotal;
    [SerializeField] private int easyEasyWeight;
    [SerializeField] private int easyMediumWeight;
    [SerializeField] private int easyHardWeight;
    private int easyTotal;
    [SerializeField] private int easymedEasyWeight;
    [SerializeField] private int easymedMediumWeight;
    [SerializeField] private int easymedHardWeight;
    private int easymedTotal;
    [SerializeField] private int medeasyEasyWeight;
    [SerializeField] private int medeasyMediumWeight;
    [SerializeField] private int medeasyHardWeight;
    private int medeasyTotal;
    [SerializeField] private int mediumEasyWeight;
    [SerializeField] private int mediumMediumWeight;
    [SerializeField] private int mediumHardWeight;
    private int mediumTotal;
    [SerializeField] private int medhardEasyWeight;
    [SerializeField] private int medhardMediumWeight;
    [SerializeField] private int medhardHardWeight;
    private int medhardTotal;
    [SerializeField] private int hardmedEasyWeight;
    [SerializeField] private int hardmedMediumWeight;
    [SerializeField] private int hardmedHardWeight;
    private int hardmedTotal;
    [SerializeField] private int hardEasyWeight;
    [SerializeField] private int hardMediumWeight;
    [SerializeField] private int hardHardWeight;
    private int hardTotal;

    // Start is called before the first frame update
    private void Awake()
    {
        /// Code done by Arian- start
        tileMovement = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TileMovement>(); //reference the object with script
        gameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameState>(); //reference the object with script

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
            //Debug.LogError("Movement speed should be lower than " + tileMovement.maxMovementSpeedGetter); //custom error message in case movementSpeed goes over the maximum speed
            return; //returning nothing
        }


        if (tileMovement.movementSpeedGetterSetter < tileMovement.maxMovementSpeedGetter) //if the movement speed is lower than maxSpeed
        {
            tileMovement.movementSpeedGetterSetter += tileMovement.speedIncreaseEverySecondGetterSetter * Time.fixedDeltaTime; //increase speed by this amount
        }
        /// Code done by Arian- end

    }


    /// Code done by Arian- start
    private void OnTriggerEnter(Collider collisionInfo) //check for collision infor
    {


        if (collisionInfo.gameObject.CompareTag("Player")) //if collision info compares with the tile death point tag, then execute the code undeneath
        {
                Instantiate(tilePrefabs[randomTile], tileSpawnStart.position, tileSpawnStart.rotation); // use random tile to instantiate a new tile, at the tile spawn point
                
            
            /// Commented out Object Pooling Starts
            //GameObject easyTiles = tileMovement.getPooledTiles();

            //if (easyTiles != null)
            //{
            //    //GameObject tilePrefab = tilePrefabs[randomTile];
            //    easyTiles.transform.position = tileSpawnStart.position;
            //    easyTiles.SetActive(true);

            //    Transform[] inactiveChildren = easyTiles.transform.GetComponentsInChildren<Transform>(true);
            //    for (int i = 0; i < inactiveChildren.Length; i++)
            //    {
            //        inactiveChildren[i].gameObject.SetActive(true);

            //    }
            //}
            /// Commented out Object Pooling Ends


        }



    }


    private void tilesAndDifficulty()
    {
        //last number in int Random.Range is exclusive
        //percentageChance = Random.Range(1, 11); //get a number between 1 and 10 and based on that determine chance so we can use it down in the
                                                    // switch statement to determine which tile should be spawned

        switch (tileEnvironment) //switch statement takes the expression of bool tileEnvironment to check whether the envirnment tiles or the right tiles are active
        {
            case false: //in case the envirnment tiles are inactive, then excute this
                targetZ = tileMovement.TargetZ.position; // moveTowards and lerp will drag the tiles in the middle in a straight line to targetZ
                
                switch (gameState.currentDifficultyTile) //switch statement that takes game state currentDifficulty
                {
                    case GameState.Difficulty.hard: //checks if highest difficulty has been reached so that it doesn't do a lot more unnecessary checks
                        checkTheHardDifficulty(); //method/function for when the highest difficulty is reached so that no additional unnecessary chesks
                                                  //are being performed
                        break;
                    default:
                        checkTheDifficulty(); //method/function used to determine which tiles should spawn with a bunch of checks until reaching highest difficulty
                        break;
                }

                break;

            case true:
                targetZ = tileMovement.TargetZright.position; // moveTowards and lerp will drag the tiles in the on the left and right in a straight line to a new targetZ
                tilePrefabs = tileMovement.EnvironmentTiles; // and the tilePrefab array that tiles spawn from, is going to be the array for the envirnment tiles
                break;
        }

    }


    private void checkTheDifficulty() //function to be called in tilesAndDifficulty function to make it look cleaner
    {                                 //function used to determine which difficulty tiles it should pull from based on chance and difficulty
                                      //only used until we get to highest difficulty   
        // [Oakley] Made some edits to Arian's code to be more flexible and open for the designers to change
        if (gameState.currentDifficultyTile == GameState.Difficulty.veryEasy) //Check the current game difficulty
        {
            veasyTotal = veasyEasyWeight + veasyHardWeight + veasyMediumWeight; //Calculate what 100% is from the weightings
            percentageChance = Random.Range(1, veasyTotal + 1); //Get a random number from 0 to the highest value
            if (percentageChance <= veasyEasyWeight) //Check if the number is within the range set by the easy weighting
            {
                tilePrefabs = tileMovement.EasyTilePrefabs; //If true then spawn an easy tile
            }
            else if (percentageChance > veasyEasyWeight && percentageChance <= (veasyEasyWeight + veasyMediumWeight)) //If it was false , check if the number is within the medium range
            {
                tilePrefabs = tileMovement.MediumTilePrefabs; //If true spawn a medium tile
            }
            else if (percentageChance > (veasyEasyWeight + veasyMediumWeight)) //If both were false, check if the number was in the hard range (redundant?)
            {
                tilePrefabs = tileMovement.HardTilePrefabs; //If true spawn a hard tile
            }

        }

        if (gameState.currentDifficultyTile == GameState.Difficulty.easy)
        {
            easyTotal = easyEasyWeight + easyHardWeight + easyMediumWeight;
            percentageChance = Random.Range(1, easyTotal + 1);
            if (percentageChance <= easyEasyWeight)
            {
                tilePrefabs = tileMovement.EasyTilePrefabs;
            }
            else if (percentageChance > easyEasyWeight && percentageChance <= (easyEasyWeight + easyMediumWeight))
            {
                tilePrefabs = tileMovement.MediumTilePrefabs;
            }
            else if (percentageChance > (easyEasyWeight + easyMediumWeight))
            {
                tilePrefabs = tileMovement.HardTilePrefabs;
            }

        }

        if (gameState.currentDifficultyTile == GameState.Difficulty.easyMedium)
        {
            easymedTotal = easymedEasyWeight + easymedHardWeight + easymedMediumWeight;
            percentageChance = Random.Range(1, easymedTotal + 1);
            if (percentageChance <= easymedEasyWeight)
            {
                tilePrefabs = tileMovement.EasyTilePrefabs;
            }
            else if (percentageChance > easymedEasyWeight && percentageChance <= (easymedEasyWeight + easymedMediumWeight))
            {
                tilePrefabs = tileMovement.MediumTilePrefabs;
            }
            else if (percentageChance > (easymedEasyWeight + easymedMediumWeight))
            {
                tilePrefabs = tileMovement.HardTilePrefabs;
            }

        }

        if (gameState.currentDifficultyTile == GameState.Difficulty.mediumEasy)
        {
            medeasyTotal = medeasyEasyWeight + medeasyHardWeight + medeasyMediumWeight;
            percentageChance = Random.Range(1, medeasyTotal + 1);
            if (percentageChance <= medeasyEasyWeight)
            {
                tilePrefabs = tileMovement.EasyTilePrefabs;
            }
            else if (percentageChance > medeasyEasyWeight && percentageChance <= (medeasyEasyWeight + medeasyMediumWeight))
            {
                tilePrefabs = tileMovement.MediumTilePrefabs;
            }
            else if (percentageChance > (medeasyEasyWeight + medeasyMediumWeight))
            {
                tilePrefabs = tileMovement.HardTilePrefabs;
            }

        }

        if (gameState.currentDifficultyTile == GameState.Difficulty.mediumHard)
        {
            medhardTotal = medhardEasyWeight + medhardHardWeight + medhardMediumWeight;
            percentageChance = Random.Range(1, medhardTotal + 1);
            if (percentageChance <= medhardEasyWeight)
            {
                tilePrefabs = tileMovement.EasyTilePrefabs;
            }
            else if (percentageChance > medhardEasyWeight && percentageChance <= (medhardEasyWeight + medhardMediumWeight))
            {
                tilePrefabs = tileMovement.MediumTilePrefabs;
            }
            else if (percentageChance > (medhardEasyWeight + medhardMediumWeight))
            {
                tilePrefabs = tileMovement.HardTilePrefabs;
            }

        }

        if (gameState.currentDifficultyTile == GameState.Difficulty.hardMedium)
        {
            hardmedTotal = hardmedEasyWeight + hardmedHardWeight + hardmedMediumWeight;
            percentageChance = Random.Range(1, hardmedTotal + 1);
            if (percentageChance <= hardmedEasyWeight)
            {
                tilePrefabs = tileMovement.EasyTilePrefabs;
            }
            else if (percentageChance > hardmedEasyWeight && percentageChance <= (hardmedEasyWeight + hardmedMediumWeight))
            {
                tilePrefabs = tileMovement.MediumTilePrefabs;
            }
            else if (percentageChance > (hardmedEasyWeight + hardmedMediumWeight))
            {
                tilePrefabs = tileMovement.HardTilePrefabs;
            }

        }
    }


    private void checkTheHardDifficulty() //separate function for the highest difficulty to determine which tiles to pull from
                                          //method created to make sure the other checks aren't done when highest difficulty is reached
    {
        if (gameState.currentDifficultyTile == GameState.Difficulty.hard)
        {
            hardTotal = hardEasyWeight + hardHardWeight + hardMediumWeight;
            percentageChance = Random.Range(1, hardTotal + 1);
            if (percentageChance <= hardEasyWeight)
            {
                tilePrefabs = tileMovement.EasyTilePrefabs;
            }
            else if (percentageChance > hardEasyWeight && percentageChance <= (hardEasyWeight + hardMediumWeight))
            {
                tilePrefabs = tileMovement.MediumTilePrefabs;
            }
            else if (percentageChance > (hardEasyWeight + hardMediumWeight))
            {
                tilePrefabs = tileMovement.HardTilePrefabs;
            }

        }

    }


    /// Code done by Arian- end



}
