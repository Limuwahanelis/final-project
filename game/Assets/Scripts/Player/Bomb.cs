using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Bomb : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator anim;
    public float exposionDelay = 0.5f;
    private float countDownStartTime;
    public CircleCollider2D colC;
    private Collider2D[] colliders;
    private bool touchedGround = false;
    private bool explode;
    private bool startCountDown;

    public Action OnExplodeEvent;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startCountDown)
        {
            if (!explode)
            {
                if (Time.time - countDownStartTime > exposionDelay)
                {
                    anim.SetTrigger("Explode");
                    Debug.Log("Exlosion");
                    explode = true;
                    OnExplodeEvent?.Invoke();
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (touchedGround) return;
        else
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                countDownStartTime = Time.time;
                startCountDown = true;
                Debug.Log("Start");
                touchedGround = true;
            }
        }
    }

    public void Delete()
    {
        colC.enabled = true;
        CheckForDestructable();
        Destroy(transform.gameObject,0.1f);
        
    }
    public void CheckForDestructable()
    {
        colliders=Physics2D.OverlapCircleAll(transform.position, colC.radius);
        for(int i=0;i<colliders.Length;i++)
        {
            if(colliders[i].gameObject.GetComponent<DestructableGround>())
            {
                Collider2D col = colliders[i];
                colliders[i].gameObject.GetComponent<DestructableGround>().DestroyTiles(colC.radius,transform.position);
                return;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, colC.radius);
    }

}
