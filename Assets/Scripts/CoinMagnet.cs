using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [SerializeField]private List<GameObject> coinsInRange;

    private void Start()
    {
        coinsInRange = new List<GameObject>();
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

        foreach (GameObject coin in coinsInRange)
        {
            if (coin != null)
            {
                coin.transform.position = Vector3.MoveTowards(coin.transform.position, Vector3.Lerp(coin.transform.position, playerPosition.position, 0.2f), 0.2f);
            }
        }
        
    }


}
