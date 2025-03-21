using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameScore : MonoBehaviour
{
    public TextMeshProUGUI letra1;
    public TextMeshProUGUI letra2;
    public TextMeshProUGUI letra3;
    public TextMeshProUGUI ScoreText;
    public GameObject marco;
    public GameObject panel;

    public MathProblemGenerator mathProblemGenerator; // Referencia al script MathProblemGenerator

    private char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    private int index1 = 0;
    private int index2 = 0;
    private int index3 = 0;

    private int marcoPositionIndex = 1; // Índice inicial (0 = -350, 1 = 0, 2 = 350)
    private readonly float[] marcoPositions = { -350f, 0f, 350f }; // Posiciones permitidas

void Start()
{
    letra1 = GameObject.Find("Letra1").GetComponent<TextMeshProUGUI>();
    letra2 = GameObject.Find("Letra2").GetComponent<TextMeshProUGUI>();
    letra3 = GameObject.Find("Letra3").GetComponent<TextMeshProUGUI>();

    // Buscar el objeto por su nombre y obtener el componente MathProblemGenerator
    GameObject obj = GameObject.Find("NombreDelGameObject"); // Cambia "NombreDelGameObject" por el nombre real del objeto
    if (obj != null)
    {
        mathProblemGenerator = obj.GetComponent<MathProblemGenerator>();
        if (mathProblemGenerator != null)
        {
            Debug.Log($"MathProblemGenerator encontrado. Score inicial: {mathProblemGenerator.score}");
        }
        else
        {
            Debug.LogWarning("MathProblemGenerator no encontrado en el objeto.");
        }
    }
    else
    {
        Debug.LogWarning("El objeto con el nombre especificado no fue encontrado.");
    }

    UpdateLetters();
    UpdateScoreText();

    // Iniciar la animación de titileo
    StartCoroutine(TitileoMarco());
}
    void Update()
    {
        // Verificar si el panel está activo
        if (!panel.activeSelf)
        {
            return; // Salir si el panel no está activo
        }

        // Mover el marco con las teclas A y D
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveMarco(-1); // Mover a la izquierda
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveMarco(1); // Mover a la derecha
        }

        // Cambiar el carácter con las teclas W y S
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeCharacter(1); // Incrementar el carácter
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeCharacter(-1); // Decrementar el carácter
        }

        // Actualizar el texto del puntaje en cada frame
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (mathProblemGenerator != null)
        {
            // Actualizar el texto de ScoreText con el valor de score
            ScoreText.text = $"{mathProblemGenerator.score}";
        }
    }

    private void MoveMarco(int direction)
    {
        // Actualizar el índice de posición del marco
        marcoPositionIndex = Mathf.Clamp(marcoPositionIndex + direction, 0, marcoPositions.Length - 1);

        // Mover el marco a la nueva posición
        Vector3 newPosition = marco.transform.localPosition;
        newPosition.x = marcoPositions[marcoPositionIndex];
        marco.transform.localPosition = newPosition;

        Debug.Log($"Marco movido a la posición: {newPosition.x}");
    }

    private void ChangeCharacter(int direction)
    {
        // Cambiar el carácter según la posición del marco
        switch (marcoPositionIndex)
        {
            case 0: // Posición de letra1
                index1 = (index1 + direction + alphabet.Length) % alphabet.Length;
                letra1.text = alphabet[index1].ToString();
                break;
            case 1: // Posición de letra2
                index2 = (index2 + direction + alphabet.Length) % alphabet.Length;
                letra2.text = alphabet[index2].ToString();
                break;
            case 2: // Posición de letra3
                index3 = (index3 + direction + alphabet.Length) % alphabet.Length;
                letra3.text = alphabet[index3].ToString();
                break;
        }

        Debug.Log($"Carácter cambiado en posición {marcoPositionIndex}");
    }

    private IEnumerator TitileoMarco()
    {
        // Alternar la opacidad del marco para crear un efecto de titileo
        SpriteRenderer renderer = marco.GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            Debug.LogWarning("El marco no tiene un componente SpriteRenderer.");
            yield break;
        }

        while (true)
        {
            // Reducir la opacidad del marco
            Color color = renderer.color;
            color.a = 0.5f; // Opacidad al 50%
            renderer.color = color;
            yield return new WaitForSeconds(0.5f);

            // Restaurar la opacidad del marco
            color.a = 1f; // Opacidad al 100%
            renderer.color = color;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void UpdateLetters()
    {
        letra1.text = alphabet[index1].ToString();
        letra2.text = alphabet[index2].ToString();
        letra3.text = alphabet[index3].ToString();
    }
}