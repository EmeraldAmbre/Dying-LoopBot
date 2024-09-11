using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public bool m_freezeTest;
    
    public int m_coinsCounter = 0;
    public int m_deathsCounter = 0;
    
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _deathPlayerPrefab;
    [SerializeField] PlayerManager _playerManager;
    [SerializeField] GameObject _activeCheckpoint;

    [SerializeField] Text _coinText;
    [SerializeField] Text _craneText;

    [SerializeField] GameObject _activePlayer;

    void Start() {

        _activePlayer = Instantiate(_playerPrefab, _activeCheckpoint.transform.position, _activeCheckpoint.transform.rotation);
        _playerManager = _activePlayer.GetComponent<PlayerManager>();
    
    }
    
    void Update() {
        
        _coinText.text = m_coinsCounter.ToString();
        _craneText.text = m_deathsCounter.ToString();
        
        if (_playerManager.m_deathState) {

            _activePlayer.SetActive(false);
            GameObject deadPlayer = Instantiate(_deathPlayerPrefab, _activePlayer.transform.position, _activePlayer.transform.rotation);
            deadPlayer.transform.localScale = new Vector3(_playerPrefab.transform.localScale.x, _playerPrefab.transform.localScale.y, _playerPrefab.transform.localScale.z);
            
            if (m_freezeTest) {

                DeadPlayerManager deadPlayerManager = deadPlayer.GetComponent<DeadPlayerManager>();
                deadPlayerManager.Freeze();
                m_freezeTest = false;

            }

            Respawn();
        
        }
    }
    
    private void Respawn() {

        _activePlayer = Instantiate(_playerPrefab, _activeCheckpoint.transform.position, _activeCheckpoint.transform.rotation);
        _playerManager = _activePlayer.GetComponent<PlayerManager>();

    }

    public void ChangeActiveCheckpoint(GameObject cp) {

        _activeCheckpoint = cp;

    }

}
