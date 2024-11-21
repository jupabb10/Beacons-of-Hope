using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LivesManager : MonoBehaviour
{
    public Image[] heartImages; // Arreglo de imágenes de corazones
    public Sprite fullHeartSprite; // Sprite del corazón lleno
    public Sprite emptyHeartSprite; // Sprite del corazón vacío
    public GameObject panel;

    private int currentLives;

    public void InitializeLives(int lives)
    {
        currentLives = lives;
        UpdateHearts();
    }


    // Actualiza el estado de las imágenes según las vidas actuales
    public void UpdateLives(int lives)
    {
        currentLives = lives;
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentLives)
            {
                heartImages[i].sprite = fullHeartSprite; // Corazón lleno
            }
            else
            {
                heartImages[i].sprite = emptyHeartSprite; // Corazón vacío
            }
        }
    }

    public void GameOver ()
    {
        panel.SetActive(true);
        SoundManager.Instance.StopBackgroundSound();
        SoundManager.Instance.PlayEffectSound(SoundManager.Instance.gameOverSound);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
