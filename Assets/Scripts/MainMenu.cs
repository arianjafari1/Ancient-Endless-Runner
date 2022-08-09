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
    [SerializeField] private AudioManager audioManager; //Setup Audio Manager    
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); //start audio manager
        audioManager.PlaySound("MainMenuMusic");
    }

    private void startGame()
    {
        SceneManager.LoadScene("SampleScene");
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
