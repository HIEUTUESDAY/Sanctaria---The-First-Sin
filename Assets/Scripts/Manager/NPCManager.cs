using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [Header("This scene DATA")]
    [SerializeField] private SceneData thisSceneData;
    [Space(5)]

    [Header("Drop NPC in scene into this BOSS OBJECT")]
    [SerializeField] private GameObject npcObject;

    // Save the current state of enemies in the scene
    public NPCData SaveNPC()
    {
        NPCData npcData = new NPCData();

        if (npcObject != null)
        {
            ITalkable npc = npcObject.GetComponent<ITalkable>();

            if (npc != null)
            {
                npcData.npcName = npcObject.name;
                npcData.isFirstTalk = npc.IsFirstTalk;
                npcData.position = new float[] { npcObject.transform.position.x, npcObject.transform.position.y, npcObject.transform.position.z };
            }
        }

        return npcData;
    }

    // Load the enemies in the scene based on the saved data
    public void LoadNPC(string currentScene)
    {
        List<SceneData> sceneDataList = SceneDataManager.Instance.sceneDataList;

        foreach (SceneData sceneData in sceneDataList)
        {
            if (currentScene.Equals(sceneData.sceneName))
            {
                thisSceneData = sceneData;
                SetNPCStatus();
                return;
            }
        }
    }

    private void SetNPCStatus()
    {
        if (thisSceneData != null)
        {
            // Find the corresponding enemy prefab by name
            GameObject npc = GameObject.Find(thisSceneData.npc.npcName);

            if (npc != null)
            {
                ITalkable npcTalkable = npc.GetComponent<ITalkable>();

                if (npcTalkable != null)
                {
                    npcTalkable.IsFirstTalk = thisSceneData.npc.isFirstTalk;
                }
            }
        }
    }
}
