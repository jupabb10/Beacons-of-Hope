using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float fuerzaSalto;
    public float fuerzaMovimiento;
    private Rigidbody2D rigidbody2;

    private float[] positions = { -1.65f, 0f, 1.65f }; // Izquierda, centro, derecha
    private int currentPositionIndex = 1;
    public float moveSpeed = 5f;

    private bool canMove = true;

    public MathProblemGenerator mathProblemGenerator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
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

        // Interpolaci�n suave hacia la posici�n deseada
        Vector3 targetPosition = new Vector3(positions[currentPositionIndex], transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void MovePlayer(int direction)
    {
        // Actualiza la posici�n solo si no estamos en los extremos
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
                mathProblemGenerator.AddScore(500);
                SoundManager.Instance.PlayEffectSound(SoundManager.Instance.correctSound);

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
                mathProblemGenerator.LoseLife();
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
            // Restaura el sprite original del bot�n
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
        canMove = false; // Detiene cualquier acci�n del jugador
        rigidbody2.velocity = Vector2.zero; // Detiene cualquier movimiento actual
    }
}
