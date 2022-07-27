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
    /// 12/07/22 - changed coinMagnet object type to SphereCollider instead of the CoinMagnet script, as this allows
    ///            any coins which had already entered the collider to be affected by the magnet.
    /// 21/07/22 - added method to stop powerup after x seconds;
    /// 26/07/22 - added feather jump powerup
    /// 27/07/22 - added shield powerup
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
    private Movement playerMovement;
    [SerializeField] private float collectableRotation;
    [SerializeField] private float powerUpDuration;
    [SerializeField] private GameObject collectable;
    private SphereCollider coinMagnet;

    private void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();
        coinMagnet = GameObject.FindWithTag("CoinMagnet").GetComponent<SphereCollider>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<Movement>();
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
                    Invoke(nameof(EndPowerUp), powerUpDuration);
                    break;
                case CollectableType.FeatherJumpPower:
                    playerMovement.ChangeJumpPower(true);
                    Invoke(nameof(EndPowerUp), powerUpDuration);
                    break;
                case CollectableType.CheatDeathPower:
                    playerMovement.IsShieldActive = true;
                    Invoke(nameof(EndPowerUp), powerUpDuration);
                    break;

            }

            gameObject.SetActive(false);
        }
    }

    public void EndPowerUp()
    {
        coinMagnet.enabled = false;
        playerMovement.ChangeJumpPower(false);
        playerMovement.IsShieldActive = false;
    }
}
