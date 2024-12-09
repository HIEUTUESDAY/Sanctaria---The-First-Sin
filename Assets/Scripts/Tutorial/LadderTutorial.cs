using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTutorial : MonoBehaviour
{
    [SerializeField] private GameObject climbIcon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!TutorialManager.Instance.ladderTutor)
            {
                StartCoroutine(LadderTutorialCoroutine());
            }
            else
            {
                climbIcon.SetActive(false);
            }
        }
    }


    private IEnumerator LadderTutorialCoroutine()
    {
        climbIcon.SetActive(true);
        TutorialManager.Instance.ladderTutor = true;
        yield return new WaitForSeconds(10f);
        climbIcon.SetActive(false);
    }
}
