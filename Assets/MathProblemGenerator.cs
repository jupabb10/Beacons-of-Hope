using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;
using System.ComponentModel;

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
    
    public TextMeshProUGUI extraLiveText;
    public int score = 0;
    public int lives = 3;
    public int timer = 25;
    public int timer2 = 25;
    public int setTimer = 25;

    private int correctAnswerIndex;
    private object correctAnswer;
    public int totalAnswerCorrect = 0;
    public int incorrectAnswersCount = 0;
    public float gameTime = 0f;
    public int correctAnswersCount = 0;
    public int level = 1; // Nivel inicial
    private int maxLevel = 4;

    private int Answer1 = 1;
    private int Answer2 = 2;

    private string formattedTime; // Variable para guardar el tiempo formateado
    private bool isGameOver = false; // Bandera para controlar si el juego ha terminado

    public ButtonAnimator buttonAnimator;

    // Start is called before the first frame update
    void Start()
    {
        if (problemText != null)
        {
            // Aplica el color y grosor del borde
            problemText.fontMaterial.SetColor("_OutlineColor", Color.red);
            problemText.fontMaterial.SetFloat("_OutlineWidth", 0.3f);

            levelText.fontMaterial.SetColor("_OutlineColor", Color.black);
            levelText.fontMaterial.SetFloat("_OutlineWidth", 0.3f);
            timerText.fontMaterial.SetColor("_OutlineColor", Color.black);
            timerText.fontMaterial.SetFloat("_OutlineWidth", 0.3f);
            extraLiveText.fontMaterial.SetColor("_OutlineColor", Color.black);
            extraLiveText.fontMaterial.SetFloat("_OutlineWidth", 0.3f);

            ScoreText.fontMaterial.SetColor("_OutlineColor", Color.black);
            ScoreText.fontMaterial.SetFloat("_OutlineWidth", 0.3f);


            
        }

        SoundManager.Instance.PlayBackgroundSound(SoundManager.Instance.mainMenuSound, true);
        livesManager.InitializeLives(lives);
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        isGameOver = false;
        GenerateNewProblem();
        InvokeRepeating("changeTimer", 1f, 1f);
    }

    private void Update()
    {
    if (!isGameOver) // Solo incrementar el tiempo si el juego no ha terminado
    {
        gameTime += Time.deltaTime;

        // Calcular minutos, segundos y milisegundos
        int minutes = Mathf.FloorToInt(gameTime / 60); // Minutos
        int seconds = Mathf.FloorToInt(gameTime % 60); // Segundos
        int milliseconds = Mathf.FloorToInt((gameTime % 1) * 1000); // Milisegundos

        // Guardar el tiempo formateado en la variable
        formattedTime = $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
    }


        if (Input.GetKeyDown(KeyCode.G))
        {
            Invoke(nameof(ValidatePlayerPosition), 0.1f);
            Invoke(nameof(GenerateNewProblem), 0.1f);
            timer = setTimer;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
        level = 1;
        GenerateNewProblem();
        LevelUp();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
        level = 2;
        GenerateNewProblem();
        LevelUp();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
        level = 3;
        GenerateNewProblem();
        LevelUp();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
        level = 4;
        GenerateNewProblem();
        LevelUp();
        }

        
    }

    public string GetFormattedTime()
{
    return formattedTime;
}

    void changeTimer()
    {
        timer = Mathf.Max(0, timer - 1);
        timer2 = timer;
        timerText.text = timer.ToString();

        if(timer == 0)
        {
            GenerateNewProblem();
            // ValidatePlayerPosition();
            LoseLife();
            correctAnswersCount = 0;
            player.ShowFeedback(false);
            timer = setTimer;
        }
    }

    public void LoseLife()
    {
        lives -= 1;
        livesManager.UpdateLives(lives);
        var extraLive = lives - 3;
        extraLiveText.text = extraLive > 0 ? $"+{extraLive}" : "";
        incorrectAnswersCount++;

        if (lives <= 0)
        {
            isGameOver = true; // Detener el tiempo
        string finalTime = GetFormattedTime();
        Debug.Log($"Tiempo final de juego: {finalTime}");

            livesManager.GameOver();
            ModelSaveData gameData = new ModelSaveData{
                player = "Player",
                score = $"{score:D7}",
                time = "01:00",
                correctAnswer= totalAnswerCorrect.ToString()
            };

            SaveManager.SavePlayerData(gameData);
            CancelInvoke();
            HideTexts();
            player.StopPlayer();
        }
    }

    private void HideTexts()
    {
        problemText.gameObject.SetActive(false);
        Answer1Text.gameObject.SetActive(false);
        Answer2Text.gameObject.SetActive(false);
        Answer3Text.gameObject.SetActive(false);
    }

    public void GenerateNewProblem()
    {
        if (level <= 6)
        {
            switch (level)
            {
                case 1: // Principiante
                Debug.Log("Principiante");
                    GenerateProblemByOperationSet(new char[] { '+', '-', '*', '/' }, 2);
                    setTimer = 25;
                    break;
                case 2: // Semi pro (decimales)
                Debug.Log("Semi pro");
                    GenerateDecimalProblemByOperationSet(new char[] { '+', '-', '*', '/' }, 2,1);
                    setTimer = 20;
                    break;
                case 3: // Pro (fracciones)
                Debug.Log("Pro");
                    GenerateFractionProblemByOperationSet(new char[] { '+', '-', '*', '/' }, 2);
                    setTimer = 15;
                    break;
                case 4: // Experto (mezclado)
                Debug.Log("Experto");
                    GenerateMixedProblem(2);
                    setTimer = 10;
                    break;
                case 5: // Genio (3 dígitos)
                Debug.Log("Genio");
                    GenerateLargeNumberProblem(3, 2);
                    setTimer = 8;
                    break;
                case 6: // Leyenda (4 dígitos)
                Debug.Log("Leyenda");
                    GenerateLargeNumberProblem(4, 2);
                    setTimer = 6;
                    break;
            }
        }
        else
        {
            // Después del nivel 6, problemas aleatorios como en el nivel 4 en adelante
            int randomFunction = UnityEngine.Random.Range(1, 4); // Elegir entre las 3 funciones
            switch (randomFunction)
            {
                case 1:
                    GenerateMixedProblem(2);
                    break;
                case 2:
                    GenerateLargeNumberProblem(3, 2); // Números de 3 dígitos
                    break;
                case 3:
                    GenerateLargeNumberProblem(4, 2); // Números de 4 dígitos
                    break;
            }
            setTimer = 8;
        }

        // Activa la animaci�n de los botones despu�s de generar un nuevo problema
        if (buttonAnimator != null)
        {
            buttonAnimator.ActivateAnimation();
        }
    }

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

