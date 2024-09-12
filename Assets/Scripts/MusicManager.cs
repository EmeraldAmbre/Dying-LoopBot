using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    private AudioSource _audioSource;

    public AudioClip m_levelMusic1;
    public AudioClip m_levelMusic2;
    public AudioClip m_menuMusic;
    public AudioClip m_victoryMusic;
    public AudioClip m_victoryMusicVariant;

    void Start() {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip clip) {
        _audioSource.clip = clip;
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
