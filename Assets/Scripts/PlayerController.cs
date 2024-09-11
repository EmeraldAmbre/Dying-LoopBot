using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    


    private float _moveInput;
    private bool _facingRight = false;
    private bool _isGrounded;
    
    public Transform groundCheck;
    
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider;
    private Animator _animator;

    [SerializeField] GameManager _gameManager;
    [SerializeField] LayerMask _platformLayerMask;
    
    [SerializeField] PlayerManager _playerManager;

    // Basic player movement parameters
    private float _movingSpeed = 5;
    private float _jumpForce = 12f;
    private float _jumpAirHandlingForce = 2.4f;
    private bool _hasJump = false;

    // Gravity parameters
    private float _initGravityScale = 5.35f;
    private float _currentGravityScale => _rigidbody.gravityScale;
    private float _gravityScaleOnFall = 3.5f;

    // Parameter for coyote jump and jump buffering 
    private float _jumpBufferTime = 0.14f;
    private float _currentJumpBufferTime = 0;

    private float _coyoteTime = 0.14f;
    private float _currentCoyoteTime = 0;
    private bool _hasStartedCoyoteTimer = false;

   // Data to clamp velocity 
    private float _xMinVelocity = -1000;
    private float _xMaxVelocity = 1000;

    private float _yMinVelocity = -1000;
    private float _yMaxVelocity = 1000;



    void Start() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        if (_gameManager == null) { _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); }
        if (_playerManager == null) { _playerManager = GetComponent<PlayerManager>(); }

        _rigidbody.gravityScale = _initGravityScale;

        Debug.Log("_rigidbody.gravityScale" + _rigidbody.gravityScale);
    }
    
    private void FixedUpdate() 
    {
        CheckGround();
        Debug.Log("Is grounded: " + _isGrounded);
    }

    void Update() {

        if (!_playerManager.m_deathState)
        {

            if (Input.GetButton("Horizontal"))
            {

                _moveInput = Input.GetAxis("Horizontal");
                Vector3 direction = transform.right * _moveInput;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, _movingSpeed * Time.deltaTime);
                _animator.SetInteger("playerState", 1); // Turn on run animation

            }

            else
            {

                if (_isGrounded) _animator.SetInteger("playerState", 0); // Turn on idle animation

            }

            HandleJumpBuffering();

            HandleCoyoteJump();

            HandleJump();

            HandleGravityChanges();

            ClampVelocity();

            if (!_isGrounded) _animator.SetInteger("playerState", 2); // Turn on jump animation

            if ((!_facingRight && _moveInput > 0) || (_facingRight && _moveInput < 0)) Flip();

        }
    }

    private void Flip() {
        
        _facingRight = !_facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    
    }
    
    private void CheckGround() {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
        float extraHeightTest = 0.1f;
        RaycastHit2D raycastHitGround = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size - new Vector3(0.5f, 0f, 0f), 0f, Vector2.down, extraHeightTest, _platformLayerMask);
        Color raycastGroundColor = Color.blue;
        Debug.DrawRay(_collider.bounds.center + new Vector3(_collider.bounds.extents.x, 0), Vector2.down * (_collider.bounds.extents.y + extraHeightTest), raycastGroundColor);
        Debug.DrawRay(_collider.bounds.center - new Vector3(_collider.bounds.extents.x, 0), Vector2.down * (_collider.bounds.extents.y + extraHeightTest), raycastGroundColor);
        Debug.DrawRay(_collider.bounds.center - new Vector3(_collider.bounds.extents.x, _collider.bounds.extents.y + extraHeightTest), Vector2.right * (_collider.bounds.extents.x) * 2, raycastGroundColor);

        _isGrounded = raycastHitGround.collider != null;
    
    }

    #region Rigibody modification related methods
    private void ClampVelocity()
    {
        Vector2 velocity = _rigidbody.velocity;

        velocity.x = Mathf.Clamp(velocity.x, _xMinVelocity, _xMaxVelocity);
        velocity.y = Mathf.Clamp(velocity.y, _yMinVelocity, _yMaxVelocity);

        _rigidbody.velocity = velocity;
    }

    private void HandleGravityChanges()
    {
        // Increase gravity when the player fall, otherwise will set its normal gravity
        if (!_isGrounded && _rigidbody.velocity.y <= 0 && _rigidbody.gravityScale != _gravityScaleOnFall)
        {
            _rigidbody.gravityScale = _gravityScaleOnFall;
        }
        else if (!_isGrounded && _rigidbody.velocity.y > 0 && _rigidbody.gravityScale != _initGravityScale)
        {
            _rigidbody.gravityScale = _initGravityScale;
        }
    } 
    #endregion

    #region Jump related methods
    private void HandleJumpBuffering()
    {
        _currentJumpBufferTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && !_isGrounded)
        {
            _currentJumpBufferTime = _jumpBufferTime;
        }

        if (_currentJumpBufferTime > 0 && _isGrounded)
        {
            Jump();
            Debug.Log("Buffer jump");
        }
    }

    private void HandleJump()
    {
        if (_isGrounded && !_hasJump) _hasJump = true;

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            Jump();
        }
        else if (Input.GetKey(KeyCode.Space) && !_isGrounded && _hasJump && _rigidbody.velocity.y > 0)
        {
            _rigidbody.AddForce(transform.up * _jumpAirHandlingForce, ForceMode2D.Force);
        }
    }

    private void Jump()
    {
        // _rigidbody.totalForce = Vector2.zero;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
        _isGrounded = false;
        _hasJump = true;
    }

    private void HandleCoyoteJump()
    {
        // Start corote timer when player leave the ground
        if (!_isGrounded && !_hasStartedCoyoteTimer && _rigidbody.velocity.y < 0 && _currentCoyoteTime == 0)
        {
            _currentCoyoteTime = _coyoteTime;
            _hasStartedCoyoteTimer = true;
        }

        // Check jump on coyote timer on
        if (Input.GetKeyDown(KeyCode.Space) && _currentCoyoteTime > 0)
        {
            Jump();
            Debug.Log("Coyote jump");
        }

        // Reset coyotetimer on ground
        if (_isGrounded && _hasStartedCoyoteTimer)
        {
            _hasStartedCoyoteTimer = false;
        }

        if (_isGrounded)
        {
            _currentCoyoteTime = 0;
        }
        else
        {
            _currentCoyoteTime -= Time.deltaTime;
        }
    } 
    #endregion

}

