using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour {

    [SerializeField] GameObject _linkedPadlock;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            PadlockManager padlockManager = _linkedPadlock.GetComponent<PadlockManager>();
            padlockManager.m_hasKey = true;
            Destroy(gameObject, 0.2f);
        }
    }

}
