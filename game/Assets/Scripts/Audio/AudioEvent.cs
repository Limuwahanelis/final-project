using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioEvent : ScriptableObject
{
    [Range(0,1)]
    public float volume;
    [Range(0, 1)]
    public float pitch;

    public abstract void Play(AudioSource audioSource);

}
