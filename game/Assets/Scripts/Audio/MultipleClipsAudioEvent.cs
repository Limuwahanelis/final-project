using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Event/MultipleClipsEvent")]
public class MultipleClipsAudioEvent : AudioEvent
{
    public AudioClip[] audioclips;


    public override void Play(AudioSource audioSource)
    {
        audioSource.clip = audioclips[Random.Range(0, audioclips.Length)];
        audioSource.volume = volume * GlobalAudioManager.instance.globalVolume;
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }
}