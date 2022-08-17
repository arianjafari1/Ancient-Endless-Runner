using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private BoulderMovement boulderMovement;
    [SerializeField] private BoulderRotate boulderRotate;
    private TileMovement tileMovement;
    private Movement playerMovement;
    private GameState currentGameState;
    [SerializeField] private ParticleSystem[] boulderParticles;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private AudioManager audioManager;


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

        startingTileSpeed = tileMovement.movementSpeedGetterSetter;
        tileAccel = tileMovement.speedIncreaseEverySecondGetterSetter;
        boulderSpeed = boulderMovement.getBackwardsMovement;
        boulderRotation = boulderRotate.RotationAmount;

        tileMovement.movementSpeedGetterSetter = -1.5f;
        tileMovement.speedIncreaseEverySecondGetterSetter = 0;
        boulderMovement.getBackwardsMovement = 0;
        boulderRotate.RotationAmount = -0.3f;
        currentGameState.CurrentGameState = GameState.gameState.beginningCutScene;
        playerMovement.PlayAnimation("CutsceneMovement");
        playerMovement.DisablePlayerInput();
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;




        Invoke(nameof(BeginGame), 4f);
        
    }

    public void BeginGame()
    {
        tileMovement.movementSpeedGetterSetter = startingTileSpeed;
        tileMovement.speedIncreaseEverySecondGetterSetter = tileAccel;
        boulderMovement.getBackwardsMovement = boulderSpeed;
        boulderRotate.RotationAmount = boulderRotation;
        currentGameState.CurrentGameState = GameState.gameState.isPlaying;
        if (PlayerPrefs.GetInt("TutorialComplete", 0) != 0) 
        { 
            playerMovement.EnablePlayerInput();
        }
        audioManager.PlaySound("Boulder_Movement");

        foreach (ParticleSystem particleSystem in boulderParticles)
        {
            particleSystem.Play();
        }
    }
}
