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
    [SerializeField] GameObject _deathFreezedPlayerPrefab;
    [SerializeField] PlayerManager _playerManager;
    [SerializeField] GameObject _activeCheckpoint;

    [SerializeField] Text _coinText;
    [SerializeField] Text _craneText;

    [SerializeField] GameObject _activePlayer;
    
    [SerializeField] private int _nb_DeadPlayersLimit = 2;

    private List<GameObject> _listDeadPlayers = new List<GameObject>();

    [SerializeField] private AudioClip _musicToPlay;
    [SerializeField] private AudioClip[] _listRespawnAudioClip;
    [SerializeField] private AudioClip[] _listResetDyingBodiesAudioClip;
    [SerializeField] private AudioClip[] _listCheckpointAudioClip;

    void Start() {

        _activePlayer = Instantiate(_playerPrefab, _activeCheckpoint.transform.position, _activeCheckpoint.transform.rotation);
        _playerManager = _activePlayer.GetComponent<PlayerManager>();


        if (_musicToPlay != null) AudioManager.Instance.PlayMusic(_musicToPlay);

        UpdateUI();

    }
    
    void Update()
    {
        if (_playerManager.m_deathState)
        {
            InstantiateDeadPlayer();

            Respawn();

            UpdateUI();

        }

        HandleResetDeadPlayerList();

        HandleManualRespawn();

    }

    private void HandleResetDeadPlayerList()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            AudioManager.Instance.RandomSoundEffect(_listResetDyingBodiesAudioClip,3);
            Debug.Log("ResetDeadPlayers");

            foreach (GameObject deadPlayer in _listDeadPlayers)
            {
                Destroy(deadPlayer);
            }
            _listDeadPlayers.Clear();
            UpdateUI();
        }
    }

    private void HandleManualRespawn()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.Instance.RandomSoundEffect(_listRespawnAudioClip, 3);
            _activePlayer.transform.position = _activeCheckpoint.transform.position;
        }
    }

    private void InstantiateDeadPlayer()
    {
        // Instantiate dead player prefab on scene regarding the freeze status
        _activePlayer.SetActive(false);
        GameObject deadPlayer;
        if (m_freezeTest) deadPlayer = Instantiate(_deathFreezedPlayerPrefab, _activePlayer.transform.position, _activePlayer.transform.rotation);
        else deadPlayer = Instantiate(_deathPlayerPrefab, _activePlayer.transform.position, _activePlayer.transform.rotation);
        deadPlayer.transform.localScale = new Vector3(_playerPrefab.transform.localScale.x, _playerPrefab.transform.localScale.y, _playerPrefab.transform.localScale.z);
        m_freezeTest = false;
        _playerManager.m_deathState = false;
        // Add the dead player to the list
        _listDeadPlayers.Add(deadPlayer);
        if (_listDeadPlayers.Count > _nb_DeadPlayersLimit)
        {
            Destroy(_listDeadPlayers[0]);
            _listDeadPlayers.RemoveAt(0);
        }
    }

    private void UpdateUI()
    {
        _coinText.text = m_coinsCounter.ToString();
        string crane_nb = _listDeadPlayers.Count.ToString() + " / " + _nb_DeadPlayersLimit.ToString();
        Debug.Log(crane_nb);
        _craneText.text = crane_nb;
    }

    private void Respawn() {

        _activePlayer = Instantiate(_playerPrefab, _activeCheckpoint.transform.position, _activeCheckpoint.transform.rotation);
        _playerManager = _activePlayer.GetComponent<PlayerManager>();

    }

    public void ChangeActiveCheckpoint(GameObject cp) {

        if(_activeCheckpoint != cp)
        {
            AudioManager.Instance.RandomSoundEffect(_listCheckpointAudioClip, 3);
            _activeCheckpoint = cp;
        }

    }

}
