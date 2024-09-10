using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [SerializeField] GameManager _gameManager;

    public bool m_isActive;

    private GameObject[] _allCheckpoints;

    void Start() {

        m_isActive = false;

        if (_gameManager == null) { _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); }

    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.tag == "Player") {

            _allCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint");

            foreach (var checkpoint in _allCheckpoints)
            {

                Checkpoint manager = checkpoint.GetComponent<Checkpoint>();
                manager.m_isActive = false;

            }

            m_isActive = true;

            _gameManager.ChangeActiveCheckpoint(gameObject);

        }
    }

}
