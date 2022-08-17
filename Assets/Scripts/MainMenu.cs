using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    ///Code done by Arian- start

    /// <summary>
    /// 29/06/2022 -[Arian] created MainMenu script
    ///            -[Arian] added functions for the Start Button, exit button
    /// 09/08/2022 -[Arian] added a slider for the overall sound of the game
    ///            -[Arian] added a function which assigns the volume from audio listener to the value of the volume slider
    ///            -[Arian] added a PlayerPrefs save function to save the settings that the player makes, as well as a load function to load in those choices
    ///            -[Arian] also added a function to be called in awake or start that checks for previous saved settings
    /// 15/08/2022 -[Arian] added a switch statement ensuring that the correct resolution is selected in the dropdown menu
    ///            -[Arian] V-sync toggle fully working
    /// </summary>

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown resolutionChanger;
    [SerializeField] private Toggle fullScreen;
    [SerializeField] private Toggle vSync;
    private const float defaultSliderValue= 1f; //default value of the Volume slider
    private List<int> widths = new List<int>() { 848, 1024, 1280, 1280, 1366, 1440, 1600, 1680, 1920, 1920, 2560, 2560, 3840, 3840, 5120, 5120, 7680, 8192 }; //list of widths that will be used for resolutions
    private List<int> heights = new List<int>() { 480, 640, 720, 800, 768, 900, 900, 1050, 1080, 1200, 1400, 1600, 2160, 2400, 2880, 3200, 4320, 5120 }; //list of heights that will be used for resolutions
    private int savedHighScore;
    private int savedLifetimeCoins;
    private int savedLifetimeTimeRan;
    private int savedTotalDeaths;
    [SerializeField] private TMP_Text HighScoreText;
    [SerializeField] private TMP_Text CoinsText;
    [SerializeField] private TMP_Text TimeRanText;
    [SerializeField] private TMP_Text DeathsText;

    private void Awake()
    {
        fullScreen.isOn = true; //setting the screen to full screen as default
        setDropDownMenuToCorrectResolution(); //set the dropdown menu resolution selector to correct resolution
        checkForPreviousSettings(); //checks for previous saved settings in player prefs
        checkPlayerSaveFile();
        SetPlayerStats();

    }

    public void changeVolume() //function to get the overall volume of the game from the audiolistner and assign it to the rge value of the volume slider
    {
        AudioListener.volume = volumeSlider.value; //volume of the game is equal to the value of the slider
        SavePlayerSettings(); //save the settings in player prefab
    }

    private void SavePlayerSettings() //function to save the player's prefered settings to
    {
        PlayerPrefs.SetFloat("generalVolume", volumeSlider.value); //save our date under the key string generalVolume, and data is the value of the volumeSlider
        PlayerPrefs.SetInt("vSyncToggle", Convert.ToInt32(vSync.isOn)); // converting a bool for toggle into int and storing it in a player pref
        PlayerPrefs.SetInt("fullScreenToggle", Convert.ToInt32(fullScreen.isOn)); // converting a bool for toggle into int and storing it in a player pref
    }

    private void LoadPlayerVolume()//load the player settings for volume
    {
        volumeSlider.value = PlayerPrefs.GetFloat("generalVolume");  //load the value with the key string generalVolume to load the player settings for volume
        vSync.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("vSyncToggle")); //setting a toggle bool to the int converted to bool from player pref and loading it
        fullScreen.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("fullScreenToggle"));

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

    private void checkPlayerSaveFile()
    {
        savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
        savedLifetimeCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        savedLifetimeTimeRan = PlayerPrefs.GetInt("TotalTime", 0);
        savedTotalDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);
        
    }

    private void SetPlayerStats()
    {
        HighScoreText.text = savedHighScore.ToString();
        CoinsText.text = savedLifetimeCoins.ToString();
        TimeRanText.text = savedLifetimeTimeRan.ToString() + "s";
        DeathsText.text = savedTotalDeaths.ToString();

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
        SavePlayerSettings();
    
    }

    public void SetVsyncOnOff(bool _verticleSync)
    {
        if (_verticleSync) //if toggle is on then vsync is true
        {
            QualitySettings.vSyncCount = 1;
            Debug.Log("V-Sync is on");
        }
        else //otherwise vsync is off
        {
            QualitySettings.vSyncCount = 0;
            Debug.Log("V-Sync is off");
        }
        SavePlayerSettings();
    }


    private void setDropDownMenuToCorrectResolution() //function to change the drop down menu to the correct resolution of the screen
    {
        switch (Screen.width, Screen.height)
        {
            case (848, 480): //in case this is the current resolution of the screen, then...
                resolutionChanger.value = 0;//... set the dropdown menu to this option (in this specific case 0)
                break;
            case (1024, 640):
                resolutionChanger.value = 1;
                break;
            case (1280, 720):
                resolutionChanger.value = 2;
                break;
            case (1280, 800):
                resolutionChanger.value = 3;
                break;
            case (1366, 768):
                resolutionChanger.value = 4;
                break;
            case (1440, 900):
                resolutionChanger.value = 5;
                break;
            case (1600, 900):
                resolutionChanger.value = 6;
                break;
            case (1680, 1050):
                resolutionChanger.value = 7;
                break;
            case (1920, 1080):
                resolutionChanger.value = 8;
                break;
            case (1920, 1200):
                resolutionChanger.value = 9;
                break;
            case (2560, 1400):
                resolutionChanger.value = 10;
                break;
            case (2560, 1600):
                resolutionChanger.value = 11;
                break;
            case (3840, 2160):
                resolutionChanger.value = 12;
                break;
            case (3840, 2400):
                resolutionChanger.value = 13;
                break;
            case (5120, 2880):
                resolutionChanger.value = 14;
                break;
            case (5120, 3200):
                resolutionChanger.value = 15;
                break;
            case (7680, 4320):
                resolutionChanger.value = 16;
                break;
            case (8192, 5120):
                resolutionChanger.value = 17;
                break;
            default: //if the game is set to non of the above resolution then set it to the lowest resolution tha game support, with fullscreen on
                resolutionChanger.value = 0;
                Screen.SetResolution(widths[0], heights[0], true);
                break;


        }

    }


    private void startGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void exitButton()
    {
        Application.Quit();
    }

    public void ResetGameSettings() //calling it when the button Reset Game Settings to Default is clicked
    {
        resetPlayerPref(); //reset the prefs
        SetPlayerStats();
        volumeSlider.value = defaultSliderValue; //setting the volume slider to its default value
        fullScreen.isOn = true; //full screen toggle is ticked and...
        Screen.fullScreen = true;//... full screen is set to true
        vSync.isOn = false; //vSync toggle is set to off and...
        QualitySettings.vSyncCount = 0;//...v-sync is set to false
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
