using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Event")]
public class AudioEvent : ScriptableObject
{

    public AudioClip[] audioclips;
    [Range(0,1)]
    public float volume;

    public void Play(AudioSource audioSource)
    {
        audioSource.clip = audioclips[Random.Range(0, audioclips.Length)];
        audioSource.volume = volume * GlobalAudioManager.instance.globalVolume;
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }
}
