using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    ///Code done by Arian- start
    
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
