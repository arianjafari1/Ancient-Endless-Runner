using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialStages
    {
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
        }

    }
}
