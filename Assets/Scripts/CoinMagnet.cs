using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour
{

    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// Script created 06/06/22 by Malachi
    /// 
    /// 06/07/22 - Created list which is added to by coins which hit the radius of the coin magnet
    ///          - added iteration to make each coin in range move towards the player
    ///          - added null check in iteration to stop it trying to move destroyed objects, causing an error
    ///          - added enabled check in triggerenter, as the function was being called while the script was disabled
    /// 08/07/22 - reversed order the list is iterated to remove destroyed objects
    ///          - tied coin speed to speed of tiles (coins wouldnt reach player at higher speeds)
    /// 
    /// </summary>
    /// 
    [SerializeField] private Transform playerPosition;
    [SerializeField] private List<GameObject> coinsInRange;
    private TileMovement tileMovement;

    private void Start()
    {
        coinsInRange = new List<GameObject>();
        tileMovement = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TileMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin" && gameObject.GetComponent<CoinMagnet>().enabled == true)
        {
            coinsInRange.Add(other.gameObject);
        }
    }

    
    private void FixedUpdate()
    {
        //https://answers.unity.com/questions/790508/remove-missing-objects-from-list.html
        //swapped foreach to a reverse for loop so i could remove destroyed gameobjects, as hundreds of missing objects
        //would remain in the list.

        //foreach (GameObject coin in coinsInRange)
        //{
        //    if (coin != null)
        //    {
        //        coin.transform.position = Vector3.MoveTowards(coin.transform.position, Vector3.Lerp(coin.transform.position, playerPosition.position, 0.2f), 0.2f);
        //    }
        //}
        for (int i = coinsInRange.Count - 1; i >= 0; i--)
        {
            if (coinsInRange[i] != null)
            {
                coinsInRange[i].transform.position = Vector3.MoveTowards(coinsInRange[i].transform.position, Vector3.Lerp(coinsInRange[i].transform.position, playerPosition.position, 0.3f), tileMovement.movementSpeedGetterSetter * 1.5f * Time.fixedDeltaTime);
                
            }
            else
            {
                coinsInRange.RemoveAt(i);
            }
        }
        
    }


}
