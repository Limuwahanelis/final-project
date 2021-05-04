using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MushroomGuyEnemy : Enemy, IDamagable
{
    public float attackDelay = 3f;
    private bool attack;
    private bool isAttacking;
    private Vector3[] patrolPos = new Vector3[2]; // first is left
    public Transform[] patrolTransofrms = new Transform[2];

    private bool isPatrollingLeft = false;
    private bool isPatrollingRight = true;

    private int lastPatrol=1;
    Coroutine cor;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        hpSys = transform.GetComponent<HealthSystem>();
        patrolPos[0] = patrolTransofrms[0].position;
        patrolPos[1] = patrolTransofrms[1].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (attack)
        {
            if (!isAttacking)
            {
                cor=StartCoroutine(AttackDelayCor());
            }
        }
        if (!isAttacking)
        {
            if (isPatrollingLeft)
            {
                MoveLeft();
            }
            if (isPatrollingRight)
            {
                MoveRight();
            }
        }
    }
    private void MoveLeft()
    {
        anim.SetBool("isWalking", true);
        transform.position = Vector3.MoveTowards(transform.position, patrolPos[0], speed * Time.deltaTime);
        //RaiseOnWalkEvent();
        if (transform.position.x <= patrolPos[0].x)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            isPatrollingRight = true;
            isPatrollingLeft = false;
            lastPatrol = 1;

        }
    }
    private void MoveRight()
    {
        anim.SetBool("isWalking", true);
        transform.position = Vector3.MoveTowards(transform.position, patrolPos[1], speed * Time.deltaTime);
        //RaiseOnWalkEvent();
        if (transform.position.x >= patrolPos[1].x)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            isPatrollingRight = false;
            isPatrollingLeft = true;
            lastPatrol = 0;
        }
    }
    public void Kill()
    {
        IncreaseInvicibilityProgress();
        Destroy(this.gameObject);
    }

    public void Knockback()
    {
        throw new System.NotImplementedException();
    }

    public override void SetPlayerInRange()
    {
        attack = true;
        isPatrollingLeft = false;
        isPatrollingRight = false;
        anim.SetBool("isWalking", false);
    }

    public override void SetPlayerNotInRange()
    {
        attack = false;
        //isAttacking = true;
    }

    public void SlowDown(float slowDownFactorx, float slowDownFactory)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int dmg)
    {
        hpSys.TakeDamage(dmg);
        anim.SetTrigger("hit");
        if(!attack)
        {
            Rotate();
        }
        if (hpSys.currentHP <= 0) Kill();
    }

    IEnumerator AttackDelayCor()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackDelay);
        
        anim.SetTrigger("Attack");
        RaiseOnAttackEvent();
        
    }
    public void EndAttack()
    {
        isAttacking = false;
        if (lastPatrol == 0) isPatrollingLeft = true;
        else isPatrollingRight = true;
    }

    void Rotate()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
