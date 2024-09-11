using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    public bool m_isGoingRight;
    public float m_lifeTime = 10f;

    private float _timer;
    private float _speed = 0.02f;

    private bool _rotated;

    public bool m_isIceBullet;

    void Awake() {

        _timer = m_lifeTime;
        _rotated = false;

    }

    void Update() {

        _timer -= Time.deltaTime;

        if (!_rotated) {

            if (m_isGoingRight) transform.localRotation = Quaternion.Euler(0f, 0f, -90f);

            else transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

            _rotated = true;

        }

        if (_timer > 0) {

            if (m_isGoingRight) {

                transform.Translate(Vector2.up * _speed);

            }

            else {

                transform.Translate(Vector2.up * _speed);

            }

        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {

        Destroy(gameObject);
        
    }

}
