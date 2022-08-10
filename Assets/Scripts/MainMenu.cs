using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    ///Code done by Arian- start

    /// <summary>
    /// 29/06/2022 -[Arian] created MainMenu script
    ///            -[Arian] added functions for the Start Button, exit button
    ///09/08/2022  -[Arian] added a slider for the overall sound of the game
    ///            -[Arian] added a function which assigns the volume from audio listener to the value of the volume slider
    ///            -[Arian] added a PlayerPrefs save function to save the settings that the player makes, as well as a load function to load in those choices
    ///            -[Arian] also added a function to be called in awake or start that checks for previous saved settings
    /// </summary>

    private GameState gameState;
    [SerializeField] private AudioManager audioManager; //Setup Audio Manager    
    [SerializeField] private Slider volumeSlider;
    private const float defaultSliderValue= 1f; //default value of the Volume slider

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); //start audio manager
        audioManager.PlaySound("MainMenuMusic");
        checkForPreviousSettings(); //checks for previous saved settings in player prefs

    }

    public void changeVolume() //function to get the overall volume of the game from the audiolistner and assign it to the rge value of the volume slider
    {
        AudioListener.volume = volumeSlider.value; //volume of the game is equal to the value of the slider
        SavePlayerSettings(); //save the settings in player prefab
    }

    private void SavePlayerSettings() //function to save the player's prefered settings to
    {
        PlayerPrefs.SetFloat("generalVolume", volumeSlider.value); //save our date under the key string generalVolume, and data is the value of the volumeSlider
    }

    private void LoadPlayerVolume()//load the player settings for volume
    {
        volumeSlider.value = PlayerPrefs.GetFloat("generalVolume");  //load the value with the key string generalVolume to load the player settings for volume
    }

    private void checkForPreviousSettings() //function to be called in awake or start to check for previous saved settings
    {
        if (!PlayerPrefs.HasKey("generalVolume")) //if there is no date from game session from the past, then the volume will be automatically set to 1
        {
            PlayerPrefs.SetFloat("generalVolume", defaultSliderValue); //setting it to 1 (100%)
            LoadPlayerVolume();
        } else
        {
            LoadPlayerVolume();
        }
    }

    private void resetPlayerPref()
    {
        PlayerPrefs.DeleteAll(); //resetting player prefs
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

    public void ResetGameSettings() //calling it when the button Reset Game Settings to Default is clicked
    {
        resetPlayerPref();
        volumeSlider.value = defaultSliderValue;
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
