using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITalkable
{
    bool IsFirstTalk { get; set; }
    public void Talk(DialogueTextSO dialogueText);
}
