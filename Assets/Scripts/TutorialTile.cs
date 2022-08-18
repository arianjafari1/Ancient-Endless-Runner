using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTile : MonoBehaviour
{
    /// <summary>
    /// Code and dev notes by Malachi unless otherwise specified
    /// Script created 04/08/22 by Malachi
    /// 
    /// 04/08/22 - created tutorial tile class which has a tutorial stage and a function which
    ///            uses this stage to trigger the corresponding function in the tutorial manager 
    ///            when the player collides with it.
    /// </summary>
    /// 
    private TutorialManager tutorialManager;
    [SerializeField] private TutorialManager.TutorialStages tutorialStage;

    private void Awake()
    {
        tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
    }

    //triggers the tutorial step when the player hits the collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            tutorialManager.TriggerTutorialStep(tutorialStage);
        }
    }

}
