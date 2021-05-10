using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeEnemy : Enemy,IDamagable
{
    public float radius = 20;
    float curAngle = 0;
    public float angleToMove=5;
    public Transform sprite;
    public Transform mainBody;
    private GameManager gamMan;
    public GameObject missilePrefab;
    private bool startAttacking=true;
    private int attackCount = 0;
    private int attacksInSeries = 3;
    private bool cooldown = false;
    private bool playerInRange = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetComponent<Animator>();
        gamMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        hpSys = transform.GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        curAngle += angleToMove*Time.deltaTime;
        mainBody.transform.localPosition = new Vector3( Mathf.Sin(Convert( curAngle))* radius, Mathf.Cos(Convert(curAngle))* radius);
        if (curAngle >= 360) curAngle = 0;

        if (playerInRange)
        {
            
            sprite.transform.right = gamMan.GetPlayer().transform.position - sprite.transform.position;
            if (sprite.transform.right.x < 0) sprite.GetComponent<SpriteRenderer>().flipY = true;
            else sprite.GetComponent<SpriteRenderer>().flipY = false;
            if (startAttacking)
            {
                if (!cooldown)
                {
                    anim.SetBool("attack", true);
                    cooldown = true;
                }
            }
        }
        
        //Debug.Log(sprite.transform.right);
    }
    private float Convert(float angleInDeg)
    {
        return Mathf.Deg2Rad * angleInDeg;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    IEnumerator AttackCor()
    {
        
        yield return new WaitForSeconds(3f);
        cooldown = false;
    }
    public void Attack()
    {
        missilePrefab.transform.up = gamMan.GetPlayer().transform.position - sprite.transform.position;
        RaiseOnAttackEvent();
        Instantiate(missilePrefab, sprite.transform.position, missilePrefab.transform.rotation);
        attackCount++;
        if(attackCount==attacksInSeries)
        {
            attackCount = 0;
            anim.SetBool("attack", false);
            StartCoroutine(AttackCor());
        }
        
    }

    public override void SetPlayerInRange()
    {
        playerInRange = true;
    }
    public override void SetPlayerNotInRange()
    {
        sprite.transform.right = new Vector2(1f, 0);
        sprite.GetComponent<SpriteRenderer>().flipY = false;
        playerInRange = false;
    }

    public void TakeDamage(int dmg)
    {
        hpSys.TakeDamage(dmg);
        if(hpSys.currentHP<=0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        IncreaseInvicibilityProgress();
        Destroy(transform.gameObject);
    }

    public void Knockback()
    {
        throw new System.NotImplementedException();
    }

    public void SlowDown(float slowDownFactorx, float slowDownFactory)
    {
        throw new System.NotImplementedException();
    }
}
