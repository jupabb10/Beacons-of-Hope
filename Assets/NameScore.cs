using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameScore : MonoBehaviour
{
    public TextMeshProUGUI letra1;
    public TextMeshProUGUI letra2;
    public TextMeshProUGUI letra3;

    private char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    private int index1 = 0;
    private int index2 = 0;
    private int index3 = 0;

    void Start()
    {
        letra1 = GameObject.Find("Letra1").GetComponent<TextMeshProUGUI>();
        letra2 = GameObject.Find("Letra2").GetComponent<TextMeshProUGUI>();
        letra3 = GameObject.Find("Letra3").GetComponent<TextMeshProUGUI>();
        UpdateLetters();
    }

    public void Prueba()
    {
        Debug.Log("Boton 1");
    }

    public void button_UP_1()
    {
        index1 = (index1 + 1) % alphabet.Length;
        UpdateLetters();
        Debug.Log("Boton UP 1");
    }

    public void button_DOWN_1()
    {
        index1 = (index1 - 1 + alphabet.Length) % alphabet.Length;
        UpdateLetters();
        Debug.Log("Boton DOWN 1");
    }

    public void button_UP_2()
    {
        index2 = (index2 + 1) % alphabet.Length;
        UpdateLetters();
        Debug.Log("Boton UP 2");
    }

    public void button_DOWN_2()
    {
        index2 = (index2 - 1 + alphabet.Length) % alphabet.Length;
        UpdateLetters();
        Debug.Log("Boton DOWN 2");
    }

    public void button_UP_3()
    {
        index3 = (index3 + 1) % alphabet.Length;
        UpdateLetters();
        Debug.Log("Boton UP 3");
    }

    public void button_DOWN_3()
    {
        index3 = (index3 - 1 + alphabet.Length) % alphabet.Length;
        UpdateLetters();
        Debug.Log("Boton DOWN 3");
    }

    private void UpdateLetters()
    {
        letra1.text = alphabet[index1].ToString();
        letra2.text = alphabet[index2].ToString();
        letra3.text = alphabet[index3].ToString();
    }
}