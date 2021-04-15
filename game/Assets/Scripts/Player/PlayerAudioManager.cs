using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioManager : MonoBehaviour
{

    AudioSource audioSource;
    [SerializeField]
    AudioEvent walkingAudioEvent;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GetComponent<Player>();
        player.OnWalkEvent += PlayWalkingSound;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void PlayWalkingSound()
    {
        walkingAudioEvent.Play(audioSource);
    }
}
