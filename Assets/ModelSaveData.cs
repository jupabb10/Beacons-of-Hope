using System.Collections.Generic;
[System.Serializable]
public class ModelSaveData
{
    public string player;
    public string score;
    public string time;
    public string correctAnswer;
}
[System.Serializable]
public class SaveDataWrapper
{
    public List<ModelSaveData> data; // Lista de los datos que deseas guardar
}