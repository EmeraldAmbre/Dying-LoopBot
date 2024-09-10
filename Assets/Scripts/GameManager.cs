using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    
    public int m_coinsCounter = 0;
    public int m_deathsCounter = -1;
    
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _deathPlayerPrefab;
    [SerializeField] GameObject _activeCheckpoint;
    [SerializeField] PlayerManager _playerManager;

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
            GameObject deathPlayer = Instantiate(_deathPlayerPrefab, _activePlayer.transform.position, _activePlayer.transform.rotation);
            deathPlayer.transform.localScale = new Vector3(_playerPrefab.transform.localScale.x, _playerPrefab.transform.localScale.y, _playerPrefab.transform.localScale.z);
            Respawn();
        
        }
    }
    
    private void Respawn() {

        _activePlayer = Instantiate(_playerPrefab, _activeCheckpoint.transform.position, _activeCheckpoint.transform.rotation);
        _playerManager = _activePlayer.GetComponent<PlayerManager>();

    }

}
