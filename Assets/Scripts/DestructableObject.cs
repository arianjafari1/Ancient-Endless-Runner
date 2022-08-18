using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// Script created 22/07/22 by Malachi
    /// 22/07/22 - play the particle system and turn off the object. will turn on broken version of object if we ever get those created
    /// 01/08/22 - [Arian] changed IntactObstacle.GetComponent<MeshRenderer>().enabled = false; to IntactObstacle.gameObject.SetActive(false);, as I can't
    ///            find a way to send it back to active from another script, Malachi might need to look at the Destruction particle system
    /// 02/08/22 - [Arian] changed it back to the way Malachi has done it
    /// 16/08/22 - turned off the collider so the boulder doesnt trigger this function again if the player broke it with shield
    /// </summary>
    /// 


    [SerializeField] GameObject[] IntactObstacles;
    [SerializeField] GameObject[] destroyedObstacle; //obsolete as we never requested rubble/shards
    [SerializeField] ParticleSystem dustParticles;


    //turns each object that is part of the obstacle array off and plays the particle effect.
    public void DestroyObstacle()
    {
        dustParticles.Play();
        foreach(GameObject obstacle in IntactObstacles)
        {
            obstacle.SetActive(false);
            
        }
        //also turns the collider of the obstacle off so it doesnt trigger for both shielded player and boukder collision
        gameObject.GetComponent<BoxCollider>().enabled = false;
        //IntactObstacle.gameObject.SetActive(false);
    }


}
