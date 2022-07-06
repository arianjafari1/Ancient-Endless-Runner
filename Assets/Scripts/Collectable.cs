using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// Script created 01/06/22 by Malachi
    /// 
    /// 01/07/22 - added collision with player which turns the object to false
    /// 05/07/22 - set object to rotate
    ///          - linked score manager to add score whenever a coin is collected
    /// 06/07/22 - unserialized the score manager, as you cant link it in prefab. instead searches for the score
    ///            manager based on the tag
    /// 
    /// </summary>

    private enum CollectableType
    {
        Coin,
        CoinMagnetPower,
        FeatherJumpPower,
        CheatDeathPower
    }
    [SerializeField] private CollectableType collectableType;

    private ScoreManager scoreManager;
    [SerializeField] private float collectableRotation;
    [SerializeField] private GameObject collectable;
    private CoinMagnet coinMagnet;

    private void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();
        coinMagnet = GameObject.FindGameObjectWithTag("CoinMagnet").GetComponent<CoinMagnet>();
    }
    private void FixedUpdate()
    {
        collectable.transform.Rotate(Vector3.forward, collectableRotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (collectableType)
            {
                case CollectableType.Coin:
                    scoreManager.CoinCollected();
                    break;
                case CollectableType.CoinMagnetPower:
                    coinMagnet.enabled = true;
                    break;

            }

            gameObject.SetActive(false);
        }
    }
}
