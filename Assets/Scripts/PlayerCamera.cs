using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Vector2 speed;

    [Header("Sprites")] 
    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteTurnLeft01;
    [SerializeField] private Sprite spriteTurnLeft02;
    [SerializeField] private Sprite spriteTurnRight01;
    [SerializeField] private Sprite spriteTurnRight02;

    [Header("Camera")]
    [SerializeField] private Camera cam = null;

    [SerializeField] private float cameraSpeed = 1f;
    [SerializeField] private bool followPlayer = false;
    
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Vector2 _direction;
    private float _timeTurning;
    private void Start()
    {
        //get reference to RB
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        //Pa camara
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    private void Update()
    {
        GetInput();
        UpdatePhysics();
        UpdateGraphics();
    }

    

    private void UpdateGraphics()
    {
        // sprite representation switch turnning time
        if (_timeTurning >= 0.5f)
        {
            // set sprite right or left depending on the lateral velocity sign
            _sr.sprite = (_rb.linearVelocity.x > 0) ? spriteTurnRight02 : spriteTurnLeft02;
        }
        else if (_timeTurning > 0f)
        {
            _sr.sprite = (_rb.linearVelocity.x > 0) ? spriteTurnRight01 : spriteTurnLeft01;
        }
        else
        {
            _sr.sprite = spriteNormal;
        }
        
        //Debug.Log(_rb.linearVelocity.x);

    }

    private void UpdatePhysics()
    {
        _rb.linearVelocity = _direction * speed;

        if (Mathf.Abs(_rb.linearVelocity.x) > 1)
        {
            _timeTurning += Time.deltaTime;
            _timeTurning = Mathf.Min(_timeTurning, 0.5f);
        }
        else
        {
            _timeTurning -= Time.deltaTime * 2f;
            _timeTurning = Mathf.Max(_timeTurning, 0f);
        }
    }
    
    private void GetInput()
    {
        _direction = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            _direction += Vector2.up;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            _direction += Vector2.down;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            _direction += Vector2.right;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            _direction += Vector2.left;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            _timeTurning = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
        
    }
    
    private void UpdateCamera()
    {
        Vector3 position = cam.transform.position;

        if (followPlayer)
        {
            position.x = transform.position.x;
            position.y = transform.position.y;
        }
        else
        {
            position.y = position.y + cameraSpeed * Time.deltaTime;
        }
        
        cam.transform.position = position;
    }
}
