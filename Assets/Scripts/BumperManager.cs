using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperManager : MonoBehaviour {

    [SerializeField] float _bumpForce = 10;

    Rigidbody2D _playerRigidbody;
    PlayerManager _playerManager;

    void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.tag == "Player") {

            _playerManager = other.gameObject.GetComponent<PlayerManager>();
            _playerRigidbody = other.gameObject.GetComponent<Rigidbody2D>();

            if (_playerManager.m_deathState == false) {
                _playerRigidbody.AddForce(Vector2.up * _bumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
