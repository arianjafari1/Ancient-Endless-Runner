using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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

        inputActions.Tutorial.Disable();
    }

    public void TriggerTutorialStep(TutorialStages tutorialStage)
    {
        switch (tutorialStage)
        {
            case TutorialStages.BeginTutorial:
                Debug.Log("Start tuto");
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
