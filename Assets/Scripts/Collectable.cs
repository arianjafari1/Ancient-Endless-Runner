using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private float collectableRotation;
    [SerializeField] private GameObject collectable;

    private void FixedUpdate()
    {
        collectable.transform.Rotate(Vector3.forward, collectableRotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            scoreManager.CoinCollected();
            gameObject.SetActive(false);
        }
    }
}
