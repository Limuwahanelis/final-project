using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
public class GlobalAudioManager : MonoBehaviour
{
    public Slider volumeSlider;
    public static Action<float> OnGlobalVolumeChange;
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
    void Start()
    {
        LoadSettings();
    }
    public void SetGlobalVolume(float value)
    {
        globalVolume = value;
        OnGlobalVolumeChange?.Invoke(value);
    }

    public void LoadSettings()
    {
        if (!File.Exists(Application.persistentDataPath + "/settings.json")) return;

        string json = File.ReadAllText(Application.persistentDataPath + "/settings.json");
        SettingsData loadedSettings = JsonUtility.FromJson<SettingsData>(json);

        globalVolume = loadedSettings.globalVolume;
        volumeSlider.value = globalVolume;
    }
}
