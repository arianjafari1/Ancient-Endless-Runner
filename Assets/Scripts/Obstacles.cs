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
    /// 
    /// </summary>
    /// 
    private Movement playerMovement;

    private void Start()
    {
        playerMovement = GetComponentInParent<Movement>();
    }

    /// Code done by Arian- start
    private void OnTriggerEnter(Collider collisionInfo) //check for collision infor
    {


        if (collisionInfo.gameObject.CompareTag("obstacleToJump"))
        {


            Debug.Log("You should have jumped, you are dead");
            playerMovement.Stagger();


        }

        if (collisionInfo.gameObject.CompareTag("obstacleToStagger"))
        {


            Debug.Log("You should have jumped, you are staggered");
            playerMovement.Stagger();


        }

        if (collisionInfo.gameObject.CompareTag("obstacleToSlide"))
        {


            Debug.Log("You should have slid, you are dead");

            playerMovement.Stagger();

        }

        if (collisionInfo.gameObject.CompareTag("pitObstacle"))
        {


            Debug.Log("Fallen into pit");



        }

        /// Code done by Arian- end

    }


}
