using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTutorial : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!TutorialManager.Instance.mapTutor)
            {
                Player.Instance.MapTutorial();
                TutorialManager.Instance.mapTutor = true;
            }
        }
    }
}
