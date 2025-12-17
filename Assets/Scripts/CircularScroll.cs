using UnityEngine;

public class CircularScroll : MonoBehaviour
{
    [SerializeField] private SpriteRenderer nextBackground;

    // Move background repeat when touch central collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 position = transform.position;
        position.y += nextBackground.size.y;
        nextBackground.transform.position = position;
    }
}
