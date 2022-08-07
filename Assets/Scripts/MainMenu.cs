using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    ///Code done by Arian- start

    /// <summary>
    /// 29/06/2022 -[Arian] created MainMenu script
    ///            -[Arian] added functions for the Start Button, exit button
    /// </summary>

    private GameState gameState;
    private AudioManager audioManager; //Setup Audio Manager    
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); //start audio manager
        gameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameState>(); //reference the object with script
        audioManager.PlaySound("GameMenuMusic");
    }

    private void startGame()
    {
        SceneManager.LoadScene("SampleScene");
        gameState.CurrentGameState = GameState.gameState.isPlaying;
    }

    private void exitButton()
    {
        Application.Quit();
        Debug.Log("Game is closed. As the game doesn't close in the editor otherwise.");
    }


    public void StartGame()
    {
        startGame();
    }

    public void ExitGame()
    {
        exitButton();
    }



/// code done by Arian- end
}
