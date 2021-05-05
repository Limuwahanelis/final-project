using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainCanvasManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject[] objectsToHideDuringBossFight;
    [SerializeField]
    GameObject[] objectsToDestroyInMainMenu;

    GameObject mainCanvas;

    public static MainCanvasManager instance;

    private void Awake()
    {
        mainCanvas = GameObject.FindGameObjectWithTag("Main Canvas");
        SceneManager.sceneLoaded += WhenNewSceneIsLoaded;
        if (instance==null)
        {
            instance = this;
        }
        if(instance!=this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WhenNewSceneIsLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        if (scene.buildIndex == 0)
        {
            Destroy(mainCanvas);
            
        }

        if (scene.buildIndex == 1)
        {
            if (mainCanvas == null) mainCanvas = GameObject.FindGameObjectWithTag("Main Canvas");
            for (int i = 0; i < objectsToDestroyInMainMenu.Length; i++)
            {
                objectsToDestroyInMainMenu[i].SetActive(true);
            }
            //bossHealthBar.SetActive(false);
        }

        if (scene.buildIndex == 2)
        {
            for (int i = 0; i < objectsToHideDuringBossFight.Length; i++)
            {
                objectsToHideDuringBossFight[i].SetActive(false);
            }
            //bossHealthBar.SetActive(true);
        }
    }

}
