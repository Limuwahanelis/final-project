using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{

    public HealthBar hpBar;
    public int maxHP;
    public int currentHP;
    public bool isInvicible;
    // Start is called before the first frame update
    void Start()
    {
        hpBar.SetMaxHealth(maxHP);
        currentHP = maxHP;
        hpBar.SetHealth(currentHP);
    }
    public void TakeDamage(int dmg)
    {
        if (isInvicible) return;
        currentHP -= dmg;
        hpBar.SetHealth(currentHP);
    }

    public void IncreaseMaxHP(int amount)
    {
        maxHP += amount;
        hpBar.SetMaxHealth(maxHP);
    }
}
