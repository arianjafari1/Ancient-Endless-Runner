using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Arian unless otherwise specified
    /// Script created 21/06/22 by Arian
    /// 
    /// 21/06/22 - added collision detection with different functions for
    /// 13/07/22 - [MALACHI] linked player stagger to collision
    ///          - [MALACHI] added pit tag
    /// 15/07/22 - [MALACHI] added death trigger on pit tagged obstacles, and linked pit fall animation
    /// 27/07/22 - added audio manager
    ///          - [MALACHI] added checks for shield powerup
    /// 
    /// </summary>
    /// 
    private Movement playerMovement;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); //refer to the script in the empty object
        playerMovement = GetComponentInParent<Movement>();

    }

    private void OnTriggerEnter(Collider collisionInfo) //check for collision infor
    {


        if (collisionInfo.gameObject.CompareTag("obstacleToJump"))
        {
            if (!playerMovement.IsShieldActive)
            {
                playerMovement.Stagger();
                audioManager.PlaySound("HitEnemySound");
                return;
            }

            Debug.Log("Shield active");
            collisionInfo.GetComponent<DestructableObject>().DestroyObstacle();
            

        }

        //these obstacles break even if the player staggers on them as they are fragile
        if (collisionInfo.gameObject.CompareTag("obstacleToStagger"))
        {
            collisionInfo.GetComponent<DestructableObject>().DestroyObstacle();
            audioManager.PlaySound("HitEnemySound");

            if (playerMovement.IsShieldActive)
            {
                Debug.Log("Shield active");
                return;
            }

            playerMovement.Stagger();


        }

        if (collisionInfo.gameObject.CompareTag("obstacleToSlide"))
        {
            if (!playerMovement.IsShieldActive)
            {
                playerMovement.Stagger();
                audioManager.PlaySound("HitEnemySound");
                return;
            }
            Debug.Log("Shield active");
            collisionInfo.GetComponent<DestructableObject>().DestroyObstacle();


        }

        if (collisionInfo.gameObject.CompareTag("pitObstacle"))
        {
            audioManager.PlaySound("Scream");
            playerMovement.SetDeathState();
            switch (playerMovement.getLane)
            {
                case Movement.Lanes.left:
                    playerMovement.PlayAnimation("FallInLeftPit");
                    break;
                case Movement.Lanes.center:
                    playerMovement.PlayAnimation("FallInCenterPit");
                    break;
                case Movement.Lanes.right:
                    playerMovement.PlayAnimation("FallInRightPit");
                    break;
            }
            


        }

        /// Code done by Arian- end

    }


}
