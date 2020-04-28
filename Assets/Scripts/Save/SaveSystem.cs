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

        bool levelHasBeenCompleted = data.completedLevels.Contains(id);

        return levelHasBeenCompleted;
    }


    public static void CompletedNewLevel(uint newLevelId)
    {
        SaveData data = Load();

        bool levelAlreadyExistsInData = data.completedLevels.Contains(newLevelId);

        if (!levelAlreadyExistsInData)
        {
            data.completedLevels.Add(newLevelId);
            Save(data);
        }
    }


    private static void Save(SaveData data)
    {
        SaveDataToJson(data);
    }


    private static SaveData Load()
    {
        if (File.Exists(dataPath))
        {
            SaveData data = RetrieveDataFromJson();
            return data;
        }
        else
        {
            SaveData data = SetupSaveData();
            return data;
        }
    }


    private static void SaveDataToJson(SaveData data)
    {
        string outputString = JsonUtility.ToJson(data);
        File.WriteAllText(dataPath, outputString);
    }


    private static SaveData  RetrieveDataFromJson()
    {
        string inputString = File.ReadAllText(dataPath);
        SaveData data = JsonUtility.FromJson<SaveData>(inputString);
        return data;
    }


    private static SaveData SetupSaveData()
    {
        SaveData data = new SaveData(new List<uint>());
        Save(data);
        return data;
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
