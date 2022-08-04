using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
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
            return;
            
        }

        inputActions.Disable();
    }

    public void TriggerTutorialStep(TutorialStages tutorialStage)
    {
        switch (tutorialStage)
        {
            case TutorialStages.BeginTutorial:
                Debug.Log("Start tuto");
                Time.timeScale = 0;
                break;
            case TutorialStages.TeachJump:
                break;
            case TutorialStages.TeachSlide:
                break;
            case TutorialStages.TeachSwipeLeft:
                break;
            case TutorialStages.TeachSwipeRight:
                break;
            case TutorialStages.EndOfTutorial:
                CompleteTutorial();
                break;
        }
    }

    private void AdvanceTutorial_performed(InputAction.CallbackContext obj)
    {
        AdvanceTutorial();
    }
    private void TeachJump_performed(InputAction.CallbackContext obj)
    {

    }

    private void TeachSlide_performed(InputAction.CallbackContext obj)
    {

    }
    private void TeachLeft_performed(InputAction.CallbackContext obj)
    {

    }

    private void TeachRight_performed(InputAction.CallbackContext obj)
    {

    }


    public void AdvanceTutorial()
    {
        Time.timeScale = 1;
    }

    public void CompleteTutorial()
    {
        PlayerPrefs.SetInt("TutorialComplete", 1);
        playerMovement.EnablePlayerInput();
    }
}
