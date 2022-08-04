using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTile : MonoBehaviour
{
    private TutorialManager tutorialManager;
    [SerializeField] private TutorialManager.TutorialStages tutorialStage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            tutorialManager.TriggerTutorialStep(tutorialStage);
        }
    }

}
