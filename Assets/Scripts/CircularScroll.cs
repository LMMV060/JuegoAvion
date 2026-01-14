using UnityEngine;

public class CircularScroll : MonoBehaviour
{
    [SerializeField] private SpriteRenderer nextBackground;

        // Comprobar constantemente si el jugador entra en Collider (OnTriggerEnter2D) o si ya está dentro (OnTriggerStay2D)
        private void OnTriggerEnter2D(Collider2D other)
        {
            UpdateBackgroundPosition(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            UpdateBackgroundPosition(other);
        }

        private void UpdateBackgroundPosition(Collider2D other)
        {
            if (other.attachedRigidbody == null) return;

            float verticalVelocity = other.attachedRigidbody.linearVelocity.y;
        
            // Si el jugador se mueve (incluso un poco), calculamos dónde debería estar el fondo
            if (Mathf.Abs(verticalVelocity) > 0.01f)
            {
                float direction = Mathf.Sign(verticalVelocity);
                float spriteHeight = nextBackground.size.y;

                // La posición ideal del fondo vecino es arriba o abajo del actual
                Vector3 targetPosition = transform.position;
                targetPosition.y += spriteHeight * direction;

                // Si el fondo vecino no está en esa posición, lo movemos
                if (Vector3.Distance(nextBackground.transform.position, targetPosition) > 0.1f)
                {
                    nextBackground.transform.position = targetPosition;
                }
            }
        }
}

