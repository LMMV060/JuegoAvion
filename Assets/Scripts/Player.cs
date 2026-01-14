using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
  
  [Header("Physics")]
  [SerializeField] private Vector2 speed = new Vector2(5f, 8f);

  [Header("Sprites")]
  [SerializeField] private Sprite spriteNormal;
  [SerializeField] private Sprite spriteTurnLeft01;
  [SerializeField] private Sprite spriteTurnLeft02;
  [SerializeField] private Sprite spriteTurnRight01;
  [SerializeField] private Sprite spriteTurnRight02;

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
  }

  void Update()
  {
    GetInputs();
    UpdatePhysics();
    UpdateGraphics();
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
    
    //reset turning time when the plater start moving
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
