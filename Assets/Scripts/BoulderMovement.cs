using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderMovement : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// Script created 28/06/22 by Malachi
    /// 
    /// 28/06/22 - added boulder speed variables
    ///          - added boulder minimum position (for how far back the boulder gets from the player)
    ///          - gave boulder an empty parent object to move the transform of, as first attempt at moving
    ///            had issues due to transform.Translate using local space rather than world space.
    ///          - added movement code which moves backwards by default, and forwards while the player staggers
    ///          - added getter/setter for the player staggering variable so it can be accessed from the player movement script
    /// 29/06/22 - added movement and tilemovement classes into script 
    ///          - boulder now has a max position where it gets destroyed upon reaching
    ///          - boulder now has collision with the player, which hides the player object and then
    ///            removes speed and acceleration from the tiles, and continues to roll onwards
    /// 05/07/22 - fixed redundant statement in start - was setting variable to start position instead of actually setting the physical start position.
    /// 13/07/22 - replaced player staggering variable with the newly added player state enum from the player movement script.
    /// 
    ///          - Added flatten animation when the boulder rolls over the player
    ///          - refactored the increased movement speed of the boulder after player death into the switch statement instead of
    ///            flat out increasing it, as the boulder could sometimes trigger the oncollisionenter multiple times which would
    ///            exponentially increase the speed and cause the boulder to shoot off like a rocket.
    ///          - created SetDeathState function to set the required variables needed for when the player dies to control boulder/tile
    ///            movement, without needing to use the exact same code in the other obstacle death type.
    ///          - added Game over screen to death function
    /// 15/07/22 - moved setDeathState to the player instead of the boulder
    /// 22/07/22 - moved showing gameover screen to when boulder goes off screen so player gets to see the death animation properly
    /// 
    /// 
    /// </summary>

    [Tooltip("This is for the parent object, not the actual boulder")]
    [SerializeField] private GameObject boulder;
    [SerializeField] private GameOver gameOverScreen;
    [SerializeField] private Movement playerMovement;
    [SerializeField] private TileMovement tileMovement;
    [Tooltip("The position at which the boulder starts - wants to be not too far back, but not so close it kills you after the first trip")]
    [SerializeField] private float startZPos;
    [Tooltip("The furthest back the boulder can go - make sure it is still visible on the camera as it needs to be intimidating")]
    [SerializeField] private float minZPos;
    [Tooltip("The furthest forward the boulder can go before despawning (post-player death)")]
    [SerializeField] private float maxZPos;
    [Tooltip("Speed at which the boulder retreats as the player accelerates")]
    [SerializeField] private float backwardsMovement;
    [Tooltip("Speed at which the boulder moves forward as the player is staggering")]
    [SerializeField] private float forwardsMovement;
    [SerializeField] private float wiggleAmount; //Movement amount side to side
    [SerializeField] private float wiggleSpeed; //Speed for side to side movement
    private Vector3 originPos;
    private Vector3 targetPos;

    
    //private bool isPlayerStaggering;
    //public bool switchPlayerStaggering
    //{
    //    get
    //    {
    //        return isPlayerStaggering;
    //    }
    //    set
    //    {
    //        if (value == true)
    //        {
    //            isPlayerStaggering = true;
    //        }
    //        else
    //        {
    //            isPlayerStaggering = false;
    //        }
    //    }
    //}


    void Start()
    {
        boulder.transform.position = new Vector3(0, 5, startZPos);
    }
    void FixedUpdate()
    {
        switch(playerMovement.getPlayerState)
        {
            case Movement.PlayerStates.staggered:
                boulder.transform.Translate(Vector3.forward * forwardsMovement);
                break;
            case Movement.PlayerStates.dead:
                boulder.transform.Translate(5 * forwardsMovement * Vector3.forward);
                break;
            case Movement.PlayerStates.running:
                if (boulder.transform.position.z >= minZPos)
                {
                    boulder.transform.Translate(Vector3.back * backwardsMovement);
                }
                break;
        }
        if (boulder.transform.position.z >= maxZPos)
        {

            gameOverScreen.ShowGameOverScreen();
            Destroy(boulder);
        }
        originPos = new Vector3(-wiggleAmount, boulder.transform.position.y, boulder.transform.position.z);
        targetPos = new Vector3(wiggleAmount, boulder.transform.position.y, boulder.transform.position.z);
        boulder.transform.position = Vector3.Lerp(originPos, targetPos, (Mathf.Sin(Time.time * wiggleSpeed) +1.0f) / 2.0f);

    }

    
    private void OnCollisionEnter(Collision collision)
    {
        //when the boulder touches the player, the player is flattened and the ball continues to roll.
        //also stops the ground movement, removes the speed increase, and increases the boulder movement speed;
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has been flattened");
            
            //tileMovement.movementSpeedGetterSetter = 0;
            //tileMovement.speedIncreaseEverySecondGetterSetter = 0;
            ////forwardsMovement *= 5;
            //playerMovement.CancelSpeedReturn();
            //playerMovement.getPlayerState = Movement.PlayerStates.dead;
            playerMovement.PlayAnimation("Flatten");
            playerMovement.SetDeathState();
        }

        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("obstacleToStagger") || collision.gameObject.CompareTag("obstacleToJump") || collision.gameObject.CompareTag("obstacleToSlide"))
        {
            collision.gameObject.GetComponent<DestructableObject>().DestroyObstacle();
        }
    }

    //public void SetDeathState()
    //{
    //    tileMovement.movementSpeedGetterSetter = 0;
    //    tileMovement.speedIncreaseEverySecondGetterSetter = 0;
    //    playerMovement.CancelSpeedReturn();
    //    playerMovement.getPlayerState = Movement.PlayerStates.dead;
    //    playerMovement.DisablePlayerInput();
    //    gameOverScreen.ShowGameOverScreen();
    //}

}
