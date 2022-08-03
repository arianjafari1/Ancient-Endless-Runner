using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    /// Code done by Arian- Start

    /// <summary>
    /// 12/07/2022 -[Arian] created GameState Script
    ///            -[Arian] added a timer float variable which I assiged to timer + Time.deltaTime in the fixed update
    ///            -[Arian]added a globalTime integer variable and assigned it to (int) (timer % 60) in the fixedUpdate to get an int for the time
    /// 14/07/2022 -[Arian] Moved the  Difficulty enum to GameState
    ///            -[Arian] set the default difficulty to veryEasy in Awake
    ///            -[Arian] added a function that is called in fixed update to change the difficulty progressively higher and higher at certain time intervals
    ///            -[Arian] added variables for the time at which difficulty changes, and made them visible in the editor
    /// 19/07/2022 -[Arian] changed the timer to += Time.fixedDeltaTime to make it framerate independent
    ///            -[Arian] changed globalTime to (int)(timer)
    /// 20/07/2022 -[Arian] added an overall gameState, with things such as beginningCutScene, isPlaying, gameOver, isPaused
    /// </summary>

    private float timer; // this is the float timer
    private int globalTime; //this is the actual global time which I will have a getter and setter for
    private TileMovement tileMovement; //tile movement script referance
    private AudioManager audioManager;

    public enum Difficulty //creating an enum for the difficulty of the game
    {
        veryEasy, //100% easy Tiles
        easy, //80% easy Tiles, 20% medium Tiles
        easyMedium, // 60% easy Tiles, 40% Medium Tiles
        mediumEasy, // 60% medium Tiles, 40% Easy Tiles
        mediumHard, // 40% medium Tiles, 20% Hard Tiles, 40% easy Tiles
        hardMedium, // 40% medium Tiles, 30% hard Tiles, 30% easy Tiles
        hard //50% medium Tiles, 30% hard Tiles, 20% easy Tiles
    }

    public enum gameState //creating an enum for the current state of the game
    {
        beginningCutScene, //is the beginning CutScene playing
        isPlaying, //player is currently playing
        gameOver, //the player is dead
        isPaused //the game is paused
    }

    private gameState currentGameState; //declaring enum type as private for changing the state of the game
    private Difficulty difficulty; //declaring enum type as private for changing difficulty

    ///start- times at which difficulties change with long descriptive names for the designer to be able to change in the editor
    [SerializeField] private int easyDifficultyTime; //time at which difficulty changes to 80% easy Tiles, 20% medium Tiles
    [SerializeField] private int easyMediumDifficultyTime; //time at which difficulty changes to 60% easy Tiles, 40% Medium Tiles
    [SerializeField] private int mediumEasyDifficultyTime; //time at which difficulty changes to 60% medium Tiles, 40% Easy Tile
    [SerializeField] private int mediumHardDifficultyTime; //time at which difficulty changes to 40% medium Tiles, 20% Hard Tiles, 40% easy Tiles
    [SerializeField] private int hardMediumDifficultyTime; //time at which difficulty changes to 40% medium Tiles, 25% hard Tiles, 35% easy Tiles
    [SerializeField] private int hardDifficultyTime; //time at which difficulty changes to 45% medium Tiles, 30% hard Tiles, 25% easy Tiles
    ///end


    private void Awake()
    {
        tileMovement = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TileMovement>(); //assigining tile movement script to tileMovement
        difficulty = Difficulty.veryEasy; //default difficulty when game starts is set to very easy
        currentGameState = gameState.isPlaying; //the game state when the game starts is game is playing
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); //refer to the script in the empty object
        //Debug.Log(currentDifficultyTile);
    }

    
    private void FixedUpdate()
    {
        changeGameState();
        //Debug.Log(currentGameState);
        changedDifficulty(); //calling function to change difficulty
    }

    private void changeGameState() //use this to change the game states and make changes to the
    {
        switch (currentGameState)
        {
            case gameState.gameOver:
                tileMovement.movementSpeedGetterSetter = 0;
                tileMovement.speedIncreaseEverySecondGetterSetter = 0;
                break;

            case gameState.isPlaying:
                timer += Time.fixedDeltaTime; //using delta time to count time in float
                globalTime = (int)(timer); //turning the timer to integer
                break;
        }
    }

    private void changedDifficulty() //function to change to change difficulty based on time
    {

        if (globalTime >= easyDifficultyTime && globalTime < easyMediumDifficultyTime) //check for the global time then if it's equal or higher to
                                                                                       //the time that the game is supposed to up the difficulty
        {
            difficulty = Difficulty.easy; //then take the difficulty up a notch, here I am changing the difficulty using my enum difficulty var type
            
        } else if (globalTime >= easyMediumDifficultyTime && globalTime < mediumEasyDifficultyTime)
        {
            difficulty = Difficulty.easyMedium;
            
        } else if (globalTime >= mediumEasyDifficultyTime && globalTime < mediumHardDifficultyTime)
        {
            difficulty = Difficulty.mediumEasy;
            
        } else if (globalTime >= mediumHardDifficultyTime && globalTime < hardMediumDifficultyTime)
        {
            difficulty = Difficulty.mediumHard;
            
        } else if (globalTime >= hardMediumDifficultyTime && globalTime < hardDifficultyTime)
        {
            difficulty = Difficulty.hardMedium;
            
        } else if (globalTime >= hardDifficultyTime)
        {
            difficulty = Difficulty.hard;
            
        }
 
    }

    public int GlobalTime //getters and setters for globalTime
    {
        get
        {
            return globalTime;
        }
        set
        {
            globalTime = value;
        }
    }

    public float Timer //getters and setters for timer float (exact time)
    {
        get
        {
            return timer;
        }
        set
        {
            timer = value;
        }
    }

    public gameState CurrentGameState //getters and setters for the enum type to be used in various scripts to determine the current state of the game
    {
        get
        {
            return currentGameState;
        }
        set
        {
            currentGameState = value;
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
/// Code done by Arian- end
}
