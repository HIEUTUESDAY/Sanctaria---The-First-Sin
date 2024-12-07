using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTutorial : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!TutorialManager.Instance.inventoryTutor)
            {
                Player.Instance.InventoryTutorial();
                TutorialManager.Instance.inventoryTutor = true;
            }
        }
    }
}