void GenerateProblemByOperationSet(char[] operations, int problemsPerOperation)
{
    int operationIndex = (correctAnswersCount / problemsPerOperation) % operations.Length;
    char operation = operations[operationIndex];
    int num1 = UnityEngine.Random.Range(1, 11);
    int num2 = UnityEngine.Random.Range(1, 11);

    if (operation == '/')
    {
        // Asegurarse de no dividir por 0
        while (num2 == 0)
        {
            num2 = UnityEngine.Random.Range(1, 11);
        }

        // División con 2 decimales
        correctAnswer = MathF.Round((float)num1 / num2, 2);
    }
    else
    {
        correctAnswer = SolveProblem(num1, num2, operation);
    }

    // Mostrar "÷" en lugar de "/"
    string displayOperation = operation == '/' ? "÷" : operation.ToString();
    DisplayProblem($"{num1} {displayOperation} {num2}", operation);
}

    void GenerateBasicProblem()
    {
        int num1 = UnityEngine.Random.Range(1, 11);
        int num2 = UnityEngine.Random.Range(1, 11);
        char[] operations;
        if (correctAnswersCount < 3)
        {
            operations = new char[] { '+', '-' }; // Solo suma y resta
        }
        else
        {
            operations = new char[] { '*', '/' }; // Multiplicaci�n y divisi�n
        }

        // Selecciona la operaci�n al azar de las permitidas
        char operation = operations[UnityEngine.Random.Range(0, operations.Length)];

        // Resuelve el problema y muestra
        //correctAnswer = SolveProblem(num1, num2, operation);
        if (operation == '/')
        {
            // Asegurarse de no dividir por 0
            while (num2 == 0)
            {
                num2 = UnityEngine.Random.Range(1, 11);
            }

            // Divisi�n con 2 decimales
            correctAnswer = MathF.Round((float)num1 / num2, 2);
        }
        else
        {
            correctAnswer = SolveProblem(num1, num2, operation);
        }

        DisplayProblem($"{num1} {operation} {num2}", operation);
    }

