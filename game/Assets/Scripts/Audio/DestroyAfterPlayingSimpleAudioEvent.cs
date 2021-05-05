using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Event/SingleClipDestroyEvent")]
public class DestroyAfterPlayingSimpleAudioEvent : AudioEvent
{
    public AudioClip audioClip;
    public override void Play(AudioSource audioSource)
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume * GlobalAudioManager.instance.globalVolume;
        audioSource.pitch = pitch;
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }
    
}
