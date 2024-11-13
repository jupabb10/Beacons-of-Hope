using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MathProblemGenerator : MonoBehaviour
{
    public TextMeshProUGUI problemText; // Campo para mostrar el problema
    private int correctAnswer; // Respuesta correcta para verificar m�s adelante

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GenerateNewProblem", 0f, 5f);
    }

    public void GenerateNewProblem()
    {
        // Genera dos n�meros aleatorios entre 1 y 10
        int num1 = Random.Range(1, 11);
        int num2 = Random.Range(1, 11);

        // Genera un operador aleatorio (0 = suma, 1 = resta, 2 = multiplicaci�n, 3 = divisi�n)
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
            case 2: // Multiplicaci�n
                correctAnswer = num1 * num2;
                operatorSymbol = "�";
                break;
            case 3: // Divisi�n
                // Aseg�rate de que el segundo n�mero sea un divisor del primero para obtener un entero
                while (num1 % num2 != 0)
                {
                    num2 = Random.Range(1, 11);
                }
                correctAnswer = num1 / num2;
                operatorSymbol = "�";
                break;
        }

        // Muestra el problema en el texto
        problemText.text = $"{num1} {operatorSymbol} {num2} = ?";
    }

    public int GetCorrectAnswer()
    {
        return correctAnswer;
    }
}
