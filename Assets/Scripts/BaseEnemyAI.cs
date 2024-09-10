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
    
    void Start() {
        
        _rigidbody = GetComponent<Rigidbody2D>();
        _hitBox = GetComponent<CapsuleCollider2D>();
        _timer = _moveLoopDuration;


    }
    
    void Update() {

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
