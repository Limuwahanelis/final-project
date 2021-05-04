using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{

    public float[] position;
    public int health;
    public int maxHealth;
    public int damage;
    public bool[] abilities;
    public float invicibilityProgress;

    public PlayerData(Player player,bool[] ab,float progress,PlayerHealthSystem playerHealth,PlayerCombat playerCombat)
    {
        position = new float[3];
        abilities = new bool[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
        abilities[(int)GameManager.ability.AIRATTACK] = ab[(int)GameManager.ability.AIRATTACK];
        abilities[(int)GameManager.ability.BOMB] = ab[(int)GameManager.ability.BOMB];
        abilities[(int)GameManager.ability.WALLJHANGANDJUMP] = ab[(int)GameManager.ability.WALLJHANGANDJUMP];
        health = playerHealth.currentHP;
        maxHealth = playerHealth.maxHP;
        damage = playerCombat.attackDamage;
        invicibilityProgress = progress;
    }



}
