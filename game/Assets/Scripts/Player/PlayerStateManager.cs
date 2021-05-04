using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerCustomState
{
    public string stateName="";
    public bool stateValue=false;
    public PlayerCustomState(string name, bool value)
    {
        this.stateName = name;
        this.stateValue = value;
    }
}
public class PlayerStateManager : MonoBehaviour
{
    public bool isOnGround;
    public bool isAlive=true;
    public bool isHangingOnWall;
    public bool isWallJumping = false;
    public PlayerCustomState[] listOfStates;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
