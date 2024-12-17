using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class SaveManager
{
    public static void SavePlayerData(ModelSaveData modelSaveData)
    {
        string filePath = Application.persistentDataPath + "/score.json";

        SaveDataWrapper wrapper;

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            wrapper = JsonUtility.FromJson<SaveDataWrapper>(json) ?? new SaveDataWrapper();
        }
        else
        {
            wrapper = new SaveDataWrapper();
        }
        if (wrapper.data == null)
        {
            wrapper.data = new List<ModelSaveData>();
        }
        wrapper.data.Add(modelSaveData);
        string newJson = JsonUtility.ToJson(wrapper);
        File.WriteAllText(filePath, newJson);

        //Debug.Log("Datos guardados correctamente.");
    }

    public static List<ModelSaveData> LoadPlayerData()
    {
        string filePath = Application.persistentDataPath + "/score.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            SaveDataWrapper wrapper = JsonUtility.FromJson<SaveDataWrapper>(json);

            if (wrapper != null)
            {
                if (wrapper.data != null)
                {
                    return wrapper.data;
                }
            }
        }
        else
        {
            Debug.LogWarning("Archivo no encontrado.");
        }

        return new List<ModelSaveData>();
    }
}
