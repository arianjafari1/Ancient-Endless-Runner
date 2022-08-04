using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{

    /// <summary>
    /// 03/08/2022 -[Arian] created PauseMenu script
    ///            -[Arian] added reference to the gameState script so I can change the CurrentGame state of the game to isPaused
    ///            and then set it back to isPlaying when resuming the game
    ///            -[Arian] created functions for all buttons that I have assigned those functions to the buttons in the editor
    ///            -[Arian] created Resume and Pause functions
    /// 04/08/2022 -[Arian] referenced the audioManager, so I can use it to stop looping sounds when pausing and restart them when resuming
    /// </summary>

    private GameState gameState;
    private AudioManager audioManager;
    [SerializeField] private GameObject backGroundPauseMenu;

    // Start is called before the first frame update
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameState>(); //reference the object with script
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); //refer to the script in the empty object
    }


    public void ShowPauseMenuScreen()
    {
        backGroundPauseMenu.SetActive(true); // set the pauseMenu screen to active
        gameState.CurrentGameState = GameState.gameState.isPaused; //se the current GameState to isPaused
        audioManager.StopSound(2); //stoping the boulder sound when pausing the menu
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
                audioManager.PlaySound("Boulder_Movement");
                Time.timeScale = 1;
                break;
        }
    }

    private void restartButton()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }

    private void menuButton() //takes you to the main menu
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;

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
