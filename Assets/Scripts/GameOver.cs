using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;

    private void showGameOverScreen(int score)
    {
        gameObject.SetActive(true); // set the gameover screen to active
        scoreText.text = "Score: " + score.ToString(); //set the score

    }

    private void restartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void menuButton() //takes you to the main menu
    {
        SceneManager.LoadScene("MainMenu");
    }



    public void ShowGameOverScreen(int inputScore) //getter for private function
    {
        showGameOverScreen(inputScore);
    }

    public void RestartButton() //getter for private function
    {
        restartButton();
    }

    public void MenuButton()  //getter for private function
    {
        menuButton();
    }
}
