using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BossAudioManager : MonoBehaviour
{

    public SingleClipAudioEvent bossMissileAudio;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        Boss.OnBossMissileAttack += PlayBossMissileSound;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBossMissileSound()
    {
        bossMissileAudio.Play(source);
    }

    private void OnDestroy()
    {
        Boss.OnBossMissileAttack -= PlayBossMissileSound;
    }
}
