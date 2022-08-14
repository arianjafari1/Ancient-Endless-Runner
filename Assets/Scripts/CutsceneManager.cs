using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private BoulderMovement boulderMovement;
    [SerializeField] private BoulderRotate boulderRotate;
    private TileMovement tileMovement;
    private Movement playerMovement;


    private float startingTileSpeed;
    private float tileAccel;
    private float boulderSpeed;
    private float boulderRotation;

    private void Start()
    {
        tileMovement = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TileMovement>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();

        startingTileSpeed = tileMovement.movementSpeedGetterSetter;
        tileAccel = tileMovement.speedIncreaseEverySecondGetterSetter;
        boulderSpeed = boulderMovement.getBackwardsMovement;
        boulderRotation = boulderRotate.RotationAmount;

        tileMovement.movementSpeedGetterSetter = -1;
        tileMovement.speedIncreaseEverySecondGetterSetter = 0;
        boulderMovement.getBackwardsMovement = 0;
        boulderRotate.RotationAmount = -0.1f;
        Invoke(nameof(BeginGame), 2f);
    }

    public void BeginGame()
    {
        tileMovement.movementSpeedGetterSetter = startingTileSpeed;
        tileMovement.speedIncreaseEverySecondGetterSetter = tileAccel;
        boulderMovement.getBackwardsMovement = boulderSpeed;
        boulderRotate.RotationAmount = boulderRotation;
    }
}
