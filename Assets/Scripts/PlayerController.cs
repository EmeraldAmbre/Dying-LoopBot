using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    


    private float _moveInput;
    private bool _isGrounded;
    private float _direction = 1;


    public Transform groundCheck;
    
    private Rigidbody2D _rigidbody;
    [SerializeField] private BoxCollider2D _groundCollider;
    private Animator _animator;

    [SerializeField] GameManager _gameManager;
    [SerializeField] LayerMask _platformLayerMask;
    
    [SerializeField] PlayerManager _playerManager;

    // Basic player movement parameters
    private float _movingSpeed = 5;
    private float _jumpForce = 12f;
    private float _jumpAirHandlingForce = 33f;
    private bool _hasJump = false;

    private bool _isJumpTriggered = false;
    private bool _isJumpgHanndlingTriggered = false;

    // Gravity parameters
    private float _initGravityScale = 5.35f;
    private float _currentGravityScale => _rigidbody.gravityScale;
    private float _gravityScaleOnFall = 3.5f;

    // Parameter for coyote jump and jump buffering 
    private float _jumpBufferTime = 0.14f;
    private float _currentJumpBufferTime = 0;

    private float _coyoteTime = 0.13f;
    private float _currentCoyoteTime = 0.0f;
    private bool _hasStartedCoyoteTimer = false;

   // Data to clamp velocity 
    private float _xMinVelocity = -20;
    private float _xMaxVelocity = 20;

    private float _yMinVelocity = -13;
    private float _yMaxVelocity = 20;



    void Start() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        if (_gameManager == null) { _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); }
        if (_playerManager == null) { _playerManager = GetComponent<PlayerManager>(); }

        _rigidbody.gravityScale = _initGravityScale;
    }
    
    private void FixedUpdate() 
    {
        CheckGround();

        if(_isJumpTriggered) 
        {
            Jump();
            _isJumpTriggered = false;
        }

        if(_isJumpgHanndlingTriggered)
        {
            _rigidbody.AddForce(transform.up * _jumpAirHandlingForce, ForceMode2D.Force);
        }

        ClampVelocity();
        Debug.Log("_rigidbody.velocity : " + _rigidbody.velocity);



        if (_direction == 1 || _direction == -1)
        {
            _rigidbody.velocity = new Vector2(_movingSpeed * _direction, _rigidbody.velocity.y);
        }
        else
        {
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);

        }

    }

    void Update() {



        if (!_playerManager.m_deathState)
        {
            HandleWalkMovement();

            HandleJumpBuffering();

            HandleCoyoteJump();

            HandleJump();

            HandleGravityChanges();

            if (!_isGrounded) _animator.SetInteger("playerState", 2); // Turn on jump animation

            HandleFlip();

        }
    }

    private void HandleWalkMovement()
    {
        if (Input.GetButton("Horizontal"))
        {
            _moveInput = Input.GetAxis("Horizontal");
            if (_moveInput > 0) _direction = 1;
            else if (_moveInput < 0) _direction = -1;
            _animator.SetInteger("playerState", 1); // Turn on run animation
        }
        else
        {
            if (_isGrounded) _animator.SetInteger("playerState", 0); // Turn on idle animation
            _direction = 0;
        }
    }

    private void HandleFlip() {
        Vector3 Scaler = transform.localScale;
        if (_direction > 0) Scaler.x = Mathf.Abs(Scaler.x);
        else if (_direction < 0) Scaler.x = -Mathf.Abs(Scaler.x);
        transform.localScale = Scaler;
    
    }
    
    private void CheckGround() {
        float extraHeightTest = 0.1f;
        RaycastHit2D raycastHitGround = Physics2D.BoxCast(_groundCollider.bounds.center, _groundCollider.bounds.size - new Vector3(0.1f, 0f, 0f), 0f, Vector2.down, extraHeightTest, _platformLayerMask);
        Color raycastGroundColor = Color.blue;
        Debug.DrawRay(_groundCollider.bounds.center + new Vector3(_groundCollider.bounds.extents.x, 0), Vector2.down * (_groundCollider.bounds.extents.y + extraHeightTest), raycastGroundColor);
        Debug.DrawRay(_groundCollider.bounds.center - new Vector3(_groundCollider.bounds.extents.x, 0), Vector2.down * (_groundCollider.bounds.extents.y + extraHeightTest), raycastGroundColor);
        Debug.DrawRay(_groundCollider.bounds.center - new Vector3(_groundCollider.bounds.extents.x, _groundCollider.bounds.extents.y + extraHeightTest), Vector2.right * (_groundCollider.bounds.extents.x) * 2, raycastGroundColor);

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
            _isJumpTriggered = true; 
            _isGrounded = false;
            _hasJump = true;
        }
    }

    private void HandleJump()
    {
        if (_isGrounded && _hasJump) _hasJump = false;

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _isJumpTriggered = true;
            _isGrounded = false;
            _hasJump = true;
        }
        else if (Input.GetKey(KeyCode.Space) && !_isGrounded && _hasJump && _rigidbody.velocity.y > 0)
        {
            _isJumpgHanndlingTriggered = true;
        }
        else
        {
            _isJumpgHanndlingTriggered = false;
        }
    }

    private void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x,0);
        _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
        _isGrounded = false;
        _hasJump = true;
        _currentCoyoteTime = 0;
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
        if (Input.GetKeyDown(KeyCode.Space) && _currentCoyoteTime > 0 && !_hasJump)
        {
            _isJumpTriggered = true;
            _isGrounded = false;
            _hasJump = true;
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
        else if(_hasStartedCoyoteTimer)
        {
            _currentCoyoteTime = _currentCoyoteTime - Time.deltaTime;
        }
    } 
    #endregion

}

