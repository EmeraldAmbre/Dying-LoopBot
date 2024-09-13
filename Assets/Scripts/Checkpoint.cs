using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour {

    [SerializeField] GameManager _gameManager;
    public bool _isFinalCheckpoint;
    [SerializeField] string _sceneTransitionName;
    [SerializeField] float _sceneTransitionDelay = 1f;

    public bool m_isActive;

    private GameObject[] _allCheckpoints;

    void Start() {

        m_isActive = false;

        if (_gameManager == null) { _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); }

    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.tag == "Player") {

            if (_isFinalCheckpoint) {

                StartCoroutine(LoadSceneWithDelay(_sceneTransitionDelay, _sceneTransitionName));
                _gameManager.ChangeActiveCheckpoint(gameObject);
                return;

            }

            _allCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint");

            foreach (var checkpoint in _allCheckpoints) {

                Checkpoint manager = checkpoint.GetComponent<Checkpoint>();
                manager.m_isActive = false;

               

            }

            m_isActive = true;

            _gameManager.ChangeActiveCheckpoint(gameObject);

        }
    }

    private IEnumerator LoadSceneWithDelay(float delay, string sceneName) {

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);

    }
}
