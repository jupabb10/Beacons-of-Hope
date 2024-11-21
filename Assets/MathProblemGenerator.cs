using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MathProblemGenerator : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI problemText; // Campo para mostrar el problema
    public LivesManager livesManager;

    public TextMeshProUGUI Answer1Text; // Campo para mostrar el problema
    public TextMeshProUGUI Answer2Text; // Campo para mostrar el problema
    public TextMeshProUGUI Answer3Text; // Campo para mostrar el problema

    public TextMeshProUGUI ScoreText; // Campo para mostrar el problema
    private int score = 0;
    public int lives = 3;

    private int correctAnswerIndex;
    private int correctAnswer;
    private int Answer1 = 1;
    private int Answer2 = 2;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBackgroundSound(SoundManager.Instance.mainMenuSound, true);
        livesManager.InitializeLives(lives);
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        // Configura las invocaciones repetitivas
        InvokeRepeating("GenerateNewProblem", 0f, 3f);
        InvokeRepeating("ValidatePlayerPosition", 3f, 3f);
    }

    public void LoseLife()
    {
        lives -= 1;
        livesManager.UpdateLives(lives);

        if (lives <= 0)
        {
            livesManager.GameOver();
            CancelInvoke(); // Detiene todas las invocaciones repetitivas en este script
            HideTexts();
            player.StopPlayer();
        }
    }

    private void HideTexts()
    {
        // Desactiva los textos relacionados con los problemas
        problemText.gameObject.SetActive(false);
        Answer1Text.gameObject.SetActive(false);
        Answer2Text.gameObject.SetActive(false);
        Answer3Text.gameObject.SetActive(false);
    }

    public void GenerateNewProblem()
    {
        int num1 = Random.Range(1, 11);
        int num2 = Random.Range(1, 11);
        int operation = Random.Range(0, 4);
        string operatorSymbol = "";

        // Calcula la respuesta correcta y construye el problema
        switch (operation)
        {
            case 0: // Suma
                correctAnswer = num1 + num2;
                operatorSymbol = "+";
                break;
            case 1: // Resta
                correctAnswer = num1 - num2;
                operatorSymbol = "-";
                break;
            case 2: // Multiplicación
                correctAnswer = num1 * num2;
                operatorSymbol = "×";
                break;
            case 3: // División
                // Asegúrate de que el segundo número sea un divisor del primero para obtener un entero
                while (num1 % num2 != 0)
                {
                    num2 = Random.Range(1, 11);
                }
                correctAnswer = num1 / num2;
                operatorSymbol = "÷";
                break;
        }

        problemText.text = $"{num1} {operatorSymbol} {num2} = ?";

        Answer1 = Random.Range(1, 100);
        Answer2 = Random.Range(1, 100);

        while (Answer1 == correctAnswer)
        {
            Answer1 = Random.Range(1, 100);
        }

        while (Answer2 == correctAnswer || Answer2 == Answer1)
        {
            Answer2 = Random.Range(1, 100);
        }

        // Decide aleatoriamente dónde colocar la respuesta correcta
        correctAnswerIndex = Random.Range(0, 3);

        if (correctAnswerIndex == 0)
        {
            Answer1Text.text = correctAnswer.ToString();
            Answer2Text.text = Answer1.ToString();
            Answer3Text.text = Answer2.ToString();
        }
        else if (correctAnswerIndex == 1)
        {
            Answer1Text.text = Answer1.ToString();
            Answer2Text.text = correctAnswer.ToString();
            Answer3Text.text = Answer2.ToString();
        }
        else
        {
            Answer1Text.text = Answer1.ToString();
            Answer2Text.text = Answer2.ToString();
            Answer3Text.text = correctAnswer.ToString();
        }
        

    }
    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }

    void ValidatePlayerPosition()
    {
        // Busca el objeto del jugador y llama a su función de validación
        player.ValidatePosition();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        // Formatear el texto con "Score" y un número de 7 dígitos
        ScoreText.text = $"Score {score:D7}";
    }
}
