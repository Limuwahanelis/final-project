using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class BombAudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioEvent explosionSound;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Bomb>().OnExplodeEvent += PlayExplosionSound;
        audioSource = GetComponent<AudioSource>();
    }

    void PlayExplosionSound()
    {
        explosionSound.Play(audioSource);
    }
}
