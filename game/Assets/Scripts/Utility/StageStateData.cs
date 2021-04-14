using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStateData
{
    public bool puzzle1Solved;
    public bool puzzle2Solved;
    public StageStateData(bool[] puzzleStates)
    {
        puzzle1Solved = puzzleStates[0];
        puzzle2Solved = puzzleStates[1];
    }

}
