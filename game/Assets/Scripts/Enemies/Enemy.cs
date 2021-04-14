using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public abstract void SetPlayerInRange();
    public abstract void SetPlayerNotInRange();
    public void IncreaseInvicibilityProgress()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Invincibility>().IncreaseProgress(invicibilityProgress);
    }
}
