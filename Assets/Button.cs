using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Button : MonoBehaviour
{

    // Referencias a las im�genes que se alternar�n
    public Sprite defaultSprite;
    public Sprite collisionSprite;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {

        // Obtiene el SpriteRenderer para cambiar las im�genes
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Asegura que la imagen inicial sea la predeterminada
        if (spriteRenderer != null && defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collider2D collision)
    {
        print("Colisione");
        // Verifica si la colisi�n es con el jugador
        //if (collision.GetComponent<Collider>().CompareTag("Player"))
        //{
        //    // Cambia la imagen a la de colisi�n
        //    if (spriteRenderer != null && collisionSprite != null)
        //    {
        //        spriteRenderer.sprite = collisionSprite;
        //    }

            
        //}
    }

    void OnCollisionExit2D(Collider2D collision)
    {
        print("Colisione");
        //if (collision.GetComponent<Collider>().CompareTag("Player"))
        //{
        //    // Restablece la imagen predeterminada
        //    if (spriteRenderer != null && defaultSprite != null)
        //    {
        //        spriteRenderer.sprite = defaultSprite;
        //    }
        //}
    }
}
