using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAI : MonoBehaviour {
    
    [SerializeField] float moveSpeed = 1f;
    public LayerMask ground;
    public LayerMask wall;
    
    private Rigidbody2D _rigidbody;
    private Collider2D triggerCollider;
    
    void Start() {
        
        _rigidbody = GetComponent<Rigidbody2D>();
    
    }
    
    void Update() {
        
        _rigidbody.velocity = new Vector2(moveSpeed, _rigidbody.velocity.y);
    
    }
    
    void FixedUpdate() {
        
        if(!triggerCollider.IsTouchingLayers(ground) || triggerCollider.IsTouchingLayers(wall))
            {
                Flip();
            }
        }
        
        private void Flip()
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            moveSpeed *= -1;
        }
    }
