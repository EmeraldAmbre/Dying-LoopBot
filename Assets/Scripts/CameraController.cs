using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public float damping = 1.5f; // movement speed
    public Vector2 offset = new Vector2(0f, 0f); // special effect if you want the character to be not in center of screen
    public bool faceLeft; //  mirror reflection of OFFSET along the y axis

    [SerializeField] Transform _playerPosition;
    [SerializeField] GameObject _activePlayer;
    [SerializeField] GameObject[] _allPlayers;
    [SerializeField] PlayerManager _activePlayerManager;
    
    private int _lastX;
    
    void Start () {
        
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);

    }
    
    void FindPlayer(bool playerFaceLeft) {

        _allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in _allPlayers) {

            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            if (playerManager.m_deathState == false) _activePlayer = player;

        }

        _activePlayerManager = _activePlayer.GetComponent<PlayerManager>();
        _playerPosition = _activePlayer.transform;
        _lastX = Mathf.RoundToInt(_playerPosition.position.x);
        
        if (playerFaceLeft) {
            transform.position = new Vector3(_playerPosition.position.x - offset.x, _playerPosition.position.y + offset.y, transform.position.z);
        }
        
        else {
            transform.position = new Vector3(_playerPosition.position.x + offset.x, _playerPosition.position.y + offset.y, transform.position.z);
        }
    }

    void Update () {

        FindPlayer(faceLeft);
        
        if (_playerPosition) {
            
            int currentX = Mathf.RoundToInt(_playerPosition.position.x);

            if (currentX > _lastX) faceLeft = false; else if (currentX < _lastX) faceLeft = true;

            _lastX = Mathf.RoundToInt(_playerPosition.position.x);

            Vector3 target;

            if (faceLeft) {
                target = new Vector3(_playerPosition.position.x - offset.x, _playerPosition.position.y + offset.y, transform.position.z);
            }
            
            else {
                target = new Vector3(_playerPosition.position.x + offset.x, _playerPosition.position.y + offset.y, transform.position.z);
            }
            
            Vector3 currentPosition = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
            
            transform.position = currentPosition;
        }

    } // Update end.

} // Class end.

