using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class MusicSlider : MonoBehaviour
{
    Slider volumeSlider;
    GlobalAudioManager audioMan;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
        audioMan = GlobalAudioManager.instance;
        LoadSettings();
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
    public void LoadSettings()
    {
        if (!File.Exists(Application.persistentDataPath + "/settings.json")) return;

        string json = File.ReadAllText(Application.persistentDataPath + "/settings.json");
        SettingsData loadedSettings = JsonUtility.FromJson<SettingsData>(json);

        volumeSlider.value = loadedSettings.globalVolume;
    }
}
