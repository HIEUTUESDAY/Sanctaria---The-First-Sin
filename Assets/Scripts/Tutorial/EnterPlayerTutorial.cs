using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPlayerTutorial : MonoBehaviour
{
    [SerializeField] private GameObject enterPlayerTutorial;

    private void Update()
    {
        EnterPlayerTutorialCheck();
    }

    private void EnterPlayerTutorialCheck()
    {
        if (!TutorialManager.Instance.enterPlayerTutor)
        {
            enterPlayerTutorial.SetActive(true);
        }
        else if (TutorialManager.Instance.enterPlayerTutor)
        {
            enterPlayerTutorial.SetActive(false);
        }
    }
}
