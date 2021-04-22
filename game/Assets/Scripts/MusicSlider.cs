using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    Slider volumeSlider;
    GlobalAudioManager audioMan;
    // Start is called before the first frame update
    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        audioMan = GlobalAudioManager.instance;
        SetSlider();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetSlider()
    {
        volumeSlider.value = audioMan.globalVolume;
    }

    public void SetVolume(float value)
    {
        audioMan.SetGlobalVolume(value);
    }
}
