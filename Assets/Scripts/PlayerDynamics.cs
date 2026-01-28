
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

// Asegura que el objeto tiene Rigidbody y SpriterRenderer (si no las crea)
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
    public class PlayerDynamics : MonoBehaviour
    {
        public enum LateralDirection
        {
            None = 0,
            Left = -1,
            Right = 1
        }
        [Header("Physics")]
        [SerializeField] private Vector2 speed = new Vector2(5f, 8f);

        [Header("Sprites")]
        [SerializeField] private Sprite spriteNormal;
        [SerializeField] private Sprite spriteTurnLeft01;
        [SerializeField] private Sprite spriteTurnLeft02;
        [SerializeField] private Sprite spriteTurnRight01;
        [SerializeField] private Sprite spriteTurnRight02;

        [Header("Scroll")] 
        [SerializeField] private bool enableCameraFollow = true;
        [SerializeField] private Camera mainCamera;
        
        [Header("Scroll")] 
        [SerializeField] private GameObject firePrefab;
        
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _direction;
        private float _timeTurning;
        private LateralDirection _lastLateralDirection = LateralDirection.None;
    
        // Laterals limits for camera follow
        private Transform _cameraLimitLeft;
        private Transform _cameraLimitRight;

        //Pool

        [SerializeField] private MisilPool pool;
        private void Awake()
        {
            //get references
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer =GetComponent<SpriteRenderer>();
            
            // register the player in the GameManager
            GameManager.Instance.Player
                = this.gameObject;
        }

        private void Start()
        {
            // get the main camera if not set
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            // get the camera limits from the GameManager
            _cameraLimitLeft = GameManager.Instance.CameraLimitLeft.transform;
            _cameraLimitRight = GameManager.Instance.CameraLimitRight.transform;
            UpdateCamera();
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
            //Habilitar que la camera siga a Player
            if (enableCameraFollow)
            {
                if (mainCamera)
                {
                    // Copiar la posición actual de la camara
                    Vector3 cameraPosition = mainCamera.transform.position;
                    // Posicion del Player
                    cameraPosition.y = transform.position.y;
                    // verify lateral limits
                    if (_cameraLimitLeft && _cameraLimitRight)
                    {
                        // check if the future camera position is inside the lateral limits
                        // obtener el centro de la camera
                        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
                        // Si la camera sale de los limites, se asigna la ultima posicion que tuvo
                        if ((transform.position.x - cameraHalfWidth) > _cameraLimitLeft.position.x
                            && (transform.position.x + cameraHalfWidth) < _cameraLimitRight.position.x)
                        {
                            cameraPosition.x = transform.position.x;
                        }
                    }
                    else
                    {
                        cameraPosition.x = transform.position.x;
                    }
                    mainCamera.transform.position = cameraPosition;
                }
            }
        }
        
        private void UpdateGraphics()
        {
            // sprite representation switch turnning time
            if (_timeTurning >= 0.5f)
            {
                // set sprite right or left depending on the lateral desired direction sign
                if (_direction.x == 0)
                {
                    // use the last lateral direction
                    _spriteRenderer.sprite = (_lastLateralDirection == LateralDirection.Right) ? spriteTurnRight02 : spriteTurnLeft02;
                }
                else
                {
                    _spriteRenderer.sprite = (_direction.x > 0) ? spriteTurnRight02 : spriteTurnLeft02;
                }
            }
            else if (_timeTurning > 0f)
            {
                // set sprite right or left depending on the lateral desired direction sign
                if (_direction.x == 0)
                {
                    // use the last lateral direction
                    _spriteRenderer.sprite = (_lastLateralDirection == LateralDirection.Right) ? spriteTurnRight01 : spriteTurnLeft01;
                }
                else
                {
                    _spriteRenderer.sprite = (_direction.x > 0) ? spriteTurnRight01 : spriteTurnLeft01;
                }
            }
            else
            {
                _spriteRenderer.sprite = spriteNormal;
            }
        }
        
        private void UpdatePhysics()
        {
            // limit the lateral movement of the player (when camera is in the lateral limits) to stop the player at the screen edge
            // Calcular la mitad del ancho del sprite del player
            float halfWidth = _spriteRenderer.bounds.extents.x;
            // Calcular la mitad del ancho de la camera
            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            // simulate lateral movement at linear velocity
            Vector3 pos = transform.position + _direction.x * speed.x * Time.deltaTime * Vector3.right;
            Vector3 newDirection= _direction;
            if ((pos.x < mainCamera.transform.position.x - cameraHalfWidth + halfWidth) && _direction.x < 0)
            {
                newDirection.x = 0;
            }
            if ((pos.x > mainCamera.transform.position.x + cameraHalfWidth - halfWidth) && _direction.x > 0)
            {
                newDirection.x = 0;
            }
            // apply linear velocity
            _rigidbody2D.linearVelocity = new Vector2(newDirection.x * speed.x, newDirection.y * speed.y);
    
            // update the "turning time" of the plane if the "lateral speed direction" [originalDirection vs real speed] (in any direction)
            // is greater than a minimum threshold (0.1) to avoid flat plane turning when the player is on the screen edge
            if (Mathf.Abs(_direction.x) > 0.1f)
            {
                _timeTurning += Time.deltaTime;
                _timeTurning = Mathf.Min(_timeTurning, 0.5f);
            }
            else
            {
                // return to the horizontal position (slowly)
                _timeTurning -= Time.deltaTime * 4f;
                _timeTurning = Mathf.Max(_timeTurning, 0f);
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
                // Estabilizar avión al dejar de pulsar dirección izquierda
                _lastLateralDirection = LateralDirection.Left;
            }

            if (Input.GetKey(KeyCode.D))
            {
                _direction += Vector2.right;
                // Estabilizar avión al dejar de pulsar dirección derecha
                _lastLateralDirection = LateralDirection.Right;
            }
    
            // reset turning time when the player start moving
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                _timeTurning = 0;
            }
    
            // player fire shot
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
            }
            
            // for debug only (fast forward and fast backward)
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    _direction += Vector2.up * 10f;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    _direction += Vector2.down * 10f;
                }
            }
        }

        private void Fire()
        {
            // Create a new GameObject firePrebab
            //GameObject fire = Instantiate(firePrefab);
            //fire.transform.position = transform.position;
            GameObject fire = pool.GetBullet();
            if (fire)
            {
                fire.transform.position = transform.position;
            }
        }
    }
