using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Event/SingleClipEvent")]
public class SingleClipAudioEvent : AudioEvent
{
    public AudioClip audioClip;
    public bool test;
    public override void Play(AudioSource audioSource)
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume * (test? 1 : GlobalAudioManager.instance.globalVolume);
        audioSource.pitch = pitch;
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }

}
