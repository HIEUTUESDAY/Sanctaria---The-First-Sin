using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    public bool healthPotionTutor = false;
    public bool attackTutor = false;
    public bool jumpTutor = false;
    public bool dashTutor = false;
    public bool wallClimbTutor = false;
    public bool checkpointTutor = false;
    public bool npcTutor = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public PlayerTutorialData GetTutorialData()
    {
        PlayerTutorialData playerTutorialData = new PlayerTutorialData(healthPotionTutor, attackTutor, jumpTutor, dashTutor, wallClimbTutor, checkpointTutor, npcTutor);
        return playerTutorialData;
    }
}
