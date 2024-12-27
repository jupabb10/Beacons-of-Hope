using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreList : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<ModelSaveData> allScores = SaveManager.LoadPlayerData();

        foreach (ModelSaveData score in allScores)
        {
            Debug.Log($"Player: {score.player}, Score: {score.score}, Time: {score.time}, Correct Answer: {score.correctAnswer}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
