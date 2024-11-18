using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueMessageObject;
    [SerializeField] private TextMeshProUGUI NPCNameText;
    [SerializeField] private TextMeshProUGUI NPCMessageText;
    [SerializeField] private float typingSpeed = 5f;

    private Queue<string> messages = new Queue<string>();
    private string message;
    private bool conversationEnded;
    private bool isTyping;

    private Coroutine TypingMessageCoroutine;

    private const string TYPING_ALPHA = "<color=#00000000>";
    private const float TYPING_TIME = 0.1f;

    public void DisplayNextMessage(DialogueTextSO dialogueText)
    {
        // if there nothing in the Queue
        if(messages.Count == 0) 
        {
            if (!conversationEnded)
            {
                StartConversation(dialogueText);
            }
            else if (conversationEnded && !isTyping)
            {
                EndConversation();
                return;
            }
        }

        // if there is something in the Queue
        if (!isTyping)
        {
            message = messages.Dequeue();

            TypingMessageCoroutine = StartCoroutine(TypingMessage(message));
        }
        else
        {
            FinishCoversationEarly();
        }

        if (messages.Count == 0)
        {
            conversationEnded = true;
        }
    }

    private void StartConversation(DialogueTextSO dialogueText)
    {
        if(!dialogueMessageObject.activeSelf)
        {
            dialogueMessageObject.SetActive(true);
        }

        NPCNameText.text = dialogueText.speakerName;

        for(int i = 0; i < dialogueText.messages.Length; i++)
        {
            messages.Enqueue(dialogueText.messages[i]);
        }

        SoundFXManager.Instance.PlayEquipItemSound();
    }

    private void EndConversation()
    {
        conversationEnded = true;

        if (dialogueMessageObject.activeSelf)
        {
            dialogueMessageObject.SetActive(false);
        }

        conversationEnded = false;
        Player.Instance.isInteractWithNPC = false;
        SoundFXManager.Instance.PlayEquipItemSound();
    }

    private void FinishCoversationEarly()
    {
        StopCoroutine(TypingMessageCoroutine);
        NPCMessageText.text = message;
        isTyping = false;
        SoundFXManager.Instance.PlayChangeTabSound();
    }

    private IEnumerator TypingMessage(string message)
    {
        isTyping = true;

        NPCMessageText.text = "";

        string originalMessage = message;
        string displayedMessage = "";
        int alphaIndex = 0;

        foreach (var m in message.ToCharArray())
        {
            alphaIndex++;
            NPCMessageText.text = originalMessage;

            displayedMessage = NPCMessageText.text.Insert(alphaIndex, TYPING_ALPHA);
            NPCMessageText.text = displayedMessage;

            yield return new WaitForSeconds(TYPING_TIME / typingSpeed);
        }

        isTyping = false;
    }
}
