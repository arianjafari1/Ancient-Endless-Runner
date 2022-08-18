using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// Script created 03/08/22 by Malachi
    /// 
    /// 03/08/22 - Created 2 arrays, one for regular start tiles and one for tutorial, and use playerprefs
    ///            to decide which ones are set to active at the start of the game. If no playerprefs is 
    ///            found, it spawns tutorial tiles, otherwise regular ones are spawned
    ///          - Created enum for tutorial stages
    ///          - set it so regular controls are disabled during the tutorial, which gets renabled in the 
    ///            complete tutorial function
    /// 04/08/22 - created trigger function for each stage of the tutorial
    ///          - added special input actions for the tutorial so i dont have to enable the regular controls
    ///          - created functions for each stage of the tutorial
    ///          - added advance tutorial function which sets timescale to 1
    ///          - sets timescale to 0 to pause the game for each stage of the tutorial.
    ///          - added each required input to the corresponding stage of the tutorial
    ///          - added the text popups to the tutorial which show up when required
    /// 17/08/22 - changed tmp objects to gameobjects, as i gave them backgrounds
    /// </summary>
    public enum TutorialStages
    {
        BeginTutorial,
        TeachJump,
        TeachSlide,
        TeachSwipeLeft,
        TeachSwipeRight,
        EndOfTutorial
    }
    [SerializeField] private GameObject[] normalStartTiles;
    [SerializeField] private GameObject[] tutorialTiles;
    [SerializeField] private Movement playerMovement;

    private MovementInputActions inputActions;
    private InputAction advanceTutorial;
    private InputAction teachJump;
    private InputAction teachSlide;
    private InputAction teachLeft;
    private InputAction teachRight;

    [SerializeField] private GameObject advanceText;
    [SerializeField] private GameObject JumpText;
    [SerializeField] private GameObject SlideText;
    [SerializeField] private GameObject LeftText;
    [SerializeField] private GameObject RightText;

    private void Awake()
    {
        inputActions = new MovementInputActions();
    }

    private void OnEnable()
    {
        advanceTutorial = inputActions.Tutorial.Advance;
        teachJump = inputActions.Tutorial.Jump;
        teachSlide = inputActions.Tutorial.Slide;
        teachLeft = inputActions.Tutorial.Left;
        teachRight = inputActions.Tutorial.Right;

        advanceTutorial.Enable();
        teachJump.Enable();
        teachSlide.Enable();
        teachLeft.Enable();
        teachRight.Enable();

        advanceTutorial.performed += AdvanceTutorial_performed;
        teachJump.performed += TeachJump_performed;
        teachSlide.performed += TeachSlide_performed;
        teachLeft.performed += TeachLeft_performed;
        teachRight.performed += TeachRight_performed;
    }
    private void OnDisable()
    {
        advanceTutorial.Disable();
        teachJump.Disable();
        teachSlide.Disable();
        teachLeft.Disable();
        teachRight.Disable();
    }

    //Checks if the tutorial has been completed before. if it has, will turn off the normal start tiles,
    //turn on the tutorial and turn off regular input.
    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialComplete", 0) == 0)
        {
            foreach (GameObject tile in normalStartTiles)
            {
                tile.SetActive(false);
            }
            foreach (GameObject tile in tutorialTiles)
            {
                tile.SetActive(true);
            }
            playerMovement.DisablePlayerInput();
            
        }
        //this gets disabled so specific actions can be enabled for each tutorial stage
        inputActions.Tutorial.Disable();
    }
    
    /// <summary>
    /// These pause the game, the enable the specific  input action for each stage to prevent the player
    /// doing inputs when not prompted. this prevents death/staggering during the tutorial.
    /// </summary>
    /// <param name="tutorialStage">the stage the tutorial is on</param>
    public void TriggerTutorialStep(TutorialStages tutorialStage)
    {
        switch (tutorialStage)
        {
            case TutorialStages.BeginTutorial:
                Time.timeScale = 0;
                inputActions.Tutorial.Advance.Enable();
                advanceText.gameObject.SetActive(true);
                break;
            case TutorialStages.TeachJump:
                Time.timeScale = 0;
                inputActions.Tutorial.Jump.Enable();
                JumpText.gameObject.SetActive(true);
                break;
            case TutorialStages.TeachSlide:
                Time.timeScale = 0;
                inputActions.Tutorial.Slide.Enable();
                SlideText.gameObject.SetActive(true);
                break;
            case TutorialStages.TeachSwipeLeft:
                Time.timeScale = 0;
                inputActions.Tutorial.Left.Enable();
                LeftText.gameObject.SetActive(true);
                break;
            case TutorialStages.TeachSwipeRight:
                Time.timeScale = 0;
                inputActions.Tutorial.Right.Enable();
                RightText.gameObject.SetActive(true);
                break;
            case TutorialStages.EndOfTutorial:
                CompleteTutorial();
                break;
        }
    }

    /// <summary>
    /// Each stage will remove the info text, redisable the input, then advance the tutorial. stages where
    /// an input will make player move (jump/slide/left/right) will trigger the corresponding function
    /// from player movement.
    /// </summary>
    #region tutorial stages
    
    private void AdvanceTutorial_performed(InputAction.CallbackContext obj)
    {
        advanceText.gameObject.SetActive(false);
        inputActions.Disable();
        AdvanceTutorial();
    }
    private void TeachJump_performed(InputAction.CallbackContext obj)
    {
        JumpText.gameObject.SetActive(false);
        playerMovement.Jump(obj);
        inputActions.Disable();
        AdvanceTutorial();
    }

    private void TeachSlide_performed(InputAction.CallbackContext obj)
    {
        SlideText.gameObject.SetActive(false);
        playerMovement.Slide(obj);
        inputActions.Disable();
        AdvanceTutorial();
    }
    private void TeachLeft_performed(InputAction.CallbackContext obj)
    {
        LeftText.gameObject.SetActive(false);
        playerMovement.GoLeft(obj);
        inputActions.Disable();
        AdvanceTutorial();
    }

    private void TeachRight_performed(InputAction.CallbackContext obj)
    {
        RightText.gameObject.SetActive(false);
        playerMovement.GoRight(obj);
        inputActions.Disable();
        AdvanceTutorial();
    }

    #endregion

    /// <summary>
    /// unpauses the game
    /// </summary>
    public void AdvanceTutorial()
    {
        Time.timeScale = 1;
    }

    /// <summary>
    /// sets the playerpref for tutorial complete so it doesnt trigger past the first run
    /// unless they reset their save file
    /// </summary>
    public void CompleteTutorial()
    {
        PlayerPrefs.SetInt("TutorialComplete", 1);
        playerMovement.EnablePlayerInput();
    }
}
