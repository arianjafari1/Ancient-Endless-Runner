using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{

    /// <summary>
    /// 04/08/2022 -[Arian] created PauseMenu script
    ///            -[Arian] added reference to the gameState script so I can change the CurrentGame state of the game to isPaused
    ///            and then set it back to isPlaying when resuming the game
    ///            -[Arian] created functions for all buttons that I have assigned those functions to the buttons in the editor
    ///            -[Arian] created Resume and Pause functions
    /// </summary>

    private GameState gameState;
    [SerializeField] private GameObject backGroundPauseMenu;

    // Start is called before the first frame update
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameState>(); //reference the object with script
    }


    public void ShowGameOverScreen()
    {
            backGroundPauseMenu.SetActive(true); // set the pauseMenu screen to active
            gameState.CurrentGameState = GameState.gameState.isPaused;
    }

    private void pauseGame()
    {
        switch (Time.timeScale)
        {
            case 1:
                Time.timeScale = 0;
                break;
        }
    }

    private void resumeGame()
    {
        switch (Time.timeScale)
        {
            default:
                backGroundPauseMenu.SetActive(false);
                gameState.CurrentGameState = GameState.gameState.isPlaying;
                Time.timeScale = 1;
                break;
        }
    }

    private void restartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void menuButton() //takes you to the main menu
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void PauseGame()
    {
        pauseGame();
    }

    public void ResumeGame()
    {
        resumeGame();
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
