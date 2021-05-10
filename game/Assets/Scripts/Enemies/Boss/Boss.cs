using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss : Enemy,IDamagable
{
    public static Action OnGameCompleteEvent;

    public static Action OnBossMissileAttack;

    public GameObject[] beams;
    private GameObject player;
    public Transform missileSpawn;
    public GameObject missilePrefab;
    public BossCrystalManager crystals;
    public Vector3 delayedBeamPos;
    public Transform attackTrans;
    public Transform vulnerableTrans;
    public Vector3 attackPos;
    public Vector3 vulnerablePos;
    //public GameObject credits;
    public float attackDelay = 1f;
    public float vulnerableTime = 2f;
    private int attackPatten = 1;
    public bool attack = false;
    private bool isAlive = true;
    private bool moveToVulnerablePos = false;
    private bool moveToAttackPos= false;

    void Awake()
    {
        attackPos = attackTrans.position;
        vulnerablePos = vulnerableTrans.position;
        player = GameObject.FindGameObjectWithTag("Player");
        hpSys = GetComponent<HealthSystem>();
        anim = GetComponent<Animator>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (isAlive)
        {
            if (attack)
            {
                switch (attackPatten)
                {
                    case 1: StartCoroutine(AttackCor1()); break;
                    case 2: StartCoroutine(AttackCor2()); break;
                    case 3: StartCoroutine(AttackCor3()); break;
                    default: break;
                }
                attack = false;
                attackPatten++;
                if (attackPatten > 3) attackPatten = 1;
            }
            if(moveToVulnerablePos)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, vulnerablePos, step);
                if(Vector3.Distance(transform.position,vulnerablePos)<0.001f)
                {
                    moveToVulnerablePos = false;
                    StartCoroutine(VulnerableCor());
                }
            }
            if (moveToAttackPos)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, attackPos, step);
                if (Vector3.Distance(transform.position, attackPos) < 0.001f)
                {
                    moveToAttackPos = false;
                    crystals.StartCrystalAttacks();
                }
            }
        }
        else
        {
            if (moveToVulnerablePos)
            {
                
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, vulnerablePos, step);
                if (Vector3.Distance(transform.position, vulnerablePos) < 0.001f)
                {
                    Kill();
                    moveToVulnerablePos = false;
                }
            }
        }
    }

    IEnumerator AttackCor1()
    {
        yield return new WaitForSeconds(attackDelay);
        for(int i=0;i<10;i++)
        {
            beams[i].transform.position = new Vector3(player.transform.position.x, beams[i].transform.position.y);
            beams[i].GetComponent<DelayedBeam>().SetCor();
            if(i>3)
            {
                beams[i - 4].GetComponent<DelayedBeam>().DisableCor();
                beams[i - 4].transform.localPosition = delayedBeamPos;
            }
            yield return new WaitForSeconds(0.5f);
        }
        for(int i=6;i<10;i++)
        {
            beams[i].GetComponent<DelayedBeam>().DisableCor();
            beams[i].transform.localPosition = delayedBeamPos;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(attackDelay);
        Debug.Log("end attack");
        moveToVulnerablePos = true;
    }
    IEnumerator AttackCor2()
    {
        yield return new WaitForSeconds(attackDelay);
        for (int i=0;i<60;i++)
        {
            //OnBossMissileAttack?.Invoke();
            GameObject missile =  Instantiate(missilePrefab, missileSpawn.transform.position, Quaternion.Euler(0,0,90+i*20));
            AudioSource tmpSource = missile.AddComponent<AudioSource>();
            GetComponent<BossAudioManager>().bossMissileAudio.Play(tmpSource);
            missile.GetComponent<Missile>().SetSpeed(10);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(attackDelay);
        moveToVulnerablePos = true;
    }
    IEnumerator AttackCor3()
    {
        yield return new WaitForSeconds(attackDelay);
        for (int i = 0; i < 20; i++)
        {
            GameObject missile = Instantiate(missilePrefab, missileSpawn.transform.position,missilePrefab.transform.rotation);
            missile.transform.up = player.transform.position - missileSpawn.transform.position;
            AudioSource tmpSource = missile.AddComponent<AudioSource>();
            GetComponent<BossAudioManager>().bossMissileAudio.Play(tmpSource);
            missile.GetComponent<Missile>().SetSpeed(10);
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(attackDelay);
        moveToVulnerablePos = true;
    }
    IEnumerator VulnerableCor()
    {
        yield return new WaitForSeconds(vulnerableTime);
        moveToAttackPos = true;
    }
    public void StartAttacking()
    {
        attack = true;
    }
    public void TakeDamage(int dmg)
    {
        hpSys.TakeDamage(dmg);
        if(hpSys.currentHP<=0)
        {
            isAlive = false;
            moveToVulnerablePos = true;
        }
    }

    public void Kill()
    {
        anim.SetTrigger("dead");
        StopAllCoroutines();
        crystals.DestroyCrystals();
    }

    public void Knockback()
    {
    }

    public void SlowDown(float slowDownFactorx, float slowDownFactory)
    {
    }

    public override void SetPlayerInRange()
    {
    }

    public override void SetPlayerNotInRange()
    {
    }
    public void ShowCredits()
    {
        OnGameCompleteEvent?.Invoke();
    }
    public void SetAttack()
    {
        attack = true;
    }
}