void GenerateDecimalProblemByOperationSet(char[] operations, int problemsPerOperation, int decimalPlaces)
{
    int operationIndex = (correctAnswersCount / problemsPerOperation) % operations.Length;
    char operation = operations[operationIndex];
    float num1;
    if (level == 2)
    {
        num1 = UnityEngine.Random.Range(1, 11); // Sin decimales
    }
    else
    {
        num1 = MathF.Round(UnityEngine.Random.Range(1f, 10f), decimalPlaces); // Con decimales
    }
    float num2 = MathF.Round(UnityEngine.Random.Range(1f, 10f), decimalPlaces);

    switch (operation)
    {
        case '+':
            correctAnswer = MathF.Round(num1 + num2, decimalPlaces);
            break;
        case '-':
            correctAnswer = MathF.Round(num1 - num2, decimalPlaces);
            break;
        case '*':
            correctAnswer = MathF.Round(num1 * num2, decimalPlaces);
            break;
        case '/':
            correctAnswer = MathF.Round(num1 / num2, decimalPlaces);
            break;
    }

    string displayOperation = (operation == '/' && level == 2) ? "÷" : operation.ToString();

    Debug.Log($"Operacion {operation} - Num1 {num1} - Num2 {num2} - CorrectAnswer {correctAnswer}");
    DisplayProblem($"{MathF.Round(num1, decimalPlaces)} {displayOperation} {MathF.Round(num2, decimalPlaces)}", operation);

}

    void GenerateDecimalProblem()
    {
        float num1, num2;
        char[] operations;

        if (correctAnswersCount < 3)
        {
            // Solo suma y resta con un decimal
            operations = new char[] { '+', '-' };
            num1 = MathF.Round(UnityEngine.Random.Range(1f, 10f), 1); // Un decimal
            num2 = MathF.Round(UnityEngine.Random.Range(1f, 10f), 1); // Un decimal
        }
        else
        {
            // Multiplicaci�n o divisi�n con dos decimales
            operations = new char[] { '*', '/' };
            num1 = MathF.Round(UnityEngine.Random.Range(1f, 10f), 2); // Dos decimales
            num2 = MathF.Round(UnityEngine.Random.Range(1f, 10f), 2); // Dos decimales
            if (operations.Contains('/') && num2 == 0)
            {
                num2 = MathF.Round(UnityEngine.Random.Range(1f, 10f), 2); // Evita divisi�n por cero
            }
        }

        char operation = operations[UnityEngine.Random.Range(0, operations.Length)];
        switch (operation)
        {
            case '+':
                correctAnswer = MathF.Round(num1 + num2, 2);
                break;
            case '-':
                correctAnswer = MathF.Round(num1 - num2, 2);
                break;
            case '*':
                correctAnswer = MathF.Round(num1 * num2, 2);
                break;
            case '/':
                correctAnswer = MathF.Round(num1 / num2, 2);
                break;
        }

        DisplayProblem($"{num1:F2} {operation} {num2:F2}", operation);
    }

    void GenerateFractionProblemByOperationSet(char[] operations, int problemsPerOperation)
    {
        int operationIndex = (correctAnswersCount / problemsPerOperation) % operations.Length;
        char operation = operations[operationIndex];
        GenerateFractionProblem();
    }

    void GenerateFractionProblem()
    {
        int numerator1 = UnityEngine.Random.Range(1, 11);
        int numerator2 = UnityEngine.Random.Range(1, 11);
        int denominator1 = UnityEngine.Random.Range(1, 11);
        int denominator2 = UnityEngine.Random.Range(1, 11);
        char[] operations;

        if (correctAnswersCount < 3)
        {
            // Solo suma y resta
            operations = new char[] { '+', '-' };

            if (correctAnswersCount == 0)
            {
                // Primera operaci�n: mismo denominador
                denominator2 = denominator1;
            }
        }
        else
        {
            // Multiplicaci�n o divisi�n
            operations = new char[] { '*' };
        }

        char operation = operations[UnityEngine.Random.Range(0, operations.Length)];

        if (denominator1 < 0)
        {
            numerator1 = -numerator1; // Poner el signo en el numerador
            denominator1 = Mathf.Abs(denominator1); // Hacer positivo el denominador
        }

        if (denominator2 < 0)
        {
            numerator2 = -numerator2; // Poner el signo en el numerador
            denominator2 = Mathf.Abs(denominator2); // Hacer positivo el denominador
        }

        // Calcular el resultado correcto
        correctAnswer = ConvertToFractionResult(
            numerator1,
            denominator1,
            numerator2,
            denominator2,
            operation
        );

        string fraction1 = $"{numerator1}/{denominator1}";
        string fraction2 = $"{numerator2}/{denominator2}";

        DisplayProblem($"{fraction1} {operation} {fraction2}", operation);
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
                throw new System.Exception("Operaci�n no v�lida");
        }

        int gcd = GCD(resultNumerator, resultDenominator);
        resultNumerator /= gcd;
        resultDenominator /= gcd;

        return $"{resultNumerator}/{resultDenominator}";
    }

    void GenerateMixedProblem(int problemsPerOperation)
    {
        int randomFunction = UnityEngine.Random.Range(1, 5); // Elegir entre las 3 funciones
        switch (randomFunction)
        {
            case 1: // Principiante
                GenerateProblemByOperationSet(new char[] { '*', '/' }, 4);
                break;
            case 2: // Semi pro (decimales)
                GenerateDecimalProblemByOperationSet(new char[] { '+', '-', '*', '/' }, 2,2);
                break;
            case 3: // Pro (fracciones)
                GenerateFractionProblemByOperationSet(new char[] { '+', '-', '*', '/' }, 2);
                break;
        }
    }

    //void GenerateMixedProblem()
    //{
    //    int type = UnityEngine.Random.Range(1, 4);
    //    if (type == 1)
    //        GenerateBasicProblem();
    //    else if (type == 2)
    //        GenerateDecimalProblem();
    //    else
    //        GenerateFractionProblem();
    //}

    void GenerateLargeNumberProblem(int digits, int problemsPerOperation)
    {
        char[] allOperations = { '+', '-', '*', '/' };
        int operationIndex = (correctAnswersCount / problemsPerOperation) % allOperations.Length;
        char operation = allOperations[operationIndex];

        int num1 = UnityEngine.Random.Range((int)Mathf.Pow(10, digits - 1), (int)Mathf.Pow(10, digits));
        int num2 = UnityEngine.Random.Range((int)Mathf.Pow(10, digits - 1), (int)Mathf.Pow(10, digits));

        correctAnswer = SolveProblem(num1, num2, operation);
        DisplayProblem($"{num1} {operation} {num2}", operation);
    }

