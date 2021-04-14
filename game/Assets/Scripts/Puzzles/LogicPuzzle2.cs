using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LogicPuzzle2 : Puzzle
{
    // Start is called before the first frame update
    public InteractableTorch2[] torches;
    public Text text;
    public int numberToGet = 29;
    int number = 0;
    public GameObject crystal;
    [SerializeField]
    private bool solved = false;
    public Text riddleText;
    private GameManager gamMan;
    
    void Start()
    {
        gamMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateNumber(bool add,int value)
    {
        if (add) number += value;
        else number -= value;
        text.text = number.ToString();
        if(number==numberToGet)
        {
            solved = true;
            gamMan.MarkPuzzleAsSolved(2);
            StartCoroutine(MoveCrystalCor());
        }
    }
    IEnumerator MoveCrystalCor()
    {
        while(crystal.transform.position.y>=-21.67)
        {
            crystal.transform.position = new Vector3(crystal.transform.position.x, crystal.transform.position.y - 0.3f, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!solved) riddleText.enabled = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!solved) riddleText.enabled = false;
    }

    public override void MarkAsSolved()
    {
        torches[0].LightUp();
        torches[2].LightUp();
        torches[3].LightUp();
        torches[4].LightUp();
        text.text = numberToGet.ToString();
        StartCoroutine(MoveCrystalCor());
    }
}
