using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private float timer; // this is the float timer
    private int globalTime; //this is the actual global time which I will have a getter and setter for


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

    private Difficulty difficulty; //declaring enum type as private

    ///start- times at which difficulties change with long descriptive names for the designer to be able to change in the editor
    [SerializeField] private int easyDifficultyTime; //time at which difficulty changes to 80% easy Tiles, 20% medium Tiles
    [SerializeField] private int easyMediumDifficultyTime; //time at which difficulty changes to 60% easy Tiles, 40% Medium Tiles
    [SerializeField] private int mediumEasyDifficultyTime; //time at which difficulty changes to 60% medium Tiles, 40% Easy Tile
    [SerializeField] private int mediumHardDifficultyTime; //time at which difficulty changes to 40% medium Tiles, 20% Hard Tiles, 40% easy Tiles
    [SerializeField] private int hardMediumDifficultyTime; //time at which difficulty changes to 40% medium Tiles, 25% hard Tiles, 35% easy Tiles
    [SerializeField] private int hardDifficultyTime; //time at which difficulty changes to 45% medium Tiles, 30% hard Tiles, 25% easy Tiles
    ///end


    // Start is called before the first frame update
    private void Awake()
    {
        difficulty = Difficulty.veryEasy;
        //Debug.Log(currentDifficultyTile);
    }

    
    private void FixedUpdate()
    {
        timer += Time.deltaTime; //using delta time to count time in float
        globalTime = (int)(timer); //turning the timer to integer
        //Debug.Log(globalTime);
        changedDifficulty(); //calling function to change difficulty
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
