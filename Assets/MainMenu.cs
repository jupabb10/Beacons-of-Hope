using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSceneMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ChangeSceneGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ChangeSceneScore()
    {
        SceneManager.LoadScene("ScoreScene");
    }

    public void ChangeSceneOptions()
    {
        SceneManager.LoadScene("OptionScene");
    }
}
