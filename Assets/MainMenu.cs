using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
