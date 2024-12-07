using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTutorial : MonoBehaviour
{
    [SerializeField] private GameObject dashIcon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!TutorialManager.Instance.dashTutor)
            {
                Player.Instance.DashTutorial();
                StartCoroutine(DashTutorialCoroutine());
            }
            else
            {
                dashIcon.SetActive(false);
            }
        }
    }


    private IEnumerator DashTutorialCoroutine()
    {
        dashIcon.SetActive(true);
        TutorialManager.Instance.dashTutor = true;
        yield return new WaitForSeconds(10f);
        dashIcon.SetActive(false);
    }
}
