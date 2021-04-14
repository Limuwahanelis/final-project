using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : Enemy,IDamagable
{

    //public Animator anim;

    //private HealthSystem hpSys;
    public Transform spawnPoint;
    public Transform[] patrolPlaces;
    public Vector3[] patrolPos;
    public SpriteRenderer rend;
   // public float speed; // maybe *Time.deltaTRime
    //public int dmg;

    private bool isPatrollingLeft = true;
    private bool isPatrollingRight = false;

    // Start is called before the first frame update
    void Start()
    {
        hpSys = GetComponent<HealthSystem>();
        anim = GetComponent<Animator>();
        patrolPos = new Vector3[2];
        patrolPos[0] = patrolPlaces[0].position;
        patrolPos[1] = patrolPlaces[1].position;
    }

    // Update is called once per frame
    void Update()
    {
        //anim.SetBool()
        if(isPatrollingLeft)
        {
            anim.SetBool("isWalking", true);
            transform.position=Vector3.MoveTowards(transform.position, patrolPos[0], speed * Time.deltaTime);
            if (transform.position.x <= patrolPos[0].x)
            {
                rend.flipX = true;
                isPatrollingRight = true;
                isPatrollingLeft = false;
            }
        }
        if (isPatrollingRight)
        {
            anim.SetBool("isWalking", true);
            transform.position= Vector3.MoveTowards(transform.position ,patrolPos[1], speed*Time.deltaTime);
            if (transform.position.x >= patrolPos[1].x)
            {
                rend.flipX = false;
                isPatrollingRight = false;
                isPatrollingLeft = true; ;
            }
        }
    }
    public  void TakeDamage(int dmg)
    {
        hpSys.TakeDamage(dmg);
        if (hpSys.currentHP <= 0) Kill();
    }
    public  void Kill()
    {
        IncreaseInvicibilityProgress();
        Destroy(transform.gameObject);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.gameObject.layer==LayerMask.NameToLayer("Player"))
        {
            IDamagable player = collision.transform.GetComponent<IDamagable>();
            player.Knockback();
            player.TakeDamage(dmg);
        }
    }
    public  void Knockback()
    {

    }
    public  void SlowDown(float slowDownFactorx, float slowDownFactory)
    {

    }

    public override void SetPlayerInRange()
    {
        throw new System.NotImplementedException();
    }

    public override void SetPlayerNotInRange()
    {
        throw new System.NotImplementedException();
    }
}
