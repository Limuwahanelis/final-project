using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{

    public float secondsToWait;
    public float gravScale=6;

    private Rigidbody2D rb2D;
    private Vector3 startPos;
    private float moveBackSpeed;
    private bool movingBack = false;

    

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    public void EnableFall()
    {
        //rb2D.gravityScale = gravScale;
        rb2D.velocity = new Vector2(0, gravScale);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
            Debug.Log("Player hit");
            collision.GetComponentInParent<IDamagable>().TakeDamage(10);
            collision.GetComponentInParent<IDamagable>().SlowDown(0.5f,1f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer==LayerMask.NameToLayer("Ground"))
        {
            rb2D.velocity = new Vector2(0, 0);
            StartCoroutine(ReturnToStartPos(secondsToWait));
        }
    }

    IEnumerator ReturnToStartPos(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        rb2D.gravityScale = 0;
        GetComponentInParent<FallingBlockHolder>().EnableReturn();
    }

}
