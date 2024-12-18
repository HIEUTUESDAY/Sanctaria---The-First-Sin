using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGame(PlayerData playerData, PlayerTutorialData playerTutorialData, PlayerCheckpointData playerCheckpointData, PlayerInventoryData playerInventoryData, PlayerMapData playerMapData, PlayerSceneData playerSceneData, int slotIndex)
    {
        string path = Application.persistentDataPath + "/savefile" + slotIndex + ".json";

        GameData data = new GameData
        {
            playerData = playerData,
            playerTutorialData = playerTutorialData,
            playerCheckpointData = playerCheckpointData,
            playerInventoryData = playerInventoryData,
            playerMapData = playerMapData,
            playerSceneData = playerSceneData
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log("Game file saved");
    }

    public static GameData LoadGame(int slotIndex)
    {
        string path = Application.persistentDataPath + "/savefile" + slotIndex + ".json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
