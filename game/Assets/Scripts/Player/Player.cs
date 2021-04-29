using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour//, IDamagable
{
    
    public enum Cause
    {
        NONE,
        ENEMY,
        WALLJUMP,
        WALLHANG,
        SLIDE,
        OVERRIDE,
        ATTACK,
        AIRATTACK,
        JUMP,
        KNOCKBACK,
        BOMB
    }
    private Cause NoControlCause=Cause.NONE;

    public event Action OnWalkEvent;

    public LayerMask groundLayer;
    public LayerMask walkableLayers;

    private PlayerCombat combat;
    public Animator anim;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    private GameManager man;
    public WallHangAndJump wallHang;

    private bool slowedDown = false;

    public float dashSpeed = 5;
    public float moveSpeed = 1;
    public float jumpPower = 2;

    public Transform toFlip;


    private bool isOnGround = false;
    public bool IsOnGround
    {
        get { return isOnGround; }
    }
    private bool isUnderCeilling = false;


    private float xSpeed = 0;


    private Coroutine myCor;

    private bool isNotMovableByPlayer = false;
    private int flipSide = 1; // 1 means right, -1 means left 
    private int slideDirection; // as above
    
    private bool isJumping = false;
    private bool jump = false;
    private bool canFlipSprite = true;


    // coliders
    public CapsuleCollider2D capsuleColl;
    public CapsuleCollider2D slideCol;
    public CapsuleCollider2D EnemyDetectorCol;
    public CapsuleCollider2D EnemyDetectorColSlide;
    public BoxCollider2D boxCol;

    //checks
    public float ceillingCheckRange;

    public Transform groundCheck;
    public float groundCheckRange;

    public float floorCheckX;
    public float floorCheckY;

    public Transform ceillingCheck;
    //slide
    private bool isSlidingUnderCeiling = false;
    private bool slide = false;
    public float slideDuration = 1f;
    private bool stopSliding = false;

    private Invincibility invincibility;

    // Start is called before the first frame update

    void Start()
    {
        combat = GetComponent<PlayerCombat>();
        rb = GetComponent<Rigidbody2D>();
        invincibility = GetComponent<Invincibility>();
        man = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        wallHang = GetComponent<WallHangAndJump>();


    }

    
    void FixedUpdate()
    {
        if (combat.IsPlayerAlive())
        {
            if (slide)
            {
                rb.velocity = new Vector2(slideDirection * dashSpeed, 0);
            }
            if (stopSliding)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                stopSliding = false;
            }
            if(jump)
            {
                anim.SetBool("InAir", true);
                rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
                jump = false;
            }
            Move();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (combat.IsPlayerAlive())
        {
            xSpeed = 0;
            FallsDown();
            //CheckIfCanAirAttack();
            UnderCeiling();
            OnGround();
            anim.SetBool("Run", false);
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                anim.SetBool("Run", true);
                if (canFlipSprite) toFlip.localScale = new Vector3(1, toFlip.localScale.y, toFlip.localScale.z);
                flipSide = 1;
                xSpeed = moveSpeed;
            }
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                anim.SetBool("Run", true);

                if (canFlipSprite) toFlip.localScale = new Vector3(-1, toFlip.localScale.y, toFlip.localScale.z);
                flipSide = -1;
                xSpeed = -moveSpeed;
            }

           
            if (!isNotMovableByPlayer)
            {
                if (isOnGround)
                {
                    if (Input.GetAxisRaw("Horizontal") != 0) OnWalkEvent?.Invoke();
                    anim.SetBool("InAir", false);
                    if (Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.DownArrow) && !slide)
                    {
                        isJumping = true;

                        anim.SetTrigger("Jump");
                    }
                }
                else
                {
                    isJumping = false;
                    anim.SetBool("InAir", true);
                    if (man.CheckIfAbilityIsUnlocked(GameManager.ability.WALLJHANGANDJUMP))
                    {
                        if (wallHang.TouchesWall() && xSpeed != 0 && wallHang.WallContactSide * xSpeed > 0 && NoControlCause != Cause.KNOCKBACK)
                        {
                            Debug.Log("hang him");
                            canFlipSprite = false;
                            wallHang.HangOnWall();
                        }
                    }
                }
                // slide
                if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.DownArrow) && xSpeed != 0 && !isJumping && isOnGround)
                {
                    if (!slide)
                    {

                        slideCol.enabled = true;
                        capsuleColl.enabled = false;
                        EnemyDetectorCol.enabled = false;
                        EnemyDetectorColSlide.enabled = true;
                        boxCol.isTrigger = true;
                        anim.SetBool("Slide", true);
                        slide = true;
                        TakeControlFromPlayer(Cause.SLIDE);
                        myCor = StartCoroutine(SlideFunc());

                        canFlipSprite = false;
                        slideDirection = flipSide;
                    }
                }
                // invicibility
                if (Input.GetButtonDown("Invincibility") && !wallHang.WallHanging)
                {
                    if (invincibility.CheckIfBarFull())
                    {
                        StartCoroutine(invincibility.InvincCor(sprite));
                    }
                }
            }
            else
            {
                if (slide)
                {
                    if (isUnderCeilling)
                    {
                        isSlidingUnderCeiling = true;
                        StopCoroutine(myCor);
                    }
                    else
                    {
                        if (isSlidingUnderCeiling)
                        {
                            slide = false;
                            stopSliding = true;
                            boxCol.isTrigger = false;
                            capsuleColl.enabled = true;
                            slideCol.enabled = false;
                            EnemyDetectorCol.enabled = true;
                            EnemyDetectorColSlide.enabled = false;
                            ReturnControlToPlayer(Cause.SLIDE);
                            anim.SetBool("Slide", false);
                            canFlipSprite = true;
                            isSlidingUnderCeiling = false;
                        }
                    }
                }
            }
        }
    }
    void Move()
    {
        if (isNotMovableByPlayer)
        {
        }
        else
        {
            //transform.Translate(new Vector2(xSpeed * Time.deltaTime, 0));
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);
        }
    }

    void OnGround()
    {
        isOnGround = Physics2D.OverlapBox(groundCheck.position, new Vector2(floorCheckX, floorCheckY),0, walkableLayers);
        if (isOnGround)
        {
            //contactSide = 0;/////////////////////////////////
        }
        else
        {
            if (myCor != null)
            {
                if(slide && !isSlidingUnderCeiling)
                {
                    StopCoroutine(myCor);
                    slide = false;
                    boxCol.isTrigger = false;
                    stopSliding = true;
                    capsuleColl.enabled = true;
                    slideCol.enabled = false;
                    EnemyDetectorCol.enabled = true;
                    EnemyDetectorColSlide.enabled = false;
                    ReturnControlToPlayer(Cause.SLIDE);
                    anim.SetBool("Slide", false);
                    canFlipSprite = true;
                }
            }
        }
    }
    void UnderCeiling()
    {
        isUnderCeilling = Physics2D.OverlapCircle(ceillingCheck.position, ceillingCheckRange, groundLayer);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, new Vector2(floorCheckX, floorCheckY));
        Gizmos.DrawWireSphere(ceillingCheck.position, ceillingCheckRange);
        //Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    IEnumerator ApplySlow(float slow)
    {
        slowedDown = true;
        float tempSpeed = moveSpeed;
        moveSpeed = moveSpeed * slow;
        yield return new WaitForSeconds(2f);
        moveSpeed = tempSpeed;
        slowedDown = false;
    }
    IEnumerator SlideFunc()
    {
        yield return new WaitForSeconds(slideDuration);
        slide = false;
        stopSliding = true;
        boxCol.isTrigger = false;
        capsuleColl.enabled = true;
        slideCol.enabled = false;
        EnemyDetectorCol.enabled = true;
        EnemyDetectorColSlide.enabled = false;
        ReturnControlToPlayer(Cause.SLIDE);
        anim.SetBool("Slide", false);
        canFlipSprite = true;
    }
    
    public void ReturnControlToPlayer(Cause returnControlCause)
    {
        if (NoControlCause == Cause.NONE) return;
        if(returnControlCause == Cause.OVERRIDE)
        {
            isNotMovableByPlayer = false;
            NoControlCause = Cause.NONE;
            return;
        }
        if (NoControlCause != returnControlCause) return;
        else
        {
            isNotMovableByPlayer = false;
        }
        if(NoControlCause == Cause.AIRATTACK)
        {
            combat.ClearHitColliders();
            //hitCollsDuringAirAttack.Clear();
        }
        NoControlCause = Cause.NONE;
    }

    public void TakeControlFromPlayer(Cause takeAwayCause)
    {
        isNotMovableByPlayer = true;
        NoControlCause = takeAwayCause;
        rb.velocity = new Vector2(0, 0);
    }
    void JumpFunc()
    {
        jump = true;

    }
    void FallsDown()
    {
        if (rb.velocity.y < 0 && !isOnGround &&!combat.IsAirAttacking())
        {
            anim.SetBool("FallsDown", true);
        }
        else anim.SetBool("FallsDown", false);
    }

    public bool CanStandFromSlide()
    {
        return !Physics2D.OverlapCircle(ceillingCheck.position, ceillingCheckRange, groundLayer);
    }

    private void FlipXScale()
    {
        toFlip.localScale = new Vector3(-toFlip.localScale.x, toFlip.localScale.y, toFlip.localScale.z);
    }
  
    public void FlipPlayer(int direction)
    {
        toFlip.localScale = new Vector3(direction, toFlip.localScale.y, toFlip.localScale.z);
        flipSide = -flipSide;
    }
    public void SetCanFlipSprite(bool canFlip)
    {
        canFlipSprite = canFlip;
    }

    public void SlowPlayerDown(float slowDownFactorx, float slowDownFactory)
    {
        if (!slowedDown)
        {
            StartCoroutine(ApplySlow(slowDownFactorx));
        }
    }

    public bool CanNotBeMovedByPlayer()
    {
        return isNotMovableByPlayer;
    }
    public int GetPlayerFlipSide()
    {
        return flipSide;
    }

    public bool IsPlayerSliding()
    {
        return slide;
    }
}