float SolveProblem(float num1, float num2, char operation)
{
    switch (operation)
    {
        case '+': 
            return num1 + num2;
        case '-': 
            return num1 - num2;
        case '*': 
            return num1 * num2;
        case '/': 
            return (num2 == 0) ? 0f : MathF.Round(num1 / num2, 2); // Redondear a 2 decimales
        default: 
            return 0f;
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

void DisplayProblem(string problem, char operation)
{
    problemText.text = problem;
    StartCoroutine(DelayedAssignAnswers(operation));
}

IEnumerator DelayedAssignAnswers(char operation)
{
    yield return new WaitForSeconds(1.2f); // Espera 1 segundo

    if (level == 1)
    {
        AssignAnswers(correctAnswer, operation);
    }
    else if (level == 2)
    {
        AssignAnswers((float)correctAnswer, operation);
    }
    else if (level == 3 || level == 4)
    {
        AssignAnswers(correctAnswer.ToString(), operation);
    }
    else if (level >= 5)
    {
        AssignAnswers(correctAnswer, operation);
    }
}

    void AssignAnswers(object correctAnswer, char operation)
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
        if (level == 2)
        {
            float deviation = UnityEngine.Random.Range(-1f, 1f);
            float generatedAnswer = Convert.ToSingle(correctAnswer) + deviation;
            incorrectAnswer = MathF.Round(generatedAnswer, 1).ToString("F1");
        }
                    else if (operation == '/')
                    {
                        // Asegurar que las divisiones siempre tengan 2 decimales
                        float deviation = UnityEngine.Random.Range(-1f, 1f);
                        float generatedAnswer = Convert.ToSingle(correctAnswer) + deviation;
                        incorrectAnswer = MathF.Round(generatedAnswer, 2).ToString("F2");
                    }

                    else if (level == 1)
                    {
                        var validateType = Int32.Parse(correctAnswer.ToString());
               
                        if (!(validateType is int))
                        {
                            float deviation = UnityEngine.Random.Range(-1f, 1f);
                            float generatedAnswer = Convert.ToSingle(correctAnswer) + deviation;
                            incorrectAnswer = MathF.Round(generatedAnswer, 2).ToString("F2");
                        }
                        else
                        {
                            int deviation = UnityEngine.Random.Range(-9, 10);
                            incorrectAnswer = (Convert.ToInt32(correctAnswer) + deviation).ToString();
                        }
                    }

                    else if (level == 3 )
                    {
                        Debug.Log($"Tipo de dato de correctAnswer: {correctAnswer.GetType()}");
                        
                        Debug.Log("Level 3 o 4");
                        incorrectAnswer = GenerateRandomFraction();

                        Debug.Log(incorrectAnswer);
                        Debug.Log($"level: {level}");
                    }
                    
                    else if (level == 4)
                    {
                    try
                     {
                       if (int.TryParse(correctAnswer.ToString(), out int correctValueInt))
                         {
            int deviation = UnityEngine.Random.Range(-9, 10);
            incorrectAnswer = (correctValueInt + deviation).ToString();
        }
        else if (float.TryParse(correctAnswer.ToString(), out float correctValueFloat))
        {
            float deviation = UnityEngine.Random.Range(-1f, 1f);
            float generatedAnswer = correctValueFloat + deviation;
            incorrectAnswer = MathF.Round(generatedAnswer, 2).ToString("F2");
        }
        else if (correctAnswer is string correctValueString)
        {
            // Generar fracciones o valores cercanos como texto
            incorrectAnswer = GenerateRandomFraction();
            Debug.Log($"Correcto #3: {correctValueString} - Incorrecto: {incorrectAnswer}");
        }
        else
        {
            throw new System.Exception("Tipo de respuesta no manejado para el nivel 4.");
        }
    }
    catch (Exception ex)
    {
        Debug.LogError($"Error processing answer: {ex.Message}");
        incorrectAnswer = "Error";
    }
}

else if (level >= 5)
{
    Debug.Log("Level 5 o superior");
    // Niveles 5 y superiores: Generar desviaciones más cercanas
                 var validateType = Int32.Parse(correctAnswer.ToString());
               
                        if (!(validateType is int))
                        {
                            float deviation = UnityEngine.Random.Range(-1f, 1f);
                            float generatedAnswer = Convert.ToSingle(correctAnswer) + deviation;
                            incorrectAnswer = MathF.Round(generatedAnswer, 2).ToString("F2");
                        }
                        else
                        {
                            int deviation = UnityEngine.Random.Range(-9, 10);
                            incorrectAnswer = (Convert.ToInt32(correctAnswer) + deviation).ToString();
                        }
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

        switch (level)
        {
            case 1:
                levelText.text = "Principiante";
                break;
            case 2:
                levelText.text = "Semi pro";
                break;
            case 3:
                levelText.text = "Pro";
                break;
            case 4:
                levelText.text = "Experto";
                break;
            case 5:
                levelText.text = "Genio";
                break;
            case 6:
                levelText.text = "Leyenda";
                break;
            default:
                levelText.text = "Leyenda";
                break;
        }
    }

    void ValidatePlayerPosition()
    {
        // Busca el objeto del jugador y llama a su funci�n de validaci�n
        player.ValidatePosition();
    }

    public void AddScore(int points)
    {
        float timeBonus = CalculateTimeBonus();
        int totalPoints = Mathf.RoundToInt(points * timeBonus); // Aplica la bonificaci�n
        score += totalPoints;

        UpdateScoreText();
    }

    private float CalculateTimeBonus()
    {
        float timePercentage = (float)timer2 / setTimer;

        if (timePercentage > 0.75f)
            return 3f; // Bonificaci�n 3x
        else if (timePercentage > 0.5f)
            return 2f; // Bonificaci�n 2x
        else if (timePercentage > 0.25f)
            return 1.25f; // Bonificaci�n 1.25xchangeTimer
        else
            return 1f; // Sin bonificaci�n
    }


    private void UpdateScoreText()
    {
        // Formatear el texto con "Score" y un n�mero de 7 d�gitos
        ScoreText.text = $"Score {score:D7}";
    }
}
