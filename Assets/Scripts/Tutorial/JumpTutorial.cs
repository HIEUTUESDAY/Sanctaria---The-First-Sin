using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTutorial : MonoBehaviour
{
    [SerializeField] private GameObject jumpIcon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { 
            if (!TutorialManager.Instance.jumpTutor)
            {
                StartCoroutine(JumpTutorialCoroutine());
            }
            else
            {
                jumpIcon.SetActive(false);
            }
        }
    }


    private IEnumerator JumpTutorialCoroutine()
    {
        jumpIcon.SetActive(true);
        TutorialManager.Instance.jumpTutor = true;
        yield return new WaitForSeconds(10f);
        jumpIcon.SetActive(false);
    }
}
