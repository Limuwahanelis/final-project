using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
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
    public enum ability
    {
        WALLJHANGANDJUMP, AIRATTACK, BOMB
    }

    
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(this.gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
        abilities[0] = true;
        abilities[1] = true;
        abilities[2] = true;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (Config.load) { Load(); Config.load = false; }
        }
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Debug.Log("loa 2+ " + SceneManager.GetActiveScene().name);
            //invBar = GameObject.FindGameObjectWithTag("Invicibility Bar").GetComponent<InvincibilityBar>();
            //player = GameObject.FindGameObjectWithTag("Player");
            LoadForBoss();
        }
        Debug.Log(player);
        Debug.Log("scene is " + SceneManager.GetActiveScene().buildIndex);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //save();
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }
    public void UnlockAbility(ability ab)
    {
        abilities[(int)ab] = true;
    }

    public bool CheckIfAbilityIsUnlocked(ability ab)
    {
        return abilities[(int)ab];
    }
    public void Save()
    {
        Debug.Log(player);
        Debug.Log(invBar);
        PlayerData playerData = new PlayerData(player.GetComponent<Player>(), abilities, invBar.GetFillAmount(), player.GetComponent<PlayerHealthSystem>(),player.GetComponent<PlayerCombat>());
        Debug.Log(playerData.maxHealth);
        string json = JsonUtility.ToJson(playerData);

        File.WriteAllText(Application.persistentDataPath + "/saves.json", json);
        Debug.Log(Application.persistentDataPath);
        SaveStageState();
    }
    public void SaveStageState()
    {
        Debug.Log(Application.persistentDataPath);
        StageStateData stageData = new StageStateData(puzzleStates);
        string json = JsonUtility.ToJson(stageData);
        File.WriteAllText(Application.persistentDataPath + "/stageState.json", json);
    }

    public void LoadStageState()
    {
        Debug.Log(Application.persistentDataPath);
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
        Debug.Log("aa");
        string json= File.ReadAllText(Application.persistentDataPath + "/saves.json");
        PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);
        Debug.Log(loadedData.maxHealth);
        player.transform.position = new Vector3(loadedData.position[0], loadedData.position[1]);
        abilities[(int)ability.AIRATTACK] = loadedData.abilities[(int)ability.AIRATTACK];
        abilities[(int)ability.BOMB] = loadedData.abilities[(int)ability.BOMB];
        abilities[(int)ability.WALLJHANGANDJUMP] = loadedData.abilities[(int)ability.WALLJHANGANDJUMP];
        player.GetComponent<PlayerHealthSystem>().SetMaxHP(loadedData.maxHealth);
        player.GetComponent<PlayerHealthSystem>().hpBar.SetHealth(loadedData.health);
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
        //Debug.Log("loa + "+SceneManager.GetActiveScene().name);

    }

    public void LoadForBoss()
    {
        
        string json = File.ReadAllText(Application.persistentDataPath + "/BossSave.json");
        Debug.Log(Application.persistentDataPath);
        Debug.Log(json);
        PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);
        //player.transform.position = new Vector3(loadedData.position[0], loadedData.position[1]);
        abilities[(int)ability.AIRATTACK] = loadedData.abilities[(int)ability.AIRATTACK];
        abilities[(int)ability.BOMB] = loadedData.abilities[(int)ability.BOMB];
        abilities[(int)ability.WALLJHANGANDJUMP] = loadedData.abilities[(int)ability.WALLJHANGANDJUMP];
        //player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player is" + player);
        Debug.Log("invBar is" + invBar);
        player.GetComponent<PlayerHealthSystem>().SetMaxHP(loadedData.maxHealth);
        //player.GetComponent<PlayerHealthSystem>().hpBar.setMaxHealth(loadedData.maxHealth);
        player.GetComponent<PlayerHealthSystem>().hpBar.SetHealth(loadedData.health);
        player.GetComponent<PlayerHealthSystem>().mText.text = loadedData.health.ToString();
        player.GetComponent<PlayerCombat>().attackDamage = loadedData.damage;
        invBar.SetFill(loadedData.invicibilityProgress);

        //for (int i = 0; i < 3; i++)
        //{
        //    if (abilities[i]) Destroy(abilityUnlocks[i]);
        //}
        Boss a = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        Debug.Log("Boss is " + a);
        a.SetAttack();

        
    }
    public void SaveForBoss()
    {
        Debug.Log(Application.persistentDataPath);
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
    }
    public void ShowGameOverScreen()
    {
        gameOverScreen.ShowScreen();
        playerIsAlive = false;
    }
}
