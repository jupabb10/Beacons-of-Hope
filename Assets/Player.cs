using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    public float fuerzaSalto;
    public float fuerzaMovimiento;
    private Rigidbody2D rigidbody2;

    private float[] positions = { -1.65f, 0f, 1.65f }; // Izquierda, centro, derecha
    private int currentPositionIndex = 1;
    public float moveSpeed = 5f;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "boton")
        {
            rigidbody2.AddForce(new Vector2(0, fuerzaSalto));
        }
    }
}
