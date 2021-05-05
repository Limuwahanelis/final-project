using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameOverScreen gameOverScreen;
    public static GameManager instance=null;
    public static GameObject player;
    public InvincibilityBar invBar;
    public GameObject[] abilityUnlocks;
    public SpriteRenderer stage1BG;
    public SpriteRenderer stage2BG;
    public Collider2D swapBGTrig;
    private bool[] puzzleStates = new bool[2];
    public Puzzle[] puzzles;
    [SerializeField]
    private bool[] abilities = new bool[3];
    public bool playerIsAlive=true;

    public GameObject credits;

    public Text messageTex;



    public enum ability
    {
        WALLJHANGANDJUMP, AIRATTACK, BOMB
    }

    
    // Start is called before the first frame update
    private void Awake()
    {
        SceneManager.sceneLoaded += WhenNewSceneIsLoaded;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        player = GameObject.FindGameObjectWithTag("Player");
        //if (SceneManager.GetActiveScene().buildIndex == 1)
        //{
        //    if (Config.load) { Load(); Config.load = false; }
        //}
    }
    void Start()
    {
        Boss.OnGameCompleteEvent += ShowCredits;
        //if (SceneManager.GetActiveScene().buildIndex == 2)
        //{
        //    Debug.Log("loa 2+ " + SceneManager.GetActiveScene().name);
        //    LoadForBoss();
        //}
        Debug.Log(player);
        Debug.Log("scene is " + SceneManager.GetActiveScene().buildIndex);

    }

    public void ShowCredits()
    {
        credits.SetActive(true);
    }

    public GameObject GetPlayer()
    {
        return player;
    }
    public void UnlockAbility(ability ab,string abilityText)
    {
        abilities[(int)ab] = true;
        SetMessage(abilityText);
    }
    public void SetMessage(string message)
    {
        messageTex.text = message;
        StartCoroutine(MessageCor());
    }
    IEnumerator MessageCor()
    {
        messageTex.enabled = true;
        yield return new WaitForSeconds(2f);
        messageTex.enabled = false;
    }
    public bool CheckIfAbilityIsUnlocked(ability ab)
    {
        return abilities[(int)ab];
    }
    public void Save()
    {

        PlayerData playerData = new PlayerData(player.GetComponent<Player>(), abilities, invBar.GetFillAmount(), player.GetComponent<PlayerHealthSystem>(),player.GetComponent<PlayerCombat>());
        string json = JsonUtility.ToJson(playerData);

        File.WriteAllText(Application.persistentDataPath + "/saves.json", json);
        SaveStageState();
    }
    public void SaveStageState()
    {
        StageStateData stageData = new StageStateData(puzzleStates);
        string json = JsonUtility.ToJson(stageData);
        File.WriteAllText(Application.persistentDataPath + "/stageState.json", json);
    }

    public void LoadStageState()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/stageState.json");
        StageStateData stageData = JsonUtility.FromJson<StageStateData>(json);
        puzzleStates[0] = stageData.puzzle1Solved;
        puzzleStates[1] = stageData.puzzle2Solved;
        for(int i=0;i<2;i++)
        {
            if (puzzleStates[i]) puzzles[i].MarkAsSolved();
        }
    }
    public void Load()
    {
        string json= File.ReadAllText(Application.persistentDataPath + "/saves.json");
        PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);
        player.transform.position = new Vector3(loadedData.position[0], loadedData.position[1]);
        abilities[(int)ability.AIRATTACK] = loadedData.abilities[(int)ability.AIRATTACK];
        abilities[(int)ability.BOMB] = loadedData.abilities[(int)ability.BOMB];
        abilities[(int)ability.WALLJHANGANDJUMP] = loadedData.abilities[(int)ability.WALLJHANGANDJUMP];
        player.GetComponent<PlayerHealthSystem>().SetMaxHP(loadedData.maxHealth);
        player.GetComponent<PlayerHealthSystem>().hpBar.SetHealth(loadedData.health);
        player.GetComponent<PlayerHealthSystem>().currentHP = loadedData.health;
        player.GetComponent<PlayerHealthSystem>().mText.text = loadedData.health.ToString();
        player.GetComponent<PlayerCombat>().attackDamage = loadedData.damage;
        invBar.SetFill(loadedData.invicibilityProgress);

        for(int i=0;i<3;i++)
        {
            if (abilities[i]) Destroy(abilityUnlocks[i]);
        }
        LoadStageState();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SaveForBoss();
        SceneManager.LoadScene(2);
        StartCoroutine( WaitForSceneToLoad(2));

    }

    public void LoadForBoss()
    {
        player.transform.position = GameObject.FindGameObjectWithTag("spawn point").transform.position;
        //string json = File.ReadAllText(Application.persistentDataPath + "/BossSave.json");
        //Debug.Log(Application.persistentDataPath);
        //Debug.Log(json);
        //PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);
        //abilities[(int)ability.AIRATTACK] = loadedData.abilities[(int)ability.AIRATTACK];
        //abilities[(int)ability.BOMB] = loadedData.abilities[(int)ability.BOMB];
        //abilities[(int)ability.WALLJHANGANDJUMP] = loadedData.abilities[(int)ability.WALLJHANGANDJUMP];
        //Debug.Log("Player is" + player);
        //Debug.Log("invBar is" + invBar);
        //player.GetComponent<PlayerHealthSystem>().SetMaxHP(loadedData.maxHealth);
        //player.GetComponent<PlayerHealthSystem>().hpBar.SetHealth(loadedData.health);
        ////player.GetComponent<PlayerHealthSystem>().currentHP = loadedData.maxHealth;
        //player.GetComponent<PlayerHealthSystem>().mText.text = loadedData.health.ToString();
        //player.GetComponent<PlayerCombat>().attackDamage = loadedData.damage;
        //invBar.SetFill(loadedData.invicibilityProgress);

        Boss a = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        //Debug.Log("Boss is " + a);
        a.SetAttack();
        //player.GetComponent<Player>().SetPosition(GameObject.FindGameObjectWithTag("spawn point").transform);

    }
    public void SaveForBoss()
    {
        PlayerData playerData = new PlayerData(player.GetComponent<Player>(), abilities, invBar.GetFillAmount(), player.GetComponent<PlayerHealthSystem>(),player.GetComponent<PlayerCombat>());
        string json = JsonUtility.ToJson(playerData);

        File.WriteAllText(Application.persistentDataPath + "/BossSave.json", json);
    }
    public void MarkPuzzleAsSolved(int puzzleNum)
    {
        puzzleNum = puzzleNum - 1;
        puzzleStates[puzzleNum] = true;
    }

    IEnumerator WaitForSceneToLoad(int sceneNumber)
    {
        while(SceneManager.GetActiveScene().buildIndex!=sceneNumber)
        {
            yield return null;
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Debug.Log("loa 2+ " + SceneManager.GetActiveScene().name);
           
            LoadForBoss();
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Debug.Log("new scene loaded");
            Config.load = false;
            Load();
        }
    }
    public void ShowGameOverScreen()
    {
        gameOverScreen.ShowScreen();
        playerIsAlive = false;
    }

    public void WhenNewSceneIsLoaded(Scene scene,LoadSceneMode loadSceneMode )
    {
        if(scene.buildIndex==0)
        {
            Destroy(player);
        }
        if (scene.buildIndex == 1 && Config.load)
        {
            Debug.Log("load dasda");
            StartCoroutine(WaitForSceneToLoad(scene.buildIndex));

        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= WhenNewSceneIsLoaded;
        Boss.OnGameCompleteEvent -= ShowCredits;
    }

}
