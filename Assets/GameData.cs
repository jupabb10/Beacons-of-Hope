[System.Serializable]
public class GameData
{
    public int score;

    public GameData(MathProblemGenerator mathProblemGenerator)
    {
        score = mathProblemGenerator.score;
    }
}
