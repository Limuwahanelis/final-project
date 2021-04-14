using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicPuzzle1 : Puzzle
{
    private bool solved;
    public InteractableTorch[] torches = new InteractableTorch[5];
    private bool[] torchState = new bool[5];
    public GameObject hpPickUp;
    private GameManager gamMan;
    // Start is called before the first frame update
    void Start()
    {
        gamMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckIfTorchesAreLit(int torchIndex)
    {
        torchState[torchIndex] = true;
        for(int i=0;i<5;i++)
        {
            if (!torchState[i]) return;
        }
        solved = true;
        gamMan.MarkPuzzleAsSolved(1);
        hpPickUp.SetActive(true);
    }
    public override void MarkAsSolved()
    {
        for(int i=0;i<5;i++)
        {
            torches[i].LightUp();
            torches[i].enabled = false;
            torches[i].transform.GetComponent<Collider2D>().enabled = false;
        }
    }
}
