using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTutorial : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!TutorialManager.Instance.checkpointTutor)
            {
                Player.Instance.CheckpointTutorial();
                TutorialManager.Instance.checkpointTutor = true;
            }
        }
    }
}
