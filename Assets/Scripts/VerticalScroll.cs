using UnityEngine;

public class VerticalScroll : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.down;
    }
}
