using UnityEngine;

public class CircularScroll : MonoBehaviour
{
    [SerializeField] private SpriteRenderer nextBackground;
    [SerializeField] private SpriteRenderer lastBackground;
    private SpriteRenderer _moveBackground;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        float direction = rb.linearVelocity.y;
        //Esto es para las nubes
        if (direction == 0) return;

        // Fondo/Nube actual
        SpriteRenderer current = GetComponent<SpriteRenderer>();

        // Determinar qué fondo mover
        if (direction > 0)
        {
            _moveBackground = nextBackground;
        }
        else
        {
            _moveBackground = lastBackground;
        }

        Vector3 nuevaPos = _moveBackground.transform.position;

        //Poner el fondo al final del fondo actual usando los bounds pillando el max (alto) y min (bajo)
        if (direction > 0)
        {
            nuevaPos.y += current.bounds.max.y - _moveBackground.bounds.min.y;
        }
        else
        {
            nuevaPos.y += current.bounds.min.y - _moveBackground.bounds.max.y;
        }

        //Aplica los calculos de antes para mover el fondo
        _moveBackground.transform.position = nuevaPos;
    }
}