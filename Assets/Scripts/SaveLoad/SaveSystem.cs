using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGame(Player player, Checkpoint checkpoint, int slotIndex)
    {
        string path = Application.persistentDataPath + "/savefile" + slotIndex + ".json";

        GameData data = new GameData();
        data.playerData = new PlayerData(player, checkpoint);
        data.checkpoint = checkpoint;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log("Game file saved at " + path);
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
