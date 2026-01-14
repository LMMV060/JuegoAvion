using UnityEditor;
using UnityEngine;

public class VerticalScrollExtended : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private SpriteRenderer nextImage;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxColliderUp;
    private BoxCollider2D _boxColliderDown;
    
    private float _height = 0f;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _height = _spriteRenderer.bounds.size.y;
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        foreach (var colliderTMP in colliders)
        {
            // ignore laterals triggers colliders via physical material
            if (colliderTMP.sharedMaterial != null)
            {
                continue;
            }
            if (colliderTMP.offset.y > 0)
            {
                _boxColliderUp = colliderTMP;
            }
            else
            {
                _boxColliderDown = colliderTMP;
            }
        }
    }
    
    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.down;
    }

    // Detect when player enter on the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the current parallax image position
            Vector3 currentPosition = transform.position;
            
            // Get player position
            Vector3 playerPosition = other.transform.position;
            
            // Player distance to the upper collider
            float distanceToUpperCollider = Mathf.Abs(playerPosition.y - (currentPosition.y + _boxColliderUp.offset.y));
            // Player distance to the lower collider
            float distanceToLowerCollider = Mathf.Abs(playerPosition.y - (currentPosition.y + _boxColliderDown.offset.y));
            //Debug.Log("Distance to Upper Collider: " + distanceToUpperCollider);
            //Debug.Log("Distance to Lower Collider: " + distanceToLowerCollider);
            if (distanceToUpperCollider < distanceToLowerCollider)
            {
                nextImage.transform.position = new Vector3(currentPosition.x, currentPosition.y + _height, currentPosition.z);
            }
            else
            {
                nextImage.transform.position = new Vector3(currentPosition.x, currentPosition.y - _height, currentPosition.z);
            }
        }
    }
    
    
    // Draw Gizmos to visualize the colliders in the editor
    private void OnDrawGizmosOLD()
    {
        Gizmos.color = Color.red;
        if (_boxColliderUp != null)
        {
            // apply scale to offset to draw a no-filled box
            Vector3 center = transform.position + (Vector3)(_boxColliderUp.transform.localScale * _boxColliderUp.offset);
            Vector3 size = (Vector3)(_boxColliderUp.transform.localScale * _boxColliderUp.size);
            Gizmos.DrawWireCube(center, size);
        }
        if (_boxColliderDown != null)
        {
            // apply scale to offset to draw a no-filled box
            Vector3 center = transform.position + (Vector3)(_boxColliderDown.transform.localScale * _boxColliderDown.offset);
            Vector3 size = (Vector3)(_boxColliderDown.transform.localScale * _boxColliderDown.size);
            Gizmos.DrawWireCube(center, size);
        }
    }
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = new Color(1f, 0f, 0f, 0.25f);
        if (_boxColliderUp != null)
        {
            Vector3 center = transform.position + (Vector3)(_boxColliderUp.transform.localScale * _boxColliderUp.offset);
            Vector3 size = (Vector3)(_boxColliderUp.transform.localScale * _boxColliderUp.size);
            // calculate vertex p1, p2, p3, p4 from center and size
            Vector3 p1 = center + new Vector3(-size.x / 2, -size.y / 2, 0);
            Vector3 p2 = center + new Vector3(-size.x / 2, size.y / 2, 0);
            Vector3 p3 = center + new Vector3(size.x / 2, size.y / 2, 0);
            Vector3 p4 = center + new Vector3(size.x / 2, -size.y / 2, 0);
            Handles.DrawSolidRectangleWithOutline(
                new Vector3[]
                {
                    p1, p2, p3, p4
                },
                new Color(1f, 0f, 0f, 0.25f),
                Color.red
            );
        }
        if (_boxColliderDown != null)
        {
            Vector3 center = transform.position + (Vector3)(_boxColliderDown.transform.localScale * _boxColliderDown.offset);
            Vector3 size = (Vector3)(_boxColliderDown.transform.localScale * _boxColliderDown.size);
            // calculate vertex p1, p2, p3, p4 from center and size
            Vector3 p1 = center + new Vector3(-size.x / 2, -size.y / 2, 0);
            Vector3 p2 = center + new Vector3(-size.x / 2, size.y / 2, 0);
            Vector3 p3 = center + new Vector3(size.x / 2, size.y / 2, 0);
            Vector3 p4 = center + new Vector3(size.x / 2, -size.y / 2, 0);
            Handles.DrawSolidRectangleWithOutline(
                new Vector3[]
                {
                    p1, p2, p3, p4
                },
                new Color(1f, 0f, 0f, 0.25f),
                Color.red
            );
        }
    }
    #endif
}