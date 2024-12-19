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
    private object correctAnswer;
    public int totalAnswerCorrect = 0;
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

        List<ModelSaveData> allScores = SaveManager.LoadPlayerData();

        foreach (ModelSaveData score in allScores)
        {
            Debug.Log($"Player: {score.player}, Score: {score.score}, Time: {score.time}, Correct Answer: {score.correctAnswer}");
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
            timer = setTimer;
            Invoke(nameof(ValidatePlayerPosition), 0.1f);
            Invoke(nameof(GenerateNewProblem), 0.1f);
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
            ModelSaveData gameData = new ModelSaveData{
                player = "Player",
                score = $"{score:D7}",
                time = "01:00",
                correctAnswer= totalAnswerCorrect.ToString()
            };

            SaveManager.SavePlayerData(gameData);
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
                setTimer = 25;
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

    //float ConvertToDecimal(int correctAnswer)
    //{
    //    return correctAnswer + UnityEngine.Random.Range(-0.5f, 0.5f); // Agrega un pequeño desplazamiento decimal
    //}

    //string ConvertToFraction(int correctAnswer)
    //{
    //    int numerator = correctAnswer * UnityEngine.Random.Range(2, 5); // Escalar el numerador
    //    int denominator = UnityEngine.Random.Range(2, 5); // Escalar el denominador

    //    // Reducir fracción si es posible
    //    int gcd = GCD(numerator, denominator);
    //    numerator /= gcd;
    //    denominator /= gcd;

    //    return $"{numerator}/{denominator}";
    //}

    int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
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
        float num1 = UnityEngine.Random.Range(1f, 10f);
        float num2 = UnityEngine.Random.Range(1f, 10f);
        char[] operations = { '+', '-', '*' };
        char operation = operations[UnityEngine.Random.Range(0, operations.Length)];

        switch (operation)
        {
            case '+':
                correctAnswer = MathF.Round(num1 + num2, 2); // Redondear a 2 decimales
                break;
            case '-':
                correctAnswer = MathF.Round(num1 - num2, 2); // Redondear a 2 decimales
                break;
            case '*':
                correctAnswer = MathF.Round(num1 * num2, 2); // Redondear a 2 decimales
                break;
        }

        DisplayProblem($"{num1:F2} {operation} {num2:F2}");
    }

    void GenerateFractionProblem()
    {
        int numerator1 = UnityEngine.Random.Range(1, 11);
        int denominator1 = UnityEngine.Random.Range(1, 11);
        int numerator2 = UnityEngine.Random.Range(1, 11);
        int denominator2 = UnityEngine.Random.Range(1, 11);

        char[] operations = { '+', '-', '*' };
        char operation = operations[UnityEngine.Random.Range(0, operations.Length)];

        correctAnswer = ConvertToFractionResult(numerator1, denominator1, numerator2, denominator2, operation);
        string fraction1 = $"{numerator1}/{denominator1}";
        string fraction2 = $"{numerator2}/{denominator2}";

        DisplayProblem($"{fraction1} {operation} {fraction2}");
    }

    string ConvertToFractionResult(int numerator1, int denominator1, int numerator2, int denominator2, char operation)
    {
        int resultNumerator, resultDenominator;

        switch (operation)
        {
            case '+':
                resultNumerator = (numerator1 * denominator2) + (numerator2 * denominator1);
                resultDenominator = denominator1 * denominator2;
                break;
            case '-':
                resultNumerator = (numerator1 * denominator2) - (numerator2 * denominator1);
                resultDenominator = denominator1 * denominator2;
                break;
            case '*':
                resultNumerator = numerator1 * numerator2;
                resultDenominator = denominator1 * denominator2;
                break;
            default:
                throw new System.Exception("Operación no válida");
        }

        int gcd = GCD(resultNumerator, resultDenominator);
        resultNumerator /= gcd;
        resultDenominator /= gcd;

        return $"{resultNumerator}/{resultDenominator}";
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
        if (level == 1)
        {
            AssignAnswers(correctAnswer);
        }
        else if (level == 2)
        {
            AssignAnswers((float)correctAnswer);
        }
        else if (level == 3 || level == 4)
        {
            AssignAnswers(correctAnswer.ToString());
        }
    }

    void AssignAnswers(object correctAnswer)
    {
        string[] answers = new string[3];
        correctAnswerIndex = UnityEngine.Random.Range(0, 3);
        answers[correctAnswerIndex] = correctAnswer.ToString();

        for (int i = 0; i < 3; i++)
        {
            if (i != correctAnswerIndex)
            {
                string incorrectAnswer;
                do
                {
                    if (level == 1)
                    {
                        int deviation = UnityEngine.Random.Range(-9, 10);
                        incorrectAnswer = (Convert.ToInt32(correctAnswer) + deviation).ToString();
                    }
                    else if (level == 2)
                    {
                        float deviation = UnityEngine.Random.Range(-1f, 1f);
                        float generatedAnswer = Convert.ToSingle(correctAnswer) + deviation;
                        incorrectAnswer = MathF.Round(generatedAnswer, 2).ToString("F2"); // Redondear y formatear a 2 decimales
                    }
                    else if (level == 3)
                    {
                        incorrectAnswer = GenerateRandomFraction();
                    }
                    else
                    {
                        throw new System.Exception("Tipo de respuesta desconocido");
                    }
                } while (incorrectAnswer == correctAnswer.ToString() || System.Array.Exists(answers, x => x == incorrectAnswer));

                answers[i] = incorrectAnswer;
            }
        }

        Answer1Text.text = answers[0];
        Answer2Text.text = answers[1];
        Answer3Text.text = answers[2];
    }

    string GenerateRandomFraction()
    {
        int numerator = UnityEngine.Random.Range(1, 11);
        int denominator;
        do
        {
            denominator = UnityEngine.Random.Range(1, 11);
        } while (denominator == 0); // Asegurarse de que el denominador no sea 0

        return $"{numerator}/{denominator}";
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
