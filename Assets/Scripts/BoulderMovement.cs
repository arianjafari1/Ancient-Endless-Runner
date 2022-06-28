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
    ///          - had issues due to transform.Translate using local space rather than world space.
    ///          - added movement code which moves backwards by default, and forwards while the player staggers
    ///          - added getter/setter for the player staggering variable so it can be accessed from the player movement script
    /// 
    /// </summary>

    [Tooltip("This is for the parent object, not the actual boulder")]
    [SerializeField]private GameObject boulder;
    [Tooltip("The position at which the boulder starts - wants to be not too far back, but not so close it kills you after the first trip")]
    [SerializeField] private float startZPos;
    [Tooltip("The furthest back the boulder can go - make sure it is still visible on the camera as it needs to be intimidating")]
    [SerializeField] private float minZPos;
    [Tooltip("Speed at which the boulder retreats as the player accelerates")]
    [SerializeField] private float backwardsMovement;
    [Tooltip("Speed at which the boulder moves forward as the player is staggering")]
    [SerializeField] private float forwardsMovement;
    private bool isPlayerStaggering;
    public bool switchPlayerStaggering
    {
        get
        {
            return isPlayerStaggering;
        }
        set
        {
            if (value == true)
            {
                isPlayerStaggering = true;
            }
            else
            {
                isPlayerStaggering = false;
            }
        }
    }


    void Start()
    {
        Vector3 startPos = new Vector3(0, 5, startZPos);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(isPlayerStaggering)
        {
            case true:
                boulder.transform.Translate(Vector3.forward * forwardsMovement);
                break;
            case false:
                if (boulder.transform.position.z >= minZPos)
                {
                    boulder.transform.Translate(Vector3.back * backwardsMovement);
                }
                break;
        }
    }

    
}
