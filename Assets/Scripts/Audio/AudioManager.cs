using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// From https://www.eventbrite.be/e/billets-du-reve-a-la-realite-lentrepreneuriat-dans-le-jeu-video-1008437382357 


// Uses :
//  AudioManager.Instance.PlayMusic(BattleMusic); 
//  AudioManager.Instance.RandomSoundEffect(AttackNoises);


public class AudioManager : MonoBehaviour
{
    // Audio players components.
    public AudioSource[] listSfxAudioSourc;
    public AudioSource MusicSource;

    // Random pitch adjustment range.
    public float LowPitchRange = .8f;
    public float HighPitchRange = 1.2f;

    // Singleton instance.
    public static AudioManager Instance = null;

    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    // Play a single clip through the sound effects source.
    public void Play(AudioClip clip, int bus = 0)
    {
        listSfxAudioSourc[bus].clip = clip;
        listSfxAudioSourc[bus].Play();
    }

    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    // Play a random clip from an array, and randomize the pitch slightly.
    public void RandomSoundEffect(AudioClip[] clips, int bus = 0 )
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

        listSfxAudioSourc[bus].pitch = randomPitch;
        listSfxAudioSourc[bus].clip = clips[randomIndex];
        listSfxAudioSourc[bus].Play();
    }

}