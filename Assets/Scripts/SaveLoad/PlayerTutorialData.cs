using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerTutorialData
{
    public bool enterPlayerTutor;
    public bool healthPotionTutor;
    public bool attackTutor;
    public bool jumpTutor;
    public bool dashTutor;
    public bool wallClimbTutor;
    public bool checkpointTutor;
    public bool inventoryTutor;
    public bool mapTutor;
    public bool heartTutor;
    public bool prayerTutor;
    public bool ladderTutor;

    public PlayerTutorialData(bool enterPlayerTutor, bool healthPotionTutor, bool attackTutor, bool jumpTutor, bool dashTutor, bool wallClimbTutor, bool checkpointTutor, bool inventoryTutor, bool mapTutor, bool heartTutor, bool prayerTutor, bool ladderTutor)
    {
        this.enterPlayerTutor = enterPlayerTutor;
        this.healthPotionTutor = healthPotionTutor;
        this.attackTutor = attackTutor;
        this.jumpTutor = jumpTutor;
        this.dashTutor = dashTutor;
        this.wallClimbTutor = wallClimbTutor;
        this.checkpointTutor = checkpointTutor;
        this.inventoryTutor = inventoryTutor;
        this.mapTutor = mapTutor;
        this.heartTutor = heartTutor;
        this.prayerTutor = prayerTutor;
        this.ladderTutor = ladderTutor;
    }
}
