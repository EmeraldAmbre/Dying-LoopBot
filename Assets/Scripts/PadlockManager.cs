using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PadlockManager : MonoBehaviour {

    BoxCollider2D _boxCollider2D;
    Animator _animator;
    bool _isOpen;

    public bool m_hasKey = false;

    void Start() {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _isOpen = false;
        m_hasKey = false;

        _boxCollider2D.isTrigger = false;
    }

    void Update() {
        if (m_hasKey) { _boxCollider2D.isTrigger = true; }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && m_hasKey) {
            _isOpen = true;
            _animator.SetBool("IsOpen", _isOpen);
        }
    }

}
