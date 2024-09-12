using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperManager : MonoBehaviour {

    [SerializeField] float _bumpForce = 10;

    Rigidbody2D _playerRigidbody;
    Rigidbody2D _corpseRigidbody;
    PlayerManager _playerManager;

    void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.tag == "Player") {

            _playerManager = other.gameObject.GetComponent<PlayerManager>();
            _playerRigidbody = other.gameObject.GetComponent<Rigidbody2D>();

            if (_playerManager.m_deathState == false) {
                _playerRigidbody.velocity = Vector3.zero;
                _playerRigidbody.AddForce(Vector2.up * _bumpForce, ForceMode2D.Impulse);
            }
        }

        else if (other.gameObject.tag == "Corpse") {

            _corpseRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
            _corpseRigidbody.velocity = Vector3.zero;
            _corpseRigidbody.AddForce(Vector2.up * _bumpForce, ForceMode2D.Impulse);

        }
    }
}
