using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerCombat : MonoBehaviour, IDamagable
{


    private PlayerHealthSystem hpSys;
    Player player;
    Animator anim;
    private Rigidbody2D rb;
    public LayerMask EnemyLayer;
    private Invincibility invincibility;

    public event Action OnAttackEvent;
    public event Action OnAirAttackEvent;

    GameManager man;
    PlayerStateManager playerStateManager;
    //air attack
    private bool checkForColliders = false;
    private bool airAttack;
    private bool canAirAttack;
    private bool airAttackOverride; // if true, some outside objects prevents air attack
    private List<Collider2D> hitCollsDuringAirAttack = new List<Collider2D>();

    [SerializeField]
    private int comboCount = 0;
    private int maxCombo = 2;
    public float maxComboDelay = 1f;
    public float airAttackSpeed = 4;
    public CapsuleCollider2D EnemyDetectorCol;

    public GameObject bombPrefab;
    public Transform bombDropPos;
    public Transform attackPos;
    public Transform toFlip;

    public float attackRange;
    public int attackDamage = 20;
    public float knockbackTime;
    private bool isKnockable = true;
    private bool knocked = false;
    private bool isInvincible = false;

    private bool isAlive=true;

    bool isAttackOnCooldown = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hpSys = GetComponent<PlayerHealthSystem>();
        invincibility = GetComponent<Invincibility>();
        man = GameManager.instance;
        playerStateManager = GetComponent<PlayerStateManager>();
    }

    private void FixedUpdate()
    {
        if (airAttack)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(airAttackSpeed * toFlip.localScale.x, 0);
        }
        if (knocked)
        {
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(-player.GetPlayerFlipSide() * 2, 2), ForceMode2D.Impulse);
            knocked = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfCanAirAttack();
        if (!player.CanNotBeMovedByPlayer())
        {
            
            // air attack
            if (Input.GetButtonDown("Attack") && !playerStateManager.isOnGround && !playerStateManager.isHangingOnWall)
            {
                if (man.CheckIfAbilityIsUnlocked(GameManager.ability.AIRATTACK))
                {
                    if (canAirAttack)
                    {
                        OnAirAttackEvent?.Invoke();
                        checkForColliders = true;
                        airAttack = true;
                        player.SetCanFlipSprite(false);
                        EnemyDetectorCol.enabled = false;
                        anim.SetBool("AirAttack", true);
                        canAirAttack = false;
                        player.TakeControlFromPlayer(Player.Cause.AIRATTACK);
                    }
                }
            }
            if (checkForColliders)
            {
                DealDMG();
            }
            // normal attack
            if (Input.GetButtonDown("Attack") && !Input.GetKey(KeyCode.DownArrow) && playerStateManager.isOnGround && !playerStateManager.isHangingOnWall)
            {
                if (!isAttackOnCooldown)
                {
                    comboCount++;
                    if (comboCount == 1) anim.SetBool("Attack1", true);
                    player.TakeControlFromPlayer(Player.Cause.ATTACK);
                }
            }
            // bomb drop
            if (Input.GetButtonDown("Attack") && Input.GetKey(KeyCode.DownArrow) && playerStateManager.isOnGround && !playerStateManager.isHangingOnWall && !player.IsPlayerSliding())
            {
                if (man.CheckIfAbilityIsUnlocked(GameManager.ability.BOMB))
                {
                    player.TakeControlFromPlayer(Player.Cause.BOMB);
                    anim.SetTrigger("Drop Bomb");
                    Instantiate(bombPrefab, bombDropPos.transform.position, bombPrefab.transform.rotation);
                    player.ReturnControlToPlayer(Player.Cause.BOMB);
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Attack") && !Input.GetKey(KeyCode.DownArrow) && playerStateManager.isOnGround && !playerStateManager.isHangingOnWall)
            {
                comboCount++;
                if (comboCount == 1) anim.SetBool("Attack1", true);
                player.TakeControlFromPlayer(Player.Cause.ATTACK);
            }
            if (checkForColliders)
            {
                DealDMG();
            }
            // air attack
            if (Input.GetButtonDown("Attack") && !playerStateManager.isOnGround && !playerStateManager.isHangingOnWall)
            {
                if (man.CheckIfAbilityIsUnlocked(GameManager.ability.AIRATTACK))
                {
                    if (canAirAttack)
                    {
                        OnAirAttackEvent?.Invoke();
                        checkForColliders = true;
                        airAttack = true;
                        player.SetCanFlipSprite(false);
                        EnemyDetectorCol.enabled = false;
                        anim.SetBool("AirAttack", true);
                        canAirAttack = false;
                        player.TakeControlFromPlayer(Player.Cause.AIRATTACK);
                    }
                }
            }
        }
    }

    public void StopCombo(int attackNum)
    {
        if (attackNum == comboCount)
        {
            for (int i = 1; i <= maxCombo; i++)
            {
                anim.SetBool("Attack" + i, false);
            }
            comboCount = 0;
            if(attackNum==1) StartCoroutine(PlayerAttackCooldownCor());

                player.ReturnControlToPlayer(Player.Cause.ATTACK);
        }
        if (comboCount > attackNum)
        {
            int temp = attackNum + 1;
            if (temp > maxCombo)
            {
                comboCount = 0;
                for (int i = 1; i <= maxCombo; i++)
                {
                    anim.SetBool("Attack" + i, false);
                }
                player.ReturnControlToPlayer(Player.Cause.ATTACK);
                return;
            }
            else
            {
                anim.SetBool("Attack" + attackNum, false);
                anim.SetBool("Attack" + temp, true);
                comboCount = temp;
            }
        }
        //player.ReturnControlToPlayer(Player.Cause.ATTACK);

    }
    public void BreakCombo()
    {
        for (int i = 1; i <= maxCombo; i++)
        {
            anim.SetBool("Attack" + i, false);
        }
        comboCount = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public void IncraseAttackDamage(int num)
    {
        attackDamage += num;
    }
    public void StopCheckingForCollidersAirAttack()
    {
        checkForColliders = false;
    }
    void AirAttackFunc()
    {
        rb.velocity = new Vector2(0, 0);
        
        if (!playerStateManager.isHangingOnWall) rb.gravityScale = 2;
        airAttack = false;
        player.SetCanFlipSprite(true);
        //canFlipSprite = true;
        anim.SetBool("AirAttack", false);
        EnemyDetectorCol.enabled = true;
        player.ReturnControlToPlayer(Player.Cause.WALLJUMP);
        player.ReturnControlToPlayer(Player.Cause.AIRATTACK);
        return;
    }
    public void TakeDamage(int dmg)
    {
        if (isAlive)
        {
            if (isInvincible || invincibility.IsInvincible()) return;
            hpSys.TakeDamage(dmg);
            if (hpSys.currentHP <= 0) Kill();
            isInvincible = true;
            StartCoroutine(RemoveInvicibility());
        }
    }
    public void Kill()
    {
        isAlive = false;
        isInvincible = true;
        isKnockable = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StopAllCoroutines();
        anim.SetTrigger("Dead");
        anim.SetBool("FallsDown", false);
        man.ShowGameOverScreen();
    }
    public void Knockback()
    {

        if (!isKnockable || invincibility.IsInvincible()) return;
        rb.velocity = new Vector2(0, 0);
        airAttackOverride = true;
        BreakCombo();
        player.ReturnControlToPlayer(Player.Cause.OVERRIDE);
        isKnockable = false;
        knocked = true;
        player.TakeControlFromPlayer(Player.Cause.KNOCKBACK);
        StartCoroutine(PushBack());
        StartCoroutine(RemoveInvicibility());
    }

    public void SlowDown(float slowDownFactorx, float slowDownFactory)
    {
        player.SlowPlayerDown(slowDownFactorx, slowDownFactory);
    }
    IEnumerator PushBack()
    {
        yield return new WaitForSeconds(knockbackTime);
        player.ReturnControlToPlayer(Player.Cause.KNOCKBACK);
        yield return new WaitForSeconds(knockbackTime);
        airAttackOverride = false;
        isKnockable = true;
    }
    IEnumerator RemoveInvicibility()
    {
        yield return new WaitForSeconds(2f);
        isInvincible = false;
    }

    IEnumerator PlayerAttackCooldownCor()
    {
        if (isAttackOnCooldown) yield break;
        isAttackOnCooldown = true;
        yield return new WaitForSeconds(1f);
        isAttackOnCooldown = false;
    }

    void CheckIfCanAirAttack()
    {
        if (!airAttackOverride)
        {
            if (playerStateManager.isOnGround || playerStateManager.isHangingOnWall) canAirAttack = true;
        }
        else canAirAttack = false;
    }
    public void DealDMG()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, EnemyLayer);
        Debug.Log("hiotdasda");
        for (int i = 0; i < hitEnemies.Length; i++)
        {
            if (airAttack)
            {
                if (!hitCollsDuringAirAttack.Contains(hitEnemies[i]))
                {
                    hitEnemies[i].transform.GetComponentInParent<IDamagable>().TakeDamage(attackDamage);
                    hitCollsDuringAirAttack.Add(hitEnemies[i]);
                }
                else continue;
            }
            Debug.Log("hit");
            hitEnemies[i].transform.GetComponentInParent<IDamagable>().TakeDamage(attackDamage);

        }

    }
    public void ClearHitColliders()
    {
        hitCollsDuringAirAttack.Clear();
    }

    public bool IsAirAttacking()
    {
        return airAttack;
    }
    //public bool IsPlayerAlive()
    //{
    //    return isAlive;
    //}
    public void InvokeAttackEvent()
    {
        OnAttackEvent?.Invoke();
    }
}
