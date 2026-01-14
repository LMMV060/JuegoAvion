using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private AudioClip shotSound;
    
    [SerializeField] private GameObject explosionPrefab;
    
    // [SerializeField] private float ttl = 3f;  // -> Time To Live
    // private float _ttl;

    /*[SerializeField] private float maxDistance;
    private float _maxDistance;*/
    
    /*private SpriteRenderer _spriteRenderer;
    private bool _hasBeenVisible;*/
    void Awake()
    {
       // _ttl = ttl;
       // _maxDistance = maxDistance;
       /*_spriteRenderer = GetComponent<SpriteRenderer>();
       _hasBeenVisible = false;*/
    }

    private void Start()
    {
        // Hacer sonar efecto de disparo
        AudioSource.PlayClipAtPoint(shotSound, transform.position, 1f);
    }
    
    void Update()
    {
        // UPDATE POSITION
        transform.Translate(Vector2.up * (speed * Time.deltaTime));
        
        // ESTO YA NO SE USA
        // update Time to Live por tiempo asignado
        /* _ttl -= Time.deltaTime;
        if (_ttl <= 0)
        {
            Destroy(gameObject);
        } */
        
        // update distance  y hacer morir objeto definido por unidades de pantalla
        /*_maxDistance -= speed * Time.deltaTime;
        if (_maxDistance <= 0f)
        {
            Destroy(gameObject);
        }*/

        /*if (!_spriteRenderer.isVisible)
        {
            _hasBeenVisible = true;
        }
        else
        {
            if (_hasBeenVisible)
            {
                Destroy(this.gameObject);
            }
        }*/
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // verify if the other object is an enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.HitByPlayerShot();
                // Create a new GameObject explosionPrefab
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = transform.position;
                Destroy(gameObject);
            }
        }
    }
}
