using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutsceneManager : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// Script created 14/08/22 by Malachi
    /// 
    /// 14/08/22 - Created cutscene manager. Saves variables regarding movement of player/boulder, gives
    ///            them a new value to make them go the other way and then after the cutscene finishes, 
    ///            resets these values to what they are set to in the editor and begins the game
    /// 15/08/22 - added gamestate to make use of the cutscenePlaying enum value
    ///          - added boulder particles, vcam and audiomanager to make the camera still while the
    ///            cutscene plays and not play the boulder dust trail, stops the boulder moving left/right,
    ///            and doesnt play the boulder sound.
    ///          - disabled player input during cutscene.
    /// 17/08/22 - player input only now gets re-enabled when tutorial has already been complete.
    /// 18/08/22 - serialized cutscene variables.
    /// </summary>
    [SerializeField] private BoulderMovement boulderMovement;
    [SerializeField] private BoulderRotate boulderRotate;
    private TileMovement tileMovement;
    private Movement playerMovement;
    private GameState currentGameState;
    [SerializeField] private ParticleSystem[] boulderParticles;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private AudioManager audioManager;

    [Tooltip("Must be negative")]
    [SerializeField] private float cutsceneTileSpeed;
    [Tooltip("Must be negative")]
    [SerializeField] private float cutsceneBoulderRotation;
    [Tooltip("Refer to the 'cutscene movement' animation")]
    [SerializeField] private float cutsceneLength;
 
    private float startingTileSpeed;
    private float tileAccel;
    private float boulderSpeed;
    private float boulderRotation;

    private void Start()
    {
        tileMovement = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TileMovement>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        currentGameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameState>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        
        //Gets the values from the editor and stores them in temporary variables
        startingTileSpeed = tileMovement.movementSpeedGetterSetter;
        tileAccel = tileMovement.speedIncreaseEverySecondGetterSetter;
        boulderSpeed = boulderMovement.getBackwardsMovement;
        boulderRotation = boulderRotate.RotationAmount;

        //Sets the speeds to the new cutscene speed, and makes sure the correct gamestate is enabled
        tileMovement.movementSpeedGetterSetter = cutsceneTileSpeed;
        tileMovement.speedIncreaseEverySecondGetterSetter = 0;
        boulderMovement.getBackwardsMovement = 0;
        boulderRotate.RotationAmount = cutsceneBoulderRotation;
        currentGameState.CurrentGameState = GameState.gameState.beginningCutScene;
        //triggers the cutscene animation (player starts far back, rotates 180 degrees and moves to start position)
        playerMovement.PlayAnimation("CutsceneMovement");
        playerMovement.DisablePlayerInput();
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        //Triggers the end of the cutscene after the finished length
        Invoke(nameof(BeginGame), cutsceneLength);
        
    }

    public void BeginGame()
    {
        //Sets the variables back to what they are in the editor, and sets the gamestate to playing
        tileMovement.movementSpeedGetterSetter = startingTileSpeed;
        tileMovement.speedIncreaseEverySecondGetterSetter = tileAccel;
        boulderMovement.getBackwardsMovement = boulderSpeed;
        boulderRotate.RotationAmount = boulderRotation;
        currentGameState.CurrentGameState = GameState.gameState.isPlaying;

        //checks if the tutorial is complete before reenabling input
        if (PlayerPrefs.GetInt("TutorialComplete", 0) != 0) 
        { 
            playerMovement.EnablePlayerInput();
        }
        audioManager.PlaySound("Boulder_Movement");

        //starts the smoke and mini-rock particles
        foreach (ParticleSystem particleSystem in boulderParticles)
        {
            particleSystem.Play();
        }
    }


}
