using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject NPCLabel;

    [SerializeField] private bool canInteract;

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && canInteract && !UIManager.Instance.menuActivated)
        {
            Interact();
        }
    }

    public abstract void Interact();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) 
        {
            canInteract = true;
            NPCLabel.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
            NPCLabel.gameObject.SetActive(false);
        }
    }
}
