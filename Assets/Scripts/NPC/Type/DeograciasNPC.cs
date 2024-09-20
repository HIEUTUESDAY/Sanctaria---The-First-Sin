using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeograciasNPC : NPC, ITalkable
{
    [SerializeField] private DialogueTextSO dialogueText;

    private DialogueManager dialogueManager;

    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public override void Interact()
    {
        Talk(dialogueText);
    }

    public void Talk(DialogueTextSO dialogueText)
    {
        dialogueManager.DisplayNextMessage(dialogueText);
    }
}
