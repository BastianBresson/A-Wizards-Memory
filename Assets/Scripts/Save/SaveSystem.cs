using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string dataSubPath = "/AWizardsMemorySave.json";
    private static string dataPath = Application.persistentDataPath + dataSubPath;

    public static bool IsLevelCompleted(uint id)
    {
        SaveData data = Load();

        return data.completedLevels.Contains(id);
    }


    public static void CompletedNewLevel(uint newLevelId)
    {
        SaveData data = Load();

        if (!data.completedLevels.Contains(newLevelId))
        {
            data.completedLevels.Add(newLevelId);
            Save(data);
        }
    }


    private static void Save(SaveData data)
    {
        string outputString = JsonUtility.ToJson(data);
        File.WriteAllText(dataPath, outputString);
    }

    private static SaveData Load()
    {
        if (File.Exists(dataPath))
        {
            string inputString = File.ReadAllText(dataPath);
            SaveData data = JsonUtility.FromJson<SaveData>(inputString);
            return data;
        }
        else
        {
            SaveData data = new SaveData(new List<uint>());
            Save(data);
            return data;
        }
    }
    
}

[System.Serializable]
public class SaveData
{
    public List<uint> completedLevels;


    public SaveData(List<uint> completedLevels)
    {
        this.completedLevels = completedLevels;
    }
}
