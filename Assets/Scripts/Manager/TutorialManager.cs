using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    public bool enterPlayerTutor = false;
    public bool healthPotionTutor = false;
    public bool attackTutor = false;
    public bool jumpTutor = false;
    public bool dashTutor = false;
    public bool wallClimbTutor = false;
    public bool checkpointTutor = false;
    public bool inventoryTutor = false;
    public bool mapTutor = false;
    public bool prayerTutor = false;
    public bool heartTutor = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void LoadTutorialData(PlayerTutorialData playerTutorialData)
    {
        enterPlayerTutor = playerTutorialData.enterPlayerTutor;
        healthPotionTutor = playerTutorialData.healthPotionTutor;
        attackTutor = playerTutorialData.attackTutor;
        jumpTutor = playerTutorialData.jumpTutor;
        dashTutor = playerTutorialData.dashTutor;
        wallClimbTutor = playerTutorialData.wallClimbTutor;
        checkpointTutor = playerTutorialData.checkpointTutor;
        inventoryTutor = playerTutorialData.inventoryTutor;
        mapTutor = playerTutorialData.mapTutor;
        prayerTutor = playerTutorialData.prayerTutor;
        heartTutor = playerTutorialData.heartTutor;
    }

    public void ResetTutorialData()
    {
        enterPlayerTutor = false;
        healthPotionTutor = false;
        attackTutor = false;
        jumpTutor = false;
        dashTutor = false;
        wallClimbTutor = false;
        checkpointTutor = false;
        inventoryTutor = false;
        mapTutor = false;
        prayerTutor = false;
        heartTutor = false;
    }

    public PlayerTutorialData GetTutorialData()
    {
        PlayerTutorialData playerTutorialData = new PlayerTutorialData(enterPlayerTutor, healthPotionTutor, attackTutor, jumpTutor, dashTutor, wallClimbTutor, checkpointTutor, inventoryTutor, mapTutor, prayerTutor, heartTutor);
        return playerTutorialData;
    }
}
