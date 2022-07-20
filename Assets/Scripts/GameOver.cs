using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;
    private ScoreManager scoreManager;
    private GameState gameState;

    private void Awake()
    {
        scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();
        gameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameState>();
        scoreText.text = "Score: " + scoreManager.getScore.ToString();
    }
    public void ShowGameOverScreen()//int score)
    {
        gameObject.SetActive(true); // set the gameover screen to active
        gameState.CurrentGameState = GameState.gameState.gameOver; //set the game state to game over
        //scoreText.text = "Score: " + score.ToString(); //set the score

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
