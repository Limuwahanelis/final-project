using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator anim;
    public float exposionDelay = 0.5f;
    private float countDownStartTime;
    public CircleCollider2D colC;
    private Collider2D[] colliders;
    public float radius;
    private bool touchedGround = false;
    private bool explode;
    private bool startCountDown;
    void Start()
    {
        anim = GetComponent<Animator>();
        //col = GetComponentInChildren<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //checkForDestructable();
        if (startCountDown)
        {
            if (!explode)
            {
                if (Time.time - countDownStartTime > exposionDelay)
                {
                    anim.SetTrigger("Explode");
                    Debug.Log("Exlosion");
                    explode = true;
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
            //if(collision.collider.CompareTag("DestructableGround"))
            //{
            //    collision.left
            //}
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
        colliders=Physics2D.OverlapCircleAll(transform.position, radius);
        for(int i=0;i<colliders.Length;i++)
        {
            if(colliders[i].gameObject.GetComponent<DestructableGround>())
            {
                Collider2D col = colliders[i];
                colliders[i].gameObject.GetComponent<DestructableGround>().Destroy(colC.radius,transform.position);
                return;
            }
        }

    }
}
