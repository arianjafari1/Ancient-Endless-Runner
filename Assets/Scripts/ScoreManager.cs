using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// Script created 05/06/22 by Malachi
    /// 
    /// 05/07/22 - added score increment every tic
    ///          - added UI elements to show score and coins
    ///          - added function to add score when coins are collected, and increase coins collected
    /// 
    /// </summary>

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
