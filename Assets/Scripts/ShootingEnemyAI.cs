using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShootingEnemyAI : MonoBehaviour {

    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] bool _isStatic = true;
    [SerializeField] float _moveLoopDuration = 2f;
    [SerializeField] float _shootLoopDuration = 2f;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] GameObject _shootingPoint;


    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _hitBox;
    private float _timer;
    private float _shootTimer;

    public bool m_facingRight = true;

    void Start() {

        _rigidbody = GetComponent<Rigidbody2D>();
        _hitBox = GetComponent<CapsuleCollider2D>();
        _timer = _moveLoopDuration;
        _shootTimer = _shootLoopDuration;
        _animator = GetComponent<Animator>();

        if (!m_facingRight && _isStatic) StupeFlip();

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

        _shootTimer -= Time.deltaTime;

        if (_shootTimer <= 0) {

            _shootTimer = _shootLoopDuration;
            GameObject bullet = Instantiate(_bulletPrefab, _shootingPoint.transform.position, _shootingPoint.transform.rotation);
            BulletManager bulletManager = bullet.GetComponent<BulletManager>();
            bulletManager.m_isGoingRight = m_facingRight;
            _animator.SetTrigger("isShooting");
            Destroy(bullet, bulletManager.m_lifeTime);

        }
    }

    private void Flip() {

        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        _moveSpeed *= -1;

    }

    private void StupeFlip() {

        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        _moveSpeed = 0;

    }

}
