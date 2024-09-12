using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private AudioSource _audioSource;

    public AudioClip m_bumpSFX;
    public AudioClip m_doorOpenSFX;
    public AudioClip m_enemyShootSFX;
    public AudioClip m_jumpSFX;
    public AudioClip m_playerCollectKeySFX;
    public AudioClip m_playerDieSFX;
    public AudioClip m_playerReappearSFX;
    public AudioClip m_projectileHitSFX;
    public AudioClip m_iceProjectileHitSFX;
    public AudioClip m_iceProjectileHitPlayerSFX;
    public AudioClip m_victorySFX;

    void Start() {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip) {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
