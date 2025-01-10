using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonAnimator : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] toggleButtons;
    [SerializeField] private GameObject[] toggleBurbujas;
    [SerializeField] private TextMeshProUGUI[] toggleTexts;

    [SerializeField] private Vector3[] initialPositions;
    [SerializeField] private Vector3[] finalPositions;

    [SerializeField] private Vector3[] initialScales;
    [SerializeField] private Vector3[] finalScales;

    [SerializeField] private float delayBeforeAnimation = 1f; // Tiempo antes de empezar la animación
    [SerializeField] private float animationDuration = 1f; // Duración de la animación

    private void Start()
    {
        // Desactiva los botones inicialmente
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }

        foreach (GameObject toggleButton in toggleButtons)
        {
            SpriteRenderer sr = toggleButton.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = true;
            }
        }

        foreach (GameObject toggleBurbuja in toggleBurbujas)
        {
            SpriteRenderer sr = toggleBurbuja.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = true;
            }
        }

        foreach (TextMeshProUGUI text in toggleTexts)
        {
            if (text != null)
            {
                text.enabled = true;
            }
        }
    }

    public void ActivateAnimation()
    {
        StartCoroutine(AnimateButtons());
    }

    private IEnumerator AnimateButtons()
    {

        foreach (GameObject toggleButton in toggleButtons)
        {
            SpriteRenderer sr = toggleButton.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = false;
            }
        }

        // Activa los botones primero
        foreach (GameObject button in buttons)
        {
            button.SetActive(true);
        }

        // Espera antes de iniciar la animación
        yield return new WaitForSeconds(delayBeforeAnimation);

        foreach (GameObject toggleBurbuja in toggleBurbujas)
        {
            SpriteRenderer sr = toggleBurbuja.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = false;
            }
        }

        foreach (TextMeshProUGUI text in toggleTexts)
        {
            Debug.Log(text);
            if (text != null)
            {
                text.enabled = false;
            }
        }

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (i < initialPositions.Length && i < finalPositions.Length)
                {
                    // Interpolación de posición y escala
                    buttons[i].transform.localPosition = Vector3.Lerp(initialPositions[i], finalPositions[i], t);
                    buttons[i].transform.localScale = Vector3.Lerp(initialScales[i], finalScales[i], t);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegúrate de que terminen exactamente en las posiciones y escalas finales
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < finalPositions.Length && i < finalScales.Length)
            {
                buttons[i].transform.localPosition = finalPositions[i];
                buttons[i].transform.localScale = finalScales[i];
            }
        }

        foreach (GameObject toggleButton in toggleButtons)
        {
            SpriteRenderer sr = toggleButton.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = true;
            }
        }

        foreach (GameObject toggleBurbuja in toggleBurbujas)
        {
            SpriteRenderer sr = toggleBurbuja.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = true;
            }
        }

        foreach (TextMeshProUGUI text in toggleTexts)
        {
            if (text != null)
            {
                text.enabled = true;
            }
        }
    }
}
