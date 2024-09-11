using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

    [SerializeField] GameObject _pairedPortal;

    PortalManager _pairedPortalManager;
    BoxCollider2D _boxCollider2D;
    Vector3 _offset;

    public bool m_isActive;

    void Start() {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _pairedPortalManager = _pairedPortal.GetComponent<PortalManager>();
        m_isActive = true;
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (m_isActive && (other.gameObject.tag == "Player" || other.gameObject.tag == "Corpse")) {

            m_isActive = false;
            _pairedPortalManager.m_isActive = false;
            _offset = transform.position - other.transform.localPosition;
            other.transform.localPosition = _offset + _pairedPortal.transform.position;

        }
    }

    void OnTriggerExit2D(Collider2D other) {

        m_isActive = true;
        
    }
}
