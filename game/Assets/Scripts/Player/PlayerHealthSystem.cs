using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealthSystem : HealthSystem
{
    public TextMeshProUGUI mText;

    private void Awake()
    {
        Debug.Log("sssssssssss");
        hpBar.SetMaxHealth(maxHP);
        currentHP = maxHP;
        hpBar.SetHealth(currentHP);
        mText.text = currentHP.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    new public void TakeDamage(int dmg)
    {
        if (isInvicible) return;
        currentHP -= dmg;
        if (currentHP < 0) currentHP = 0;
        hpBar.SetHealth(currentHP);
        mText.text = currentHP.ToString();
    }
    new public void IncreaseMaxHP(int amount)
    {
        maxHP += amount;
        currentHP = maxHP;
        hpBar.SetMaxHealth(maxHP);
        hpBar.SetHealth(currentHP);
        mText.text = currentHP.ToString();
    }
    public void SetMaxHP(int amount)
    {
        maxHP = amount;
        currentHP = maxHP;
        hpBar.SetMaxHealth(maxHP);
        hpBar.SetHealth(currentHP);
        mText.text = currentHP.ToString();
    }
}
