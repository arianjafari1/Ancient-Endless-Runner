using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// Code done by Arian- start
    private void OnTriggerEnter(Collider collisionInfo) //check for collision infor
    {


        if (collisionInfo.gameObject.CompareTag("obstacleToJump"))
        {


            Debug.Log("You should have jumped, you are dead");



        }

        if (collisionInfo.gameObject.CompareTag("obstacleToStagger"))
        {


            Debug.Log("You should have jumped, you are staggered");



        }

        if (collisionInfo.gameObject.CompareTag("obstacleToSlide"))
        {


            Debug.Log("You should have slid, you are dead");



        }


        /// Code done by Arian- end

    }


}
