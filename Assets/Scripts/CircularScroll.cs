using UnityEngine;

public class CircularScroll : MonoBehaviour
{
    //SerializeField para el siguiente y anterior fondo
    [SerializeField] private SpriteRenderer nextBackground;
    [SerializeField] private SpriteRenderer lastBackground;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Comprueba al jugador (No hay otro rb)
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector3 position = transform.position;
        float height = nextBackground.size.y;
        
        //Si el jugador va hacia adelante
        if (rb.linearVelocity.y > 0)
        {
            position.y += height;
            nextBackground.transform.position = position;
        }
        //Si el jugador va hacia atrás
        else if (rb.linearVelocity.y < 0)
        {
            position.y -= height;
            lastBackground.transform.position = position;
        }
    }
}