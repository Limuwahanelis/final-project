using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(HealthSystem))]
public abstract class Enemy : MonoBehaviour
{
    //[SerializeField]
    //private HealthSystem hpSys;
    // Start is called before the first frame update
    [SerializeField]
    private float invicibilityProgress=0.2f;
    public Animator anim;
    protected HealthSystem hpSys;
    public float speed;
    public int dmg;

    public event Action OnWalkEvent;
    public event Action OnAttackEvent;

    public abstract void SetPlayerInRange();
    public abstract void SetPlayerNotInRange();
    public void IncreaseInvicibilityProgress()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Invincibility>().IncreaseProgress(invicibilityProgress);
    }

    protected void RaiseOnWalkEvent()
    {
        OnWalkEvent?.Invoke();
    }
    protected void RaiseOnAttackEvent()
    {
        OnAttackEvent?.Invoke();
    }
}
