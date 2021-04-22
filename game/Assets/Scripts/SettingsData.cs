using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsData
{
    public int width;
    public int height;
    public int refreshRate;
    public float globalVolume;
    public bool fullScreen;

    public SettingsData(Resolution res, float volume,bool fullScreen)
    {
        width = res.width;
        height = res.height;
        refreshRate = res.refreshRate;
        globalVolume = volume;
        this.fullScreen = fullScreen;
    }
}
