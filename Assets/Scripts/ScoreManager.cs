using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

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
    ///          - moved the endpowerup function to this script, as it would never be called in the powerup
    ///            script if the tile the powerup was on gets despawned, causing infinite powerup 
    /// 15/08/22 - score doesnt increase during the cutscene
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
    [SerializeField] private ScriptableRendererFeature ShieldEffect;
    private ParticleSystem MagnetEffect;
    private ParticleSystem FeatherEffect;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        gameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameState>();
        coinMagnet = GameObject.FindGameObjectWithTag("CoinMagnet").GetComponent<SphereCollider>();
        MagnetEffect = GameObject.FindGameObjectWithTag("ShieldEffect").GetComponent<ParticleSystem>(); //I gave the tag the wrong name
        FeatherEffect = GameObject.FindGameObjectWithTag("JumpEffect").GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        //cancels update during cutscne
        if (gameState.CurrentGameState == GameState.gameState.beginningCutScene)
        {
            return;
        }

        //updates the coin and score text
        coinsUI.text = "x" + coinsCollected.ToString();
        scoreUI.text = "Score: " + score.ToString();

        //score increment is lower if the player is staggered
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
    //collecting a coin increases score, and increments coinscollected by 1
    public void CoinCollected()
    {
        coinsCollected++;
        score += coinScoreIncrease;
    }


    //saves the players stats to the playerprefs
    public void AddLifetimeStats()
    {

        int PreviousSavedCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        int PreviousTotalTime = PlayerPrefs.GetInt("TotalTime", 0);
        int PreviousTotalDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);

        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        PlayerPrefs.SetInt("TotalCoins", PreviousSavedCoins + coinsCollected);
        PlayerPrefs.SetInt("TotalTime", PreviousTotalTime + gameState.GlobalTime);
        PlayerPrefs.SetInt("TotalDeaths", PreviousTotalDeaths + 1);


    }

    //turns off the powerup effects, as well as the visual effects
    private void EndPowerUp()
    {
        playerMovement.IsShieldActive = false;
        playerMovement.IsSuperJumpActive = false;
        coinMagnet.enabled = false;

        MagnetEffect.Stop();
        FeatherEffect.Stop();
        ShieldEffect.SetActive(false);

    }
    //sets the invoke for ending the powerup, so that the object that used to do it doesnt get 
    //despawned while attempting it and never turns powerup off
    public void StartPowerupCountdown(float powerUpDuration)
    {
        Invoke(nameof(EndPowerUp), powerUpDuration);
    }
}
