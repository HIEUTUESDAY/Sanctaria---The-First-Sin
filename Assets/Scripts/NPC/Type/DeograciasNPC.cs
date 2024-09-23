using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeograciasNPC : NPC, ITalkable
{
    [SerializeField] private DialogueTextSO normalDialogueText;
    [SerializeField] private DialogueTextSO hintDialogueText;
    private DialogueManager dialogueManager;
    [SerializeField] private bool talkSecondTime = false;

    public override void Interact()
    {
        if (!talkSecondTime)
        {
            Talk(normalDialogueText);
            talkSecondTime = true;
        }
        else
        {
            Talk(hintDialogueText);
        }
    }

    public void Talk(DialogueTextSO dialogueText)
    {
        dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager != null)
        {
            dialogueManager.DisplayNextMessage(dialogueText);

        }
    }
}
