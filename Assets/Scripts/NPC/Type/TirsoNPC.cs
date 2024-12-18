using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirsoNPC : NPC, ITalkable
{
    [SerializeField] private DialogueTextSO firstTalkDialogue;
    [SerializeField] private DialogueTextSO secondTalkDialogue;
    private DialogueManager dialogueManager;
    [field: SerializeField] public bool IsFirstTalk { get; set; } = true;

    public override void Interact()
    {
        if (IsFirstTalk)
        {
            Talk(firstTalkDialogue);
            IsFirstTalk = false;
        }
        else
        {
            Talk(secondTalkDialogue);
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
