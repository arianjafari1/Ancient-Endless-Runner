using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// Script created 22/07/22 by Malachi
    /// 01/08/2022 -[Arian] changed IntactObstacle.GetComponent<MeshRenderer>().enabled = false; to IntactObstacle.gameObject.SetActive(false);, as I can't
    ///            find a way to send it back to active from another script, Malachi might need to look at the Destruction particle system
    /// 02/08/2022 -[Arian] changed it back to the way Malachi has done it
    /// </summary>
    /// 


    [SerializeField] GameObject IntactObstacle;
    [SerializeField] GameObject destroyedObstacle;
    [SerializeField] ParticleSystem dustParticles;


    public void DestroyObstacle()
    {
        dustParticles.Play();
        IntactObstacle.GetComponent<MeshRenderer>().enabled = false;
        //IntactObstacle.gameObject.SetActive(false);
    }


}
