using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MathProblemGenerator : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI problemText; // Campo para mostrar el problema
    public LivesManager livesManager;

    public TextMeshProUGUI levelText;

    public TextMeshProUGUI Answer1Text; // Campo para mostrar el problema
    public TextMeshProUGUI Answer2Text; // Campo para mostrar el problema
    public TextMeshProUGUI Answer3Text; // Campo para mostrar el problema

    public TextMeshProUGUI ScoreText; // Campo para mostrar el problema

    public TextMeshProUGUI timerText; // Campo para mostrar el problema
    public int score = 0;
    public int lives = 3;
    public int timer = 25;
    public int setTimer = 25;

    private int correctAnswerIndex;
    private int correctAnswer;
    public int correctAnswersCount = 0;
    public int level = 1; // Nivel inicial
    private int maxLevel = 4;

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
        //InvokeRepeating("GenerateNewProblem", 0f, 3f);
        //InvokeRepeating("ValidatePlayerPosition", 3f, 3f);
        GenerateNewProblem();
        InvokeRepeating("changeTimer", 1f, 1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateNewProblem();
            ValidatePlayerPosition();
            timer = setTimer;
        }
    }

    void changeTimer()
    {
        timer = timer - 1;
        timerText.text = timer.ToString();

        if(timer == 0)
        {
            GenerateNewProblem();
            ValidatePlayerPosition();
            timer = setTimer;
        }
    }

    public void LoseLife()
    {
        lives -= 1;
        livesManager.UpdateLives(lives);

        if (lives <= 0)
        {
            livesManager.GameOver();
            SaveManager.SavePlayerData(this);
            Debug.Log("Datos guardados");
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
        switch (level)
        {
            case 1:
                GenerateBasicProblem();
                setTimer = 25 ;
                break;
            case 2:
                GenerateDecimalProblem();
                setTimer = 20;
                break;
            case 3:
                GenerateFractionProblem();
                setTimer = 15;
                break;
            default:
                GenerateMixedProblem();
                setTimer = 10;
                break;
        }
    }
    void GenerateBasicProblem()
    {
        int num1 = UnityEngine.Random.Range(1, 11);
        int num2 = UnityEngine.Random.Range(1, 11);
        char[] operations = { '+', '-', '*', '/' };
        char operation = operations[UnityEngine.Random.Range(0, operations.Length)];

        correctAnswer = SolveProblem(num1, num2, operation);
        DisplayProblem($"{num1} {operation} {num2}");
    }

    void GenerateDecimalProblem()
    {
        float num1 = (float)Math.Round(UnityEngine.Random.Range(1f, 10f), 1);
        float num2 = (float)Math.Round(UnityEngine.Random.Range(1f, 10f), 1);
        char[] operations = { '+', '-', '*' };
        char operation = operations[UnityEngine.Random.Range(0, operations.Length)];

        correctAnswer = (int)SolveProblem(num1, num2, operation);
        DisplayProblem($"{num1} {operation} {num2}");
    }

    void GenerateFractionProblem()
    {
        int numerator1 = UnityEngine.Random.Range(1, 11);
        int denominator1 = UnityEngine.Random.Range(1, 11);
        int numerator2 = UnityEngine.Random.Range(1, 11);
        int denominator2 = UnityEngine.Random.Range(1, 11);

        string fraction1 = $"{numerator1}/{denominator1}";
        string fraction2 = $"{numerator2}/{denominator2}";
        char[] operations = { '+', '-', '*' };
        char operation = operations[UnityEngine.Random.Range(0, operations.Length)];

        correctAnswer = SolveFractionProblem(numerator1, denominator1, numerator2, denominator2, operation);
        DisplayProblem($"{fraction1} {operation} {fraction2}");
    }

    void GenerateMixedProblem()
    {
        int type = UnityEngine.Random.Range(1, 4);
        if (type == 1)
            GenerateBasicProblem();
        else if (type == 2)
            GenerateDecimalProblem();
        else
            GenerateFractionProblem();
    }

    int SolveProblem(float num1, float num2, char operation)
    {
        switch (operation)
        {
            case '+': return (int)(num1 + num2);
            case '-': return (int)(num1 - num2);
            case '*': return (int)(num1 * num2);
            case '/': return (int)(num1 / num2);
            default: return 0;
        }
    }

    int SolveFractionProblem(int num1, int den1, int num2, int den2, char operation)
    {
        int result = 0;
        switch (operation)
        {
            case '+':
                result = num1 * den2 + num2 * den1;
                break;
            case '-':
                result = num1 * den2 - num2 * den1;
                break;
            case '*':
                result = num1 * num2;
                break;
        }
        return result;
    }

    void DisplayProblem(string problem)
    {
        problemText.text = problem;
        AssignAnswers(correctAnswer);
    }

    void AssignAnswers(int correctAnswer)
    {
        int[] answers = new int[3];
        correctAnswerIndex = UnityEngine.Random.Range(0, 3); // Actualiza el índice correcto
        answers[correctAnswerIndex] = correctAnswer;

        for (int i = 0; i < 3; i++)
        {
            if (i != correctAnswerIndex)
            {
                answers[i] = UnityEngine.Random.Range(1, 101); // Genera respuestas incorrectas
            }
        }

        Answer1Text.text = answers[0].ToString();
        Answer2Text.text = answers[1].ToString();
        Answer3Text.text = answers[2].ToString();
    }

    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }

    public void LevelUp()
    {
        level++;
        correctAnswersCount = 0;
        levelText.text = $"Nivel {level}";
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
