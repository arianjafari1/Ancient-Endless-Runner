using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    /// <summary>
    /// 07/07/2022 -[Arian] created GameOver Script
    ///            -[Arian] created GameOver Screen in Canvas with functional buttons and score text
    ///            -[Arian] added code for the restartButton to restart scene, and for the main menu button to go to the main menu of the game
    ///            -[Arian] added ShowGameOverScreen function for setting game over screen to active, and adding score text on it, this function should be called in other scripts
    /// 14/07/22 - [MALACHI] added score manager to display the actual score
    ///          - [MALACHI] made score update automatically rather than having it as a parameter in the function
    /// 20/07/2022 -[Arian] change gameState from GameState Script to gameOver in the ShowGameOverScreen function 
    /// 22/07/22 - [MALACHI] moved changing gamestate to gameover when player dies and not when the gameover screen appears
    /// </summary>

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeAliveText;
    [SerializeField] private TextMeshProUGUI coinsCollectedText;
    private ScoreManager scoreManager;
    private GameState gameState;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); //start audio manager
        scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();
        gameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameState>();
        scoreText.text = "Score: " + scoreManager.getScore.ToString();
        timeAliveText.text = "Time alive: " + gameState.GlobalTime.ToString() + " sec";
        coinsCollectedText.text = "Coins collected: " + scoreManager.getCoins.ToString();
        audioManager.PlaySound("GameMenuMusic"); //Play Menu Music
    }
    public void ShowGameOverScreen()//int score)
    {
        gameObject.SetActive(true); // set the gameover screen to active
        //gameState.CurrentGameState = GameState.gameState.gameOver; //set the game state to game over
        //scoreText.text = "Score: " + score.ToString(); //set the score
        audioManager.StopSound(4); //Stop Game Music
        scoreManager.AddLifetimeStats();
    }

    private void restartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void menuButton() //takes you to the main menu
    {
        SceneManager.LoadScene("MainMenu");
    }



    //public void ShowGameOverScreen(int inputScore) //getter for private function
    //{
    //    showGameOverScreen(inputScore);
    //}

    public void RestartButton() //getter for private function
    {
        restartButton();
    }

    public void MenuButton()  //getter for private function
    {
        menuButton();
    }
}
