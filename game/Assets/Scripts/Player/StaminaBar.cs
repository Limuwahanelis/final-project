using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;

    public void SetStamina(float amount)
    {
        slider.value = amount;
    }

    public void SetMaxStamina(float amount)
    {
        slider.maxValue = amount;
    }
}
