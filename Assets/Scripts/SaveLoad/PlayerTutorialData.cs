using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerTutorialData
{
    public bool healthPotionTutor;
    public bool attackTutor;
    public bool jumpTutor;
    public bool dashTutor;
    public bool wallClimbTutor;
    public bool checkpointTutor;
    public bool NpcTutor;

    public PlayerTutorialData(bool healthPotionTutor, bool attackTutor, bool jumpTutor, bool dashTutor, bool wallClimbTutor, bool checkpointTutor, bool npcTutor)
    {
        this.healthPotionTutor = healthPotionTutor;
        this.attackTutor = attackTutor;
        this.jumpTutor = jumpTutor;
        this.dashTutor = dashTutor;
        this.wallClimbTutor = wallClimbTutor;
        this.checkpointTutor = checkpointTutor;
        this.NpcTutor = npcTutor;
    }
}
