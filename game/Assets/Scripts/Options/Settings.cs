using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    struct ResIndex
    {
        public int refreshRateIndex;
        public int resolutionIndex;
    }
    public static Settings instance = null;
    Resolution[] allResolutions;
    ResIndex currentResIndex;
    List<List<Resolution>> resolutions = new List<List<Resolution>>();
    List<int> refreshRates = new List<int>();
    List<string> refreshRatesS = new List<string>();
    List<List<string>> resolutionOptions = new List<List<string>>();
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown refreshRateDropdown;
    public Toggle fullScreenToggle;
    bool fullScreen;
    // Start is called before the first frame update

    private void Awake()
    {

    }
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        currentResIndex.refreshRateIndex = 0;
        currentResIndex.resolutionIndex = 0;

        allResolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        refreshRateDropdown.ClearOptions();


        FindAllRefreshRates();

        AddResolutionsToLists();


        for (int i = 0; i < resolutions.Count; i++)
        {
            resolutions[i].Sort((r1, r2) => r1.height.CompareTo(r2.height));
            resolutions[i].Sort((r1, r2) => r1.width.CompareTo(r2.width));
        }
        refreshRateDropdown.AddOptions(refreshRatesS);
        refreshRateDropdown.RefreshShownValue();
        resolutionDropdown.AddOptions(resolutionOptions[0]);

    }
    private void AddResolutionsToLists()
    {
        for (int i = 0; i < refreshRates.Count; i++)
        {
            resolutions.Add(new List<Resolution>());
            resolutionOptions.Add(new List<string>());
        }

        for (int i = 0; i < allResolutions.Length; i++)
        {
            int refreshRateIndex = -1;

            for (int k = 0; k < refreshRates.Count; k++)
            {
                if (refreshRates[k] == allResolutions[i].refreshRate)
                {
                    refreshRateIndex = k;
                    break;
                }

            }
            resolutions[refreshRateIndex].Add(allResolutions[i]);
            resolutionOptions[refreshRateIndex].Add(allResolutions[i].width + " x " + allResolutions[i].height);

        }
    }
    private void FindAllRefreshRates()
    {
        for (int i = 0; i < allResolutions.Length; i++)
        {
            if (!refreshRates.Exists((x) => x == allResolutions[i].refreshRate))
            {
                refreshRates.Add(allResolutions[i].refreshRate);
            }
        }
        refreshRates.Sort();
        for (int i = 0; i < refreshRates.Count; i++)
        {
            refreshRatesS.Add(refreshRates[i].ToString() + " Hz");
        }

    }
    public void SetResolution(int resolutionIndex)
    {
        currentResIndex.resolutionIndex = resolutionIndex;
        Screen.SetResolution(resolutions[currentResIndex.refreshRateIndex][currentResIndex.resolutionIndex].width, resolutions[currentResIndex.refreshRateIndex][currentResIndex.resolutionIndex].height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        fullScreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
    }

    public void SelectRefreshRate(int refreshRateIndex)
    {
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions[refreshRateIndex]);
        resolutionDropdown.RefreshShownValue();
        Resolution res = findResolutionForRefreshRateIndex(resolutions[currentResIndex.refreshRateIndex][currentResIndex.resolutionIndex].width, resolutions[currentResIndex.refreshRateIndex][currentResIndex.resolutionIndex].height, refreshRateIndex);
        resolutionDropdown.value = currentResIndex.resolutionIndex;
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);

    }

    Resolution findResolutionForRefreshRateIndex(int width, int height, int refreshRateIndex)
    {
        Resolution toReturn = new Resolution();

        for (int i = 0; i < resolutions[refreshRateIndex].Count; i++)
        {
            if (resolutions[refreshRateIndex][i].width == Screen.width)
            {
                if (resolutions[refreshRateIndex][i].height == Screen.height)
                {

                    toReturn = resolutions[refreshRateIndex][i];
                    currentResIndex.refreshRateIndex = refreshRateIndex;
                    currentResIndex.resolutionIndex = i;
                    return toReturn;
                }
            }
        }
        toReturn = FindNearestResolutionForRefreshRate(width, height, refreshRateIndex); //resolutions[refreshRateIndex][currentResIndex.resolutionIndex];
        currentResIndex.refreshRateIndex = refreshRateIndex;
        return toReturn;
    }
    Resolution FindNearestResolutionForRefreshRate(int width, int height, int refreshRateIndex)
    {
        int minResolutionIndex = 0;
        int difference = Mathf.Abs(resolutions[refreshRateIndex][0].width - width) + Mathf.Abs(resolutions[refreshRateIndex][0].height - height);
        int minDifference = difference;
        for (int i = 1; i < resolutions[refreshRateIndex].Count; i++)
        {
            difference = Mathf.Abs(resolutions[refreshRateIndex][i].width - width) + Mathf.Abs(resolutions[refreshRateIndex][i].height - height);
            if (difference < minDifference)
            {
                minDifference = difference;
                minResolutionIndex = i;
            }
        }
        currentResIndex.resolutionIndex = minResolutionIndex;
        return resolutions[refreshRateIndex][minResolutionIndex];
    }
    public Resolution GetResolution()
    {
        return resolutions[currentResIndex.refreshRateIndex][currentResIndex.resolutionIndex];
    }

    public bool GetFullScreen()
    {
        return fullScreen;
    }
}
