using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public bool m_deathState;

    private Rigidbody2D _rigidbody;
    private Animator _animator;


    [SerializeField] private AudioClip[] _listCoinCollectedAudioClip;
    [SerializeField] private AudioClip[] _listDeathAudioClip;
    [SerializeField] private AudioClip[] _listDeathFreezedAudioClip;


    [SerializeField] GameManager _gameManager;

    void Awake() {

        m_deathState = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        if (_gameManager == null) { _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); }

    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.tag == "Coin") {

            _gameManager.m_coinsCounter += 1;
            Destroy(other.gameObject);

            AudioManager.Instance.RandomSoundEffect(_listCoinCollectedAudioClip ,1);

        }

        else if (other.gameObject.tag == "Bullet") {

            BulletManager bulletManager = other.gameObject.GetComponent<BulletManager>();

            if (bulletManager.m_isIceBullet) _gameManager.m_freezeTest = true;

            _gameManager.m_deathsCounter += 1;
            m_deathState = true;
            if(_gameManager.m_freezeTest) AudioManager.Instance.RandomSoundEffect(_listDeathFreezedAudioClip, 2);
            else AudioManager.Instance.RandomSoundEffect(_listDeathAudioClip, 2);

        }
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.tag == "Enemy") {

            _gameManager.m_deathsCounter += 1;
            m_deathState = true;

            AudioManager.Instance.RandomSoundEffect(_listDeathAudioClip ,2); ;

        }
        else if (other.gameObject.tag == "FreezeEnemy")
        {
            _gameManager.m_freezeTest = true;
            _gameManager.m_deathsCounter += 1;
            m_deathState = true;

            AudioManager.Instance.RandomSoundEffect(_listDeathFreezedAudioClip ,2);

        }
    }

}
