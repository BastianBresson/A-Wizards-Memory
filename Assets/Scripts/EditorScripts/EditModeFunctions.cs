using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class EditModeFunctions : EditorWindow
{
    private static string dataSubPath = "/AWizardsMemorySave.json";
    private static string dataPath;

    [MenuItem("Window/Edit Mode Functions")]
    public static void ShowWindow()
    {
        GetWindow<EditModeFunctions>("Edit Mode Functions");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Delete Save File"))
        {
            DeleteSaveFile();
        }
    }

    private void DeleteSaveFile()
    {
        File.Delete(dataPath);
        if (File.Exists(dataPath))
        {
            Debug.Log("Unsucsesful: save file still exists");
        }
        else
        {
            Debug.Log("Succesful: save file no longer exists");
        }
    }

    private void OnEnable()
    {
        dataPath = Application.persistentDataPath + dataSubPath;
    }
}
