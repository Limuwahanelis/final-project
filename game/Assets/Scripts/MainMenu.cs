using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenu : MonoBehaviour
{

    struct ResIndex
    {
        public int refreshRateIndex;
        public int resolutionIndex;
    }

    Resolution[] allResolutions;
    ResIndex currentResIndex;
    List<Resolution> resolutionsWithCurrentRefreshRate = new List<Resolution>();
    List<List<Resolution>> resolutions = new List<List<Resolution>>();
    List<int> refreshRates = new List<int>();
    List<List<string>> resolutionOptions = new List<List<string>>();
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown refreshRateDropdown;
    
    // Start is called before the first frame update
    void Start()
    {
        currentResIndex.refreshRateIndex = 0;
        currentResIndex.resolutionIndex = 0;

        int currentRefreshRate = Screen.currentResolution.refreshRate;

        allResolutions = Screen.resolutions;
        SetFullScreen(false);
        resolutionDropdown.ClearOptions();
        refreshRateDropdown.ClearOptions();
        List<string> refreshRatesS = new List<string>();


        int firstRefreshRate = allResolutions[0].refreshRate;
        refreshRates.Add(allResolutions[0].refreshRate);
        refreshRatesS.Add(allResolutions[0].refreshRate.ToString() + " Hz");

        int nextRefreshRate = -1;
        int j = 1;

        while (nextRefreshRate != firstRefreshRate)
        {
            refreshRates.Add(allResolutions[j].refreshRate);
            refreshRatesS.Add(allResolutions[j].refreshRate.ToString() + " Hz");
            j++;
            nextRefreshRate = allResolutions[j].refreshRate;

        }


        for (int i = 0; i < j; i++)
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

        for (int i = 0; i < resolutions.Count; i++)
        {
            resolutions[i].Sort((r1, r2) => r1.height.CompareTo(r2.height));
            resolutions[i].Sort((r1, r2) => r1.width.CompareTo(r2.width));
        }

        refreshRateDropdown.AddOptions(refreshRatesS);
        refreshRateDropdown.RefreshShownValue();
        resolutionDropdown.AddOptions(resolutionOptions[0]);
        Screen.SetResolution(resolutions[0][0].width, resolutions[0][0].height, false);
    }
    public void SetResolution(int resolutionIndex)
    {
        currentResIndex.resolutionIndex = resolutionIndex;
        Screen.SetResolution(resolutions[currentResIndex.refreshRateIndex][currentResIndex.resolutionIndex].width, resolutions[currentResIndex.refreshRateIndex][currentResIndex.resolutionIndex].height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullscreen)
    {
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
        Resolution toReturn=new Resolution();

        for (int i = 0; i < resolutions[refreshRateIndex].Count; i++)
        {
            if (resolutions[refreshRateIndex][i].width == Screen.width)
            {
                if (resolutions[refreshRateIndex][i].height == Screen.height)
                {
                    
                    toReturn = resolutions[refreshRateIndex][i];
                    currentResIndex.refreshRateIndex = refreshRateIndex;
                    currentResIndex.resolutionIndex =i;
                    return toReturn;
                }
            }
        }

        toReturn = resolutions[refreshRateIndex][currentResIndex.resolutionIndex];
        currentResIndex.refreshRateIndex = refreshRateIndex;
        return toReturn;
    }

    static public MainMenu menu;
    public GameObject controlPanel;
    public GameObject optionsPanel;
    public GameObject buttons;
    public void Awake()
    {
        menu = this;
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Load()
    {
        Config.load = true;
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Back(GameObject panel)
    {
        panel.SetActive(false);
        buttons.SetActive(true);
    }
    public void Controls()
    {
        controlPanel.SetActive(true);
        buttons.SetActive(false);
    }
    public void Options()
    {
        optionsPanel.SetActive(true);
        buttons.SetActive(false);
    }
}
