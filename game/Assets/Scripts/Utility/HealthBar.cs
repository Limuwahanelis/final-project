using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(int hp)
    {
        slider.value = hp;
    }

    public void SetMaxHealth(int maxHP)
    {
        slider.maxValue = maxHP;
    }
}
