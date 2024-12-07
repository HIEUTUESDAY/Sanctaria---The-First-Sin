using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTutorial : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!TutorialManager.Instance.attackTutor)
            {
                Player.Instance.AttackTutorial();
                TutorialManager.Instance.attackTutor = true;
            }
        }
    }
}
