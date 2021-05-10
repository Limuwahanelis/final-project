using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioManager : MonoBehaviour
{

    AudioSource audioSource;
    [SerializeField]
    AudioEvent walkingAudioEvent;
    [SerializeField]
    AudioEvent combatAudioEvent;
    [SerializeField]
    AudioEvent airAttackAudioEvent;

    Player player;
    PlayerCombat combat;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        combat = GetComponent<PlayerCombat>();
        player = GetComponent<Player>();
        player.OnWalkEvent += PlayWalkingSound;
        combat.OnAttackEvent += PlayAttackSound;
        combat.OnAirAttackEvent += PlayAirAttackSound;
    }

    // Update is called once per frame
    void Update()
    {

    }

     void PlayWalkingSound()
    {
        walkingAudioEvent.Play(audioSource);
    }
    void PlayAttackSound()
    {
        combatAudioEvent.Play(audioSource);
    }
    void PlayAirAttackSound()
    {
        airAttackAudioEvent.Play(audioSource);
    }

    void OnDestroy()
    {
        player.OnWalkEvent -= PlayWalkingSound;
        combat.OnAttackEvent -= PlayAttackSound;
        combat.OnAirAttackEvent -= PlayAirAttackSound;
    }
    
}
