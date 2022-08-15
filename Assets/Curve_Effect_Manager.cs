using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Curve_Effect_Manager : MonoBehaviour
{
    /// <summary>
    /// This script was all made by Oakley, I (Malachi) just added a couple of lines to prevent
    /// the curve continuing to move despite the player being dead
    /// </summary>

    [SerializeField] private float curveStart; //How far away from the camera the curve should begin
    [SerializeField] private float curveShallowness; //How extreme the curve should be
    [SerializeField] private float curveMaxHeight; //The maximum elevation of the height of the curve
    [SerializeField] private float curveLeftMax; //The maximum curvature to the left
    [SerializeField] private float curveRightMax; //The maximum curvature to the right
    [SerializeField] private int curveAmount;
    private float curveCurrentValue; //The current curvature left or right
    [SerializeField] private float curvePauseMinTime; //The minimum time that the curve should pause before moving
    [SerializeField] private float curvePauseMaxTime; //The maximum time that the curve should pause before moving
    private float currentPauseTime; //The current time that the curve has been paused
    private float targetPauseTime; //The target time for the pause
    private bool isPaused; // Check for if the curve is currently paused
    private bool isCurvedLeft; // Check for direction of curve
    [SerializeField] private float transSpeed; //How fast the curve should transition
    [SerializeField] private bool useWarp;

    private GameState currentGameState;
    public GameObject[] lights; //Create an array to store lights in the scene
    [SerializeField] private Camera cam; //Get camera
    private Transform camTrans;
    private Vector3 camPos;
    private float camZPos;
    private double distanceFromCam;
    private float LerpAmount;
    private Vector3 lightOrigin;
    private Vector3 lightDestination;
    [SerializeField] private GameObject theSun;
    private Transform theSunTrans;
    private float sunCurrentRot;
    private float sunTargetRot;

    [SerializeField] private float sunYRotateAmplitude;
    [SerializeField] private float sunYDefaultRotation;

    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalFloat("_curveStart", curveStart);
        Shader.SetGlobalFloat("_curveShallowness", curveShallowness);
        Shader.SetGlobalFloat("_curveMaxHeight", curveMaxHeight);
        Shader.SetGlobalInt("_curveAmount", curveAmount);
        
        Shader.SetGlobalFloat("_curveMaxWidth", curveLeftMax); //Starts curve to the left
        curveCurrentValue = curveLeftMax; // Set current curve to Left maximum
        isCurvedLeft = true; //Set curved left to true
        currentPauseTime = 0.0f; //Set current pause time to zero
        targetPauseTime = UnityEngine.Random.Range(curvePauseMinTime, curvePauseMaxTime); //Set target pause time to random value between Min and Max pause time
        currentGameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameState>();

        float useWarpVal = useWarp ? 1.0f : 0.0f;
        Shader.SetGlobalFloat("_useWarp", useWarpVal);

        camTrans = cam.GetComponent<Transform>();
        camPos = camTrans.position;
        camZPos = camPos.z;
    }

    ///<summary>
    /// Change the sun's rotation to make the shadows look consistent
    ///</summary>
    private void UpdateSunRotation(float currentCurveValue)
    {
        float sunYAngle = this.sunYDefaultRotation;

        float curveProportion = -currentCurveValue / Mathf.Abs(this.curveLeftMax);

        sunYAngle += this.sunYRotateAmplitude * curveProportion;

        this.theSun.transform.eulerAngles = new Vector3(this.theSun.transform.eulerAngles.x, sunYAngle, this.theSun.transform.eulerAngles.x);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // While current pause time is less than the target, isPaused is true. And we update the current time until it reaches max
        if (Math.Abs(targetPauseTime - currentPauseTime) <= 0.01)
        {
            currentPauseTime = Mathf.Lerp(currentPauseTime, targetPauseTime, Time.deltaTime);
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }
        // If isPaused is false then we curve;
        if (!isPaused && !(currentGameState.CurrentGameState == GameState.gameState.gameOver || currentGameState.CurrentGameState == GameState.gameState.beginningCutScene))
        {
            //If we are currently curved left and the curve is not yet at the right side we keep curving
            if (isCurvedLeft && Math.Abs(curveCurrentValue - curveRightMax) >= 0.01)
            {
                curveCurrentValue = Mathf.Lerp(curveCurrentValue, curveRightMax, (Mathf.Sin((transSpeed / 1000) * Time.deltaTime) +0.01f) / 2.0f); //Move the curve right over time
                this.UpdateSunRotation(curveCurrentValue);
                Shader.SetGlobalFloat("_curveMaxWidth", curveCurrentValue); //Update the shader as we go
            }
            else
            { //If we are no longer curved left, or we are at the right side then we pause
                isCurvedLeft = false; 
                currentPauseTime = 0.0f; //Set current pause time to zero
                targetPauseTime = UnityEngine.Random.Range(curvePauseMinTime, curvePauseMaxTime); //Set target pause time to random value between Min and Max pause time
            }
            if (!isCurvedLeft && Math.Abs(curveCurrentValue - curveLeftMax) >= 0.01) //If we are current curved right and the curve is not yet at the left side we curve
            {
                curveCurrentValue = Mathf.Lerp(curveCurrentValue, curveLeftMax, (Mathf.Sin((transSpeed /1000) * Time.deltaTime) +0.01f) / 2.0f); //Move left over time
                this.UpdateSunRotation(curveCurrentValue);               
                Shader.SetGlobalFloat("_curveMaxWidth", curveCurrentValue); //Update the shader
            }
            else
            { //If we are no longer curved right, or we reach the left side then we pause again
                isCurvedLeft = true;
                currentPauseTime = 0.0f; //Set current pause time to zero
                targetPauseTime = UnityEngine.Random.Range(curvePauseMinTime, curvePauseMaxTime); //Set target pause time to random value between Min and Max pause time
            }
        }
        //Script to move the lights
        /*lights = GameObject.FindGameObjectsWithTag("Light");

        //Check use warp
        if (useWarp)
        {
            foreach (GameObject light in lights)
            {
                distanceFromCam = ((light.transform.position.z - camZPos) + curveStart) * curveShallowness;
                LerpAmount = Convert.ToSingle(Math.Pow(distanceFromCam, curveAmount));
                lightOrigin = light.transform.position;
                lightDestination = new Vector3(lightOrigin.x + curveCurrentValue, lightOrigin.y + curveMaxHeight, lightOrigin.z);
                light.transform.position = Vector3.Lerp(lightOrigin, lightDestination, LerpAmount);
            }
        }/*/
    }
}
