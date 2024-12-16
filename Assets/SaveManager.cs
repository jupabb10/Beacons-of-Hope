using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class SaveManager
{
    public static void SavePlayerData(ModelSaveData modelSaveData)
    {
        string filePath = Application.persistentDataPath + "/score.json";

        // Leer datos existentes
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

        // Asegurarse de que la lista esté inicializada
        if (wrapper.data == null)
        {
            wrapper.data = new List<ModelSaveData>();
        }

        // Agregar el nuevo dato
        wrapper.data.Add(modelSaveData);

        // Serializar y guardar
        string newJson = JsonUtility.ToJson(wrapper, true); // true para formatear el JSON
        File.WriteAllText(filePath, newJson);

        //Debug.Log("Datos guardados correctamente.");
    }

    public static List<ModelSaveData> LoadPlayerData()
    {
        string filePath = Application.persistentDataPath + "/score.json";

        if (File.Exists(filePath))
        {
            Debug.Log($"Archivo encontrado en: {filePath}");
            string json = File.ReadAllText(filePath);
            Debug.Log($"Contenido del archivo: {json}");

            SaveDataWrapper wrapper = JsonUtility.FromJson<SaveDataWrapper>(json);

            if (wrapper != null)
            {
                //Debug.Log($"Wrapper deserializado correctamente. Número de elementos en 'data': {wrapper.data?.Count ?? 0}");

                if (wrapper.data != null)
                {
                    return wrapper.data; // Retorna la lista de datos
                }
            }
            else
            {
                //Debug.LogWarning("El wrapper deserializado es nulo.");
            }
        }
        else
        {
            Debug.LogWarning("Archivo no encontrado.");
        }

        return new List<ModelSaveData>(); // Retorna lista vacía si no hay datos
    }
}
