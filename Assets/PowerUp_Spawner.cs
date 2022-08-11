using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Spawner : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Oakley unless otherwise specified
    /// Script created 09/08/22 by Oakley
    /// 
    /// 09/08/22 - created script to choose a random powerup to spawn
    /// </summary>

    //[SerializeField] private Transform spawnTrans;
    public GameObject[] powerUpType;
    //[SerializeField] private Vector3 spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        //spawnTrans = spawnTrans.GetComponent<Transform>();
        //spawnPos = spawnTrans.position;

        if (GameObject.FindGameObjectWithTag("PowerUp") != null)
        {
            GameObject newCoin = Instantiate(powerUpType[3], this.transform.position, this.transform.rotation);
            newCoin.transform.parent = this.transform;
            return;
        }

        int powerUp_num = Random.Range(0, this.powerUpType.Length - 1);
        GameObject newPowerUp = Instantiate(powerUpType[powerUp_num], this.transform.position, this.transform.rotation);

        newPowerUp.transform.parent = this.transform;
        Debug.Log("Spawn powerup");
    }
}
