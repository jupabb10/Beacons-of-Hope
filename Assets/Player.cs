using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    public float fuerzaSalto;
    public float fuerzaMovimiento;
    private Rigidbody2D rigidbody2;

    private float[] positions = { -1.65f, 0f, 1.65f }; // Izquierda, centro, derecha
    private int currentPositionIndex = 1;
    public float moveSpeed = 5f;

    private bool canMove = true;

    public GameObject feedbackPanel; // Referencia al panel de feedback
    public Color correctColor = Color.green; // Color para respuestas correctas
    public Color incorrectColor = Color.red; // Color para respuestas incorrectas
    public Color defaultColor = new Color(1, 1, 1, 0); // Transparente por defecto
    private Image panelImage;

    public MathProblemGenerator mathProblemGenerator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();

        if (feedbackPanel != null)
        {
            // Obtener el componente Image del panel
            panelImage = feedbackPanel.GetComponent<Image>();
            if (panelImage != null)
            {
                // Asegurarse de que el color inicial sea el por defecto
                panelImage.color = defaultColor;
                feedbackPanel.SetActive(false);
            }
        }
    }

    public void ShowFeedback(bool isCorrect)
    {
        if (panelImage == null) return;
        feedbackPanel.SetActive(true);
        panelImage.color = isCorrect ? correctColor : incorrectColor;

        Invoke(nameof(ResetPanelColor), 1f);
    }

    private void ResetPanelColor()
    {
        if (panelImage != null)
        {
            // Volver al color por defecto (transparente)
            panelImage.color = defaultColor;
            feedbackPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Detecta input para moverse a la izquierda
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePlayer(-1); // Mover hacia la izquierda
        }
        // Detecta input para moverse a la derecha
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePlayer(1); // Mover hacia la derecha
        }

        // Interpolación suave hacia la posición deseada
        Vector3 targetPosition = new Vector3(positions[currentPositionIndex], transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void MovePlayer(int direction)
    {
        // Actualiza la posición solo si no estamos en los extremos
        int newPositionIndex = Mathf.Clamp(currentPositionIndex + direction, 0, positions.Length - 1);
        currentPositionIndex = newPositionIndex;
    }

    public void ValidatePosition()
    {
        if (mathProblemGenerator != null)
        {
            int correctIndex = mathProblemGenerator.GetCorrectAnswerIndex();
            GameObject correctButton = GameObject.FindWithTag($"boton{correctIndex}");

            if (currentPositionIndex == correctIndex)
            {
                mathProblemGenerator.timer = mathProblemGenerator.setTimer;
                mathProblemGenerator.correctAnswersCount++;
                mathProblemGenerator.AddScore(500);
                if (mathProblemGenerator.correctAnswersCount >= 4)
                {
                    mathProblemGenerator.LevelUp();
                }

                SoundManager.Instance.PlayEffectSound(SoundManager.Instance.correctSound);
                ShowFeedback(true);

                if (correctButton != null)
            {
                ButtonScript buttonScript = correctButton.GetComponent<ButtonScript>();
                if (buttonScript != null && buttonScript.winSprite != null)
                {
                    SpriteRenderer buttonSpriteRenderer = correctButton.GetComponent<SpriteRenderer>();
                    if (buttonSpriteRenderer != null)
                    {
                        buttonSpriteRenderer.sprite = buttonScript.winSprite;
                    }
                }
            }
            }
            else
            {
                mathProblemGenerator.correctAnswersCount = 0;
                mathProblemGenerator.LoseLife();
                ShowFeedback(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SpriteRenderer buttonSpriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
        ButtonScript buttonScript = collision.gameObject.GetComponent<ButtonScript>();
        string collisionTag = collision.gameObject.tag;

        if (collisionTag.StartsWith("boton")  && canMove)
        {
            rigidbody2.AddForce(new Vector2(0, fuerzaSalto));
            if (buttonSpriteRenderer != null)
            {
                if (buttonScript != null && buttonScript.newSprite != null)
                {
                    buttonSpriteRenderer.sprite = buttonScript.newSprite;
                }
            }
        }
    }

    //private IEnumerator RestoreSpriteAfterDelay(SpriteRenderer spriteRenderer, Sprite mainSprite, float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    if (spriteRenderer != null)
    //    {
    //        spriteRenderer.sprite = mainSprite;
    //    }
    //}

    private void OnCollisionExit2D(Collision2D collision)
    {
        string collisionTag = collision.gameObject.tag;

        if (collisionTag.StartsWith("boton"))
        {
            // Restaura el sprite original del botón
            SpriteRenderer buttonSpriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            ButtonScript buttonScript = collision.gameObject.GetComponent<ButtonScript>();

            if (buttonSpriteRenderer != null && buttonScript != null && buttonScript.mainSprite != null)
            {
                buttonSpriteRenderer.sprite = buttonScript.mainSprite;
            }
        }
    }

    public void StopPlayer()
    {
        canMove = false; // Detiene cualquier acción del jugador
        rigidbody2.velocity = Vector2.zero; // Detiene cualquier movimiento actual
    }
}
