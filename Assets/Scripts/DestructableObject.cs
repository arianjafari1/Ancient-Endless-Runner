using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// Script created 22/07/22 by Malachi
    /// 
    /// </summary>
    /// 


    [SerializeField] GameObject IntactObstacle;
    [SerializeField] GameObject destroyedObstacle;
    [SerializeField] ParticleSystem dustParticles;


    public void DestroyObstacle()
    {
        dustParticles.Play();
        IntactObstacle.GetComponent<MeshRenderer>().enabled = false;
    }


}
