using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialStages
    {
        BeginTutorial,
        TeachJump,
        TeachSlide,
        TeachSwipe,
        TeachMagnet,
        TeachSuperJump,
        TeachShield,
        EndOfTutorial
    }
    [SerializeField] private GameObject[] normalStartTiles;
    [SerializeField] private GameObject[] tutorialTiles;
    [SerializeField] private Movement playerMovement;

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
            //playerMovement.DisablePlayerInput();
        }

    }

    public void TriggerTutorialStep(TutorialStages tutorialStage)
    {
        switch (tutorialStage)
        {
            case TutorialStages.BeginTutorial:
                break;
            case TutorialStages.TeachJump:
                break;
            case TutorialStages.TeachSlide:
                break;
            case TutorialStages.TeachSwipe:
                break;
            case TutorialStages.TeachMagnet:
                break;
            case TutorialStages.TeachSuperJump:
                break;
            case TutorialStages.TeachShield:
                break;
            case TutorialStages.EndOfTutorial:
                CompleteTutorial();
                break;
        }
    }


    public void CompleteTutorial()
    {
        PlayerPrefs.SetInt("TutorialComplete", 1);
        playerMovement.EnablePlayerInput();
    }
}
