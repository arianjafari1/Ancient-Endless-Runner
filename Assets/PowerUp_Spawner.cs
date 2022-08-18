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
    /// 10/08/22 - [MALACHI] made it so powerups can only spawn if one doesnt exist in the scene. will
    ///            spawn a coin instead.
    /// 17/08/22 - [MALACHI] made it so powerups wont spawn if one is already active. this should prevent
    ///            them overlapping.
    /// </summary>

    //[SerializeField] private Transform spawnTrans;
    [SerializeField] private GameObject[] powerUpType;
    //[SerializeField] private Vector3 spawnPos;

    private SphereCollider coinMagnet;
    private Movement playerMovement;

    void Start()
    {
        //spawnTrans = spawnTrans.GetComponent<Transform>();
        //spawnPos = spawnTrans.position;
        coinMagnet = GameObject.FindGameObjectWithTag("CoinMagnet").GetComponent<SphereCollider>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();

        if (GameObject.FindGameObjectWithTag("PowerUp") != null || coinMagnet.enabled == true || playerMovement.IsShieldActive == true || playerMovement.IsSuperJumpActive)
        {
            //sets a coin to the object if powerup is active/in scene already
            GameObject newCoin = Instantiate(powerUpType[powerUpType.Length - 1], this.transform.position, this.transform.rotation);
            newCoin.transform.parent = this.transform;
            return;
        }
        //otherwise spawns a random powerup
        int powerUp_num = Random.Range(0, this.powerUpType.Length - 1);
        GameObject newPowerUp = Instantiate(powerUpType[powerUp_num], this.transform.position, this.transform.rotation);

        newPowerUp.transform.parent = this.transform;
    }
}
