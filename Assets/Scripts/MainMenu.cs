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

    [SerializeField] private Slider volumeSlider;
    private Dropdown resolutionChanger;
    private const float defaultSliderValue= 1f; //default value of the Volume slider
    private List<int> widths = new List<int>() {848, 1024, 1280, 1280, 1366, 1440, 1600, 1680, 1920, 1920, 2560, 2560, 3840, 3840, 5120, 5120, 7680, 8192 }; //list of widths that will be used for resolutions
    private List<int> heights = new List<int>() {480, 640, 720, 800, 768, 900, 900, 1050, 1080, 1200, 1400, 1600, 2160, 2400, 2880, 3200, 4320, 5120 }; //list of heights that will be used for resolutions

    private void Awake()
    {
        resolutionChanger = GetComponent<Dropdown>();
        checkForPreviousSettings(); //checks for previous saved settings in player prefs
        Debug.Log (Screen.width + " X " + Screen.height);
        Debug.Log(resolutionChanger.value);
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

    public void SetScreenResolution (int i) //a function to set the screen resolution based on whether the screen is fullscreen and windows and out width and height lists
    {
        bool fullScreen = Screen.fullScreen; //a bolean that will return whether the game is fullscreen or not as either true or false
        int width = widths[i];
        int height = heights[i];
        Screen.SetResolution(width, height, fullScreen); //setting the screen resolution
    }

    public void SetFullScreenOrWindowed(bool _fullScreen) //function to set the game to windowed or full screen
    {
        if (_fullScreen)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
    
    }

    public void SetVsyncOnOff()
    {
        return;
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
