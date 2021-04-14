using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrystalAttackPattern : MonoBehaviour
{
    public Transform[] attackPositions;
    public Transform[] attackTargets;
    public BossCrystal[] crystals;
    private bool fired = false;
    public bool PatternHasEnded = false;
    private bool patternInProcess = false;
    public float patternAttackTimeDuration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        foreach(BossCrystal crystal in crystals)
        {
            crystal.SetAttackDuration(patternAttackTimeDuration);
            Debug.Log(patternAttackTimeDuration);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (patternInProcess)
        {

            if (!fired)
            {
                bool fire = true;
                foreach (BossCrystal crystal in crystals)
                {
                    if (!crystal.readyTofire)
                    {
                        fire = false;
                        break;
                    }
                }
                if (fire)
                {
                    fired = true;
                    foreach (BossCrystal crystal in crystals)
                    {
                        crystal.FireBeam();
                    }
                }
            }
            if (fired)
            {
                bool endPattern = true;
                foreach (BossCrystal crystal in crystals)
                {
                    if (!crystal.endAttack)
                    {
                        endPattern = false;
                        break;
                    }
                }
                if (endPattern)
                {
                    PatternHasEnded = true;
                    patternInProcess = false;

                }
            }
        }
    }
    public void StartPattern()
    {
        fired = false;
        PatternHasEnded = false;
        patternInProcess = true;
        for (int i = 0; i < crystals.Length; i++)
        {
            crystals[i].SetAttackDuration(patternAttackTimeDuration);
            crystals[i].SetPostionToMove(attackPositions[i].position);
            crystals[i].SetTarget(attackTargets[i].position);
        }
    }

}
