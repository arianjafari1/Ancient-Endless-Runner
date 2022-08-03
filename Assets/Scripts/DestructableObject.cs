using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// Script created 22/07/22 by Malachi
    /// 22/07/22 - play the particle system and turn off the object. will turn on broken version of object if we ever get those
    ///            created
    /// 01/08/22 - [Arian] changed IntactObstacle.GetComponent<MeshRenderer>().enabled = false; to IntactObstacle.gameObject.SetActive(false);, as I can't
    ///            find a way to send it back to active from another script, Malachi might need to look at the Destruction particle system
    /// 02/08/22 - [Arian] changed it back to the way Malachi has done it
    /// </summary>
    /// 


    [SerializeField] GameObject[] IntactObstacles;
    [SerializeField] GameObject[] destroyedObstacle;
    [SerializeField] ParticleSystem dustParticles;


    public void DestroyObstacle()
    {
        dustParticles.Play();
        foreach(GameObject obstacle in IntactObstacles)
        {
            obstacle.SetActive(false);
        }
        //IntactObstacle.gameObject.SetActive(false);
    }


}
