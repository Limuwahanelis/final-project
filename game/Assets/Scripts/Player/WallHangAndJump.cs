﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHangAndJump : MonoBehaviour
{

    public Player player;
    private Rigidbody2D rb;
    private Animator anim;

    private Coroutine myCor;

    public LayerMask groundLayer;

    public StaminaBar staminaBar;
    public Canvas staminaBarCanvas;


    public bool WallHanging
    {
        get { return isWallHanging; }
    }
    public bool WallJumping
    {
        get { return isWallJumping; }
    }
    public float WallContactSide
    {
        get { return contactSide; }
    }
    private bool isWallHanging=false;

    private bool touchesWallLeft;
    private bool touchesWallRight;
    public float wallHangDuration = 3f;

    public Vector2 wallJumpVec;
    public float wallJumpStrength;

    private bool isWallJumping;

    private bool wallJump;

    private int contactSide;// 1 means right, -1 means left
    private int prevoiusContactSide=2;

    public Transform wallCheck1;
    public Transform wallCheck2;

    public float wallCheckX;
    public float wallCheckY;

    // Start is called before the first frame update
    void Start()
    {
        anim = player.anim;
        rb = player.rb;
        staminaBar.SetMaxStamina(wallHangDuration);
        staminaBar.SetStamina(wallHangDuration);
        staminaBarCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWallHanging)
        {

            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("Wall jump");
                anim.SetBool("IsHanging", false);
                wallJump = true;

                isWallJumping = true;
                player.TakeControlFromPlayer(Player.Cause.WALLJUMP);
                isWallHanging = false;
                staminaBar.SetStamina(wallHangDuration);
                staminaBarCanvas.enabled = false;

                WallJump();

                player.SetCanFlipSprite(true);
                //canFlipSprite = true;
                //airAttack = false;
                //hitCollsDuringAirAttack.Clear();
            }
        }
        if (player.IsOnGround)
        {
            contactSide = 0;
            prevoiusContactSide = 0;
            player.ReturnControlToPlayer(Player.Cause.WALLJUMP);
            if (isWallJumping)
            {
                isWallJumping = false;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    public bool TouchesWall()
    {
        if (!isWallHanging)
        {
            touchesWallLeft = Physics2D.OverlapBox(wallCheck1.position, new Vector2(wallCheckX, wallCheckY), 0f, groundLayer);
            if (touchesWallLeft)
            {
                contactSide = -1;
                if (prevoiusContactSide != contactSide)
                {
                    isWallJumping = false;

                    return true;
                }
            }
            touchesWallRight = Physics2D.OverlapBox(wallCheck2.position, new Vector2(wallCheckX, wallCheckY), 0f, groundLayer);
            if (touchesWallRight)
            {
                contactSide = 1;
                if (prevoiusContactSide != contactSide)
                {
                    isWallJumping = false;

                    return true;
                }
            }
        }
        return false;
    }
    public void HangOnWall()
    {

        myCor = StartCoroutine(StickToWall());
    }
    public IEnumerator StickToWall()
    {
        //if (isWallHanging) yield return null;
        player.FlipPlayer(contactSide);
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, 0);
        isWallHanging = true;
        player.ReturnControlToPlayer(Player.Cause.WALLJUMP);
        player.TakeControlFromPlayer(Player.Cause.WALLHANG);
        staminaBarCanvas.enabled = true;
        anim.SetBool("IsHanging", true);
        float time = wallHangDuration;
        while (time >= 0)
        {
            time -= Time.deltaTime;
            staminaBar.SetStamina(time);
            yield return null;
        }
        staminaBar.SetStamina(wallHangDuration);
        staminaBarCanvas.enabled = false;
        rb.gravityScale = 2;
        player.ReturnControlToPlayer(Player.Cause.WALLHANG);
        isWallHanging = false;
        prevoiusContactSide = contactSide;
        //canFlipSprite = true;
        anim.SetBool("IsHanging", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(wallCheck1.position, new Vector3(wallCheckX, wallCheckY));
        Gizmos.DrawWireCube(wallCheck2.position, new Vector3(wallCheckX, wallCheckY));
    }
    void WallJump()
    {
        if (myCor != null) StopCoroutine(myCor);
        staminaBar.SetStamina(wallHangDuration);
        staminaBarCanvas.enabled = false;
        rb.gravityScale = 2;
        Vector2 temp = wallJumpVec;
        temp = new Vector2(wallJumpVec.x * -contactSide, wallJumpVec.y);
        rb.AddForce(temp * wallJumpStrength, ForceMode2D.Impulse);
        prevoiusContactSide = contactSide;

        //flipXScale();
        wallJump = false;
    }
}
