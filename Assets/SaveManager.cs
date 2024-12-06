using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static void SavePlayerData(MathProblemGenerator mathProblemGenerator)
    {
        GameData gameData = new GameData(mathProblemGenerator);
        string dataPath = Application.persistentDataPath + "/score.save";
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, gameData);
        fileStream.Close();
    }

    public static GameData loadGameData()
    {
        string dataPath = Application.persistentDataPath + "/score.save";

        if(File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            GameData gameData = (GameData)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return gameData;
        }
        else
        {
            Debug.Log("No se encontro ningun archivo de guardado");
            return null;
        }
    }
}
