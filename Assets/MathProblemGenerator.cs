using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MathProblemGenerator : MonoBehaviour
{
    public TextMeshProUGUI problemText; // Campo para mostrar el problema

    public TextMeshProUGUI correctText; // Campo para mostrar el problema
    public TextMeshProUGUI falseText1; // Campo para mostrar el problema
    public TextMeshProUGUI falseText2; // Campo para mostrar el problema

    private int correctAnswer; // Respuesta correcta para verificar más adelante
    private int falseAnswer1 = 1; // Respuesta incorrecta
    private int falseAnswer2 = 2; // Respuesta incorrecta
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GenerateNewProblem", 0f, 5f);
    }

    public void GenerateNewProblem()
    {
        // Genera dos números aleatorios entre 1 y 10
        int num1 = Random.Range(1, 11);
        int num2 = Random.Range(1, 11);

        falseAnswer1 = Random.Range(1, 100);
        falseAnswer2 = Random.Range(1, 100);
        // Genera un operador aleatorio (0 = suma, 1 = resta, 2 = multiplicación, 3 = división)
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

        Debug.Log(correctAnswer);
        correctText.text = correctAnswer.ToString();
        falseText1.text = falseAnswer1.ToString();
        falseText2.text = falseAnswer2.ToString();
        // Muestra el problema en el texto
        problemText.text = $"{num1} {operatorSymbol} {num2} = ?";
    }

    public int GetCorrectAnswer()
    {
        return correctAnswer;
    }
}
