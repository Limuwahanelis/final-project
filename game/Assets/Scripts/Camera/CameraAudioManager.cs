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
        source = GetComponent<AudioSource>();
        mainTheme.Play(source);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
