using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    [SerializeField] float _movingSpeed;
    [SerializeField] float _jumpForce;
    
    private float _moveInput;
    private bool _facingRight = false;
    private bool _isGrounded;
    
    public Transform groundCheck;
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    [SerializeField] GameManager _gameManager;
    [SerializeField] PlayerManager _playerManager;

    void Start() {
        
        _rigidbody = GetComponent<Rigidbody2D>();
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

            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) {

                _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);

            }

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
        _isGrounded = colliders.Length > 1;
    
    }

}

