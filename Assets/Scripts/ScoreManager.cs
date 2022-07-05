using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int coinsCollected = 0;
    private int score = 0;
    [SerializeField] int scoreIncrement;
    [SerializeField] private int coinScoreIncrease;
    [SerializeField] private TMP_Text coinsUI;
    [SerializeField] private TMP_Text scoreUI;

    private void Update()
    {
        coinsUI.text = "Coins Collected: " + coinsCollected.ToString();
        scoreUI.text = "Score: " + score.ToString();
    }

    private void FixedUpdate()
    {
        score += scoreIncrement;
    }

    public void CoinCollected()
    {
        coinsCollected++;
        score += coinScoreIncrease;
    }

}
