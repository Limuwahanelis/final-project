using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverPanel;
    public void ShowScreen()
    {
        gameOverPanel.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void GoToMenu()
    {
        Destroy(GameObject.FindGameObjectWithTag("GameManager"));
        SceneManager.LoadScene(0);
    }
}
