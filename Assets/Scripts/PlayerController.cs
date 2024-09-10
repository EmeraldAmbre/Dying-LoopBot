using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    [SerializeField] float _movingSpeed;
    private float _jumpForce = 10;
    
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



    // Timer fo improving game feel
    private float _jumpBufferTime = 0.13f;
    private float _currentJumpBufferTime = 0;

    private float _coyoteTime = 0.13f;
    private float _currentCoyoteTime = 0;
    private bool _hasStartedCoyoteTimer = false;

    void Start() {
        
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        if (_gameManager == null) { _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); }
        if (_playerManager == null) { _playerManager = GetComponent<PlayerManager>(); }
    
    }
    
    private void FixedUpdate() {
        
        CheckGround();
    
    }

    void Update() {

        if (!_playerManager.m_deathState) {

            if (Input.GetButton("Horizontal")) {

                _moveInput = Input.GetAxis("Horizontal");
                Vector3 direction = transform.right * _moveInput;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, _movingSpeed * Time.deltaTime);
                _animator.SetInteger("playerState", 1); // Turn on run animation

            }

            else {

                if (_isGrounded) _animator.SetInteger("playerState", 0); // Turn on idle animation

            }

            HandleJumpBuffering();

            HandleCoyoteTimer();

            HandleJump();


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
        RaycastHit2D raycastHit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size - new Vector3(0.1f, 0f, 0f), 0f, Vector2.down, extraHeightTest, _platformLayerMask);

        Color rayColor = Color.blue;
        Debug.DrawRay(_collider.bounds.center + new Vector3(_collider.bounds.extents.x, 0), Vector2.down * (_collider.bounds.extents.y + extraHeightTest), rayColor);
        Debug.DrawRay(_collider.bounds.center - new Vector3(_collider.bounds.extents.x, 0), Vector2.down * (_collider.bounds.extents.y + extraHeightTest), rayColor);
        Debug.DrawRay(_collider.bounds.center - new Vector3(_collider.bounds.extents.x, _collider.bounds.extents.y + extraHeightTest), Vector2.right * (_collider.bounds.extents.x) * 2, rayColor);



        _isGrounded = raycastHit.collider != null;
    
    }

    private void HandleJumpBuffering()
    {
        _currentJumpBufferTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && !_isGrounded)
        {
            _currentJumpBufferTime = _jumpBufferTime;
        }

        if(_currentJumpBufferTime > 0 && _isGrounded) Jump();
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            Jump();
            Debug.Log("JUMP FROM GROUND");
        }

    }

    private void Jump()
    {
        // _rigidbody.totalForce = Vector2.zero;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
        Debug.Log("Velocity y" + _rigidbody.velocity.y);
        _isGrounded = false;
    }

    private void HandleCoyoteTimer()
    {
        // Start corote timer when player leave the ground
        if(!_isGrounded && !_hasStartedCoyoteTimer && _rigidbody.velocity.y < 0 && _currentCoyoteTime == 0)
        {
            _currentCoyoteTime = _coyoteTime;
            _hasStartedCoyoteTimer = true;
        }

        // Check jump on coyote timer on
        if(Input.GetKeyDown(KeyCode.Space) && _currentCoyoteTime > 0)
        {
            Jump();
            Debug.Log("Coyote jump");
        }

        // Reset coyotetimer on ground
        if(_isGrounded && _hasStartedCoyoteTimer)
        {
            _hasStartedCoyoteTimer = false;
        }

        if(_isGrounded)
        {
            _currentCoyoteTime = 0;
        }
        else
        {
            _currentCoyoteTime -= Time.deltaTime;
        }
        Debug.Log("_currentCoyoteTime  " + _currentCoyoteTime);
    }

}

