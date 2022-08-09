using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Spawner : MonoBehaviour
{
   // [SerializeField] private Transform spawnTrans;
    public GameObject[] powerUpType;
    //[SerializeField] private Vector3 spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        //spawnTrans = spawnTrans.GetComponent<Transform>();
        //spawnPos = spawnTrans.position;
        int powerUp_num = Random.Range(0,this.powerUpType.Length);
        GameObject newPowerUp = Instantiate(powerUpType[powerUp_num], this.transform.position, this.transform.rotation);

        newPowerUp.transform.parent = this.transform;
    }
}
