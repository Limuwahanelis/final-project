using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PlayerOptionsSaver : MonoBehaviour
{
    Settings settings;
    GlobalAudioManager audioMan;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        settings = Settings.instance;
        audioMan = GlobalAudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveSettings()
    {
        Debug.Log(settings.GetResolution());
        SettingsData settingsData = new SettingsData(settings.GetResolution(), audioMan.globalVolume, settings.GetFullScreen());
        string json = JsonUtility.ToJson(settingsData);
        Debug.Log(json);
        File.WriteAllText(Application.persistentDataPath + "/settings.json", json);
    }
}
