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
    /// 13/07/22 - score goes up less when staggered, and stops when player has died
    /// 14/07/22 - added score getter
    /// 
    /// </summary>

    private int coinsCollected = 0;
    private int score = 0;
    public int getCoins
    {
        get
        {
            return coinsCollected;
        }
    }
    public int getScore
    {
        get
        {
            return score;
        }
    }
    [SerializeField] int scoreIncrement;
    [SerializeField] private int staggeredScoreIncrement;
    [SerializeField] private int coinScoreIncrease;
    [SerializeField] private TMP_Text coinsUI;
    [SerializeField] private TMP_Text scoreUI;
    private Movement playerMovement;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }

    private void FixedUpdate()
    {
        coinsUI.text = "Coins Collected: " + coinsCollected.ToString();
        scoreUI.text = "Score: " + score.ToString();

        switch (playerMovement.getPlayerState)
        {
            case Movement.PlayerStates.running:
                score += scoreIncrement;
                break;
            case Movement.PlayerStates.staggered:
                score += staggeredScoreIncrement;
                break;
        }
        
    }

    public void CoinCollected()
    {
        coinsCollected++;
        score += coinScoreIncrease;
    }

}
