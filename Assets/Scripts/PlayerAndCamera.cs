using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAndCamera : MonoBehaviour
{
  
  [Header("Physics")]
  [SerializeField] private Vector2 speed = new Vector2(5f, 8f);

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
  
  //NEW
  [Header("Limits")]
  [SerializeField] private SpriteRenderer background;
  
  private Rigidbody2D _rigidbody2D;
  private SpriteRenderer _spriteRenderer;
  private Vector2 _direction;
  private float _timeTurning;

  
  void Start()
  {
    //get reference to Rigidbody
    _rigidbody2D = GetComponent<Rigidbody2D>();
    _spriteRenderer = GetComponent<SpriteRenderer>();
  }

  private void Awake()
  {
    _rigidbody2D = GetComponent<Rigidbody2D>();
    _spriteRenderer = GetComponent<SpriteRenderer>();
    // if camera is not assigment get Camera from scene
    if (cam == null)
    {
      cam = Camera.main;
    }
  }

  void Update()
  {
    GetInputs();
    UpdatePhysics();
    UpdateGraphics();
  }

  private void LateUpdate()
  {
    UpdateCamera();
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
      position.y += cameraSpeed * Time.deltaTime;
    }

    // --- LÍMITES DE LA CÁMARA EN X ---
    if (background != null)
    {
      float camHalfWidth = cam.orthographicSize * cam.aspect;
      float bgHalfWidth = background.bounds.size.x / 2f;
      float limitX = bgHalfWidth - camHalfWidth;

      // Clampeo de la cámara para que no vea se vea fuera del fondo (solo si el fondo es más ancho que la cámara)
      if (limitX > 0)
        position.x = Mathf.Clamp(position.x, background.transform.position.x - limitX, background.transform.position.x + limitX);
      else
        position.x = background.transform.position.x;
    }

    cam.transform.position = position;
  }

  private void UpdateGraphics()
  {
    // sprite representation switch turnning time
    if (_timeTurning >= 0.5f)
    {
      // set sprite right or left depending on the lateral velocity sign
      _spriteRenderer.sprite = (_rigidbody2D.linearVelocity.x > 0) ? spriteTurnRight02 : spriteTurnLeft02;
    }
    else if (_timeTurning > 0f)
    {
      _spriteRenderer.sprite = (_rigidbody2D.linearVelocity.x > 0) ? spriteTurnRight01 : spriteTurnLeft01;
    }
    else
    {
      _spriteRenderer.sprite = spriteNormal;
    }
  }

  private void GetInputs()
  {
    //Set Vector a 0
    _direction = Vector2.zero;
    
    //Set Planet move direction
    if (Input.GetKey(KeyCode.W))
    {
      _direction += Vector2.up;
    } 
    
    if (Input.GetKey(KeyCode.S))
    {
      _direction += Vector2.down;
    }

    if (Input.GetKey(KeyCode.A))
    {
      _direction += Vector2.left;
    }

    if (Input.GetKey(KeyCode.D))
    {
      _direction += Vector2.right;
    }
    
    //reset turning time when the player start moving
    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
    {
      _timeTurning = 0;
    }
    
    // player fire shot
    if (Input.GetKeyDown(KeyCode.Space))
    {
      // TODO Fire()
    }
  }
  
  private void UpdatePhysics()
  {
    _rigidbody2D.linearVelocity = _direction * speed;
    // --- LÍMITES DEL JUGADOR EN X ---
    if (background != null)
    {
        float bgHalfWidth = background.bounds.size.x / 2f;
        float playerX = Mathf.Clamp(transform.position.x, background.transform.position.x - bgHalfWidth, background.transform.position.x + bgHalfWidth);
        transform.position = new Vector3(playerX, transform.position.y, transform.position.z);
    }
    // update the "turning time" of the plane if the lateral speed (in any direction) is greater than a minimum threshold (1)
    if (Mathf.Abs(_rigidbody2D.linearVelocity.x) > 1)
    {
      _timeTurning += Time.deltaTime;
      _timeTurning = Mathf.Min(_timeTurning, 0.5f);
    }
    else
    {
      // return to the horizontal position (slowly)
      _timeTurning -= Time.deltaTime * 2f;
      _timeTurning = Mathf.Max(_timeTurning, 0f);
    }
  }
  
}
