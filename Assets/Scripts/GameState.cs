using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private float timer; // this is the float timer
    private int globalTime; //this is the actual global time which I will have a getter and setter for
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        timer += Time.deltaTime; //using delta time to count time in float
        globalTime = (int)(timer % 60); //turning the timer to integer
        Debug.Log(globalTime);
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



}
