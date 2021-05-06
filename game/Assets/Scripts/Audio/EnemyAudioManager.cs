using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField]
    AudioEvent walkingAudioEvent;
    [SerializeField]
    AudioEvent combatAudioEvent;

    Enemy enemy;
    PlayerCombat combat;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        combat = GetComponent<PlayerCombat>();
        enemy = GetComponent<Enemy>();
        enemy.OnAttackEvent += PlayAttackSound;
        //combat.OnAttackEvent += PlayAttackSound;
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
}
