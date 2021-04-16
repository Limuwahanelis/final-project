using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GlobalAudioManager : MonoBehaviour
{
    public Slider volumeSlider;
    public static GlobalAudioManager instance=null;
    [Range(0,1)]
    public float globalVolume;
    void Awake()
    {
        DontDestroyOnLoad(this);
        if(instance==null)
        {
            instance = this;
        }
        else if(instance!=this)
        {
            Destroy(gameObject);
        }
    }

    public void SetGlobalVolume()
    {
        globalVolume = volumeSlider.value;
    }
}
