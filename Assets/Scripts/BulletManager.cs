using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    public bool m_isGoingRight;
    public float m_lifeTime = 10f;

    private float _timer;
    private float _speed = 2f;

    private bool _rotated;

    public bool m_isIceBullet;
    [SerializeField] private ParticleSystem particules;

    void Awake() {

        _timer = m_lifeTime;
        _rotated = false;

    }

    void Update() {

        _timer -= Time.deltaTime;

        if (!_rotated) {

            //if (m_isGoingRight) transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            //else transform.localRotation = Quaternion.Euler(0f, 0f, 180f);

            if (!m_isGoingRight) transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            

            _rotated = true;

        }

        if (_timer > 0) {

            if (m_isGoingRight) {

                transform.Translate(Vector2.right * _speed * Time.deltaTime);

            }

            else {

                transform.Translate(Vector2.left * _speed * Time.deltaTime);

            }

        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {

        Destroy(gameObject);
        
    }

    private void OnDestroy()
    {
        Destroy(particules.gameObject, 2f);
    }

}
