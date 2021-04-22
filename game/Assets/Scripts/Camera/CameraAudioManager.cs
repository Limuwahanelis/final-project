using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAudioManager : MonoBehaviour
{
    public AudioEvent mainTheme;
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        GlobalAudioManager.OnGlobalVolumeChange += ChangeVolume;
        source = GetComponent<AudioSource>();
        mainTheme.Play(source);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ChangeVolume(float value)
    {
        Debug.Log("fire");
        source.volume = value;
    }
}
