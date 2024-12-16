using System.Collections.Generic;

public class ModelSaveData
{
    public string player;
    public string score;
    public string time;
    public string correctAnswer;
}

public class SaveDataWrapper
{
    public List<ModelSaveData> data; // Lista de los datos que deseas guardar
}