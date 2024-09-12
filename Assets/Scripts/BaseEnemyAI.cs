using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAI : MonoBehaviour {
    
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] bool _isStatic = false;
    [SerializeField] float _moveLoopDuration = 2f;

    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _hitBox;
    private float _timer;
    private Vector2 _initPosition;

    void Start() {
        
        _rigidbody = GetComponent<Rigidbody2D>();
        _hitBox = GetComponent<CapsuleCollider2D>();
        _timer = _moveLoopDuration;
        _initPosition = transform.position;

    }
    
    void Update() {
        // When player hit the button to respawn
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = _initPosition;
            _rigidbody.velocity = Vector2.zero;
        }

        if (!_isStatic) {

            _rigidbody.velocity = new Vector2(_moveSpeed, _rigidbody.velocity.y);
            _timer -= Time.deltaTime;

            if (_timer <= 0) {

                Flip();
                _timer = _moveLoopDuration;

            }

        }
    }
    
    private void Flip() {
        
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        _moveSpeed *= -1;

    }

}
