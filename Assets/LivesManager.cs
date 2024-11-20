using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public Image[] heartImages; // Arreglo de im�genes de corazones
    public Sprite fullHeartSprite; // Sprite del coraz�n lleno
    public Sprite emptyHeartSprite; // Sprite del coraz�n vac�o

    private int currentLives;

    public void InitializeLives(int lives)
    {
        currentLives = lives;
        UpdateHearts();
    }


    // Actualiza el estado de las im�genes seg�n las vidas actuales
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
                heartImages[i].sprite = fullHeartSprite; // Coraz�n lleno
            }
            else
            {
                heartImages[i].sprite = emptyHeartSprite; // Coraz�n vac�o
            }
        }
    }
}
