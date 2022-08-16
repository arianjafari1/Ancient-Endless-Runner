using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    ///          - powerup now rotates in the correct orientation
    ///          - rotation differs between collectables, so added a rotation direction enum so each collectable can be 
    ///            set individually
    /// 12/08/22 - moved end powerup function to the score manager, as the powerup wouldnt end if the powerup object
    ///            despawned before the end of the countdown.
    /// 16/08/22 - added effects for each powerup
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
    private AudioManager audioManager;
    private Movement playerMovement;
    [SerializeField] private float collectableRotation;
    private enum RotationDirection
    {
        up,
        side,
        forward
    }
    [SerializeField] private RotationDirection rotationDirection;
    [SerializeField] private float powerUpDuration;
    [SerializeField] private GameObject collectable;
    private SphereCollider coinMagnet;
    [SerializeField] private ScriptableRendererFeature ShieldEffect;
    private ParticleSystem MagnetEffect;
    private ParticleSystem FeatherEffect;

    private void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        coinMagnet = GameObject.FindGameObjectWithTag("CoinMagnet").GetComponent<SphereCollider>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        MagnetEffect = GameObject.FindGameObjectWithTag("ShieldEffect").GetComponent<ParticleSystem>(); //I gave the tag the wrong name
        FeatherEffect = GameObject.FindGameObjectWithTag("JumpEffect").GetComponent<ParticleSystem>();
    }
    private void FixedUpdate()
    {
        switch (rotationDirection)
        {
            case RotationDirection.up:
                collectable.transform.Rotate(Vector3.up, collectableRotation);
                break;
            case RotationDirection.forward:
                collectable.transform.Rotate(Vector3.forward, collectableRotation);
                break;
            case RotationDirection.side:
                collectable.transform.Rotate(Vector3.left, collectableRotation);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (collectableType)
            {
                case CollectableType.Coin:
                    scoreManager.CoinCollected();
                    audioManager.StopSound(9);
                    audioManager.PlaySound("Coin");
                    break;
                case CollectableType.CoinMagnetPower:
                    coinMagnet.enabled = true;
                    scoreManager.StartPowerupCountdown(powerUpDuration);
                    MagnetEffect.Play();
                    break;
                case CollectableType.FeatherJumpPower:
                    playerMovement.IsSuperJumpActive = true;
                    scoreManager.StartPowerupCountdown(powerUpDuration);
                    FeatherEffect.Play();
                    break;
                case CollectableType.CheatDeathPower:
                    playerMovement.IsShieldActive = true;
                    scoreManager.StartPowerupCountdown(powerUpDuration);
                    ShieldEffect.SetActive(true);
                    break;

            }

            gameObject.SetActive(false);
        }
    }

    //public void EndPowerUp()
    //{
    //    Debug.Log("Powerup ends");
    //    coinMagnet.enabled = false;
    //    playerMovement.IsSuperJumpActive = false;
    //    playerMovement.IsShieldActive = false;
    //    
    //}

}
