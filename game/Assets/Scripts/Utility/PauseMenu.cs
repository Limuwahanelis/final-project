using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public bool paused = false;
    // Update is called once per frame
    void Update()
    {
        

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().playerIsAlive)
            {
                if (!paused)
                {
                    Pause();
                    paused = true;
                }
                else
                {
                    Resume();
                    paused = false;
                }
            }
        }
    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        Destroy(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetPlayer());
        Destroy(GameObject.FindGameObjectWithTag("GameManager"));
        SceneManager.LoadScene(0);
    }

    public void ShowPanel(GameObject panelToShow) 
    {
        panelToShow.SetActive(true);
    }
    public void HidePanel(GameObject panelToHide)
    {
        panelToHide.SetActive(false);
    }

    public void SaveSettings()
    {
        PlayerOptionsSaver.instance.SaveSettings();
    }
}
