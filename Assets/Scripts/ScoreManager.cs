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
    /// 12/08/22 - added high score/lifetime stats saving.
    /// 
    /// </summary>

    private int coinsCollected = 0;
    private int score = 0;
    private GameState gameState;
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
    private SphereCollider coinMagnet;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        gameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameState>();
        coinMagnet = GameObject.FindGameObjectWithTag("CoinMagnet").GetComponent<SphereCollider>();
    }

    private void FixedUpdate()
    {
        if (gameState.CurrentGameState == GameState.gameState.beginningCutScene)
        {
            return;
        }

        coinsUI.text = "x" + coinsCollected.ToString();
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



    public void AddLifetimeStats()
    {

        int PreviousSavedCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        int PreviousTotalTime = PlayerPrefs.GetInt("TotalTime", 0);

        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        PlayerPrefs.SetInt("TotalCoins", PreviousSavedCoins + coinsCollected);
        PlayerPrefs.SetInt("TotalTime", PreviousTotalTime + gameState.GlobalTime);

        Debug.Log(PlayerPrefs.GetInt("TotalCoins"));
        Debug.Log(PlayerPrefs.GetInt("TotalTime"));
        Debug.Log(PlayerPrefs.GetInt("HighScore"));

    }

    private void EndPowerUp()
    {
        playerMovement.IsShieldActive = false;
        playerMovement.IsSuperJumpActive = false;
        coinMagnet.enabled = false;

    }

    public void StartPowerupCountdown(float powerUpDuration)
    {
        Invoke(nameof(EndPowerUp), powerUpDuration);
    }
}
