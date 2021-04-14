using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvincibilityBar : MonoBehaviour
{

    public Image fillImage;
    private Color32 normalColor;
    private bool isBarFull = false;
    public Color32 FilledColor;

    // Start is called before the first frame update
    void Start()
    {
        normalColor = fillImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IncreaseFill(float amount)
    {
        float tmp=fillImage.fillAmount+amount;
        fillImage.fillAmount = tmp;
        if (fillImage.fillAmount == 1)
        {
            fillImage.color = FilledColor;
            isBarFull = true;
        }

        //return toReturn;
    }
    public bool CheckIfBarFull()
    {
        return isBarFull;
    }
    public void UseInvicibility()
    {
        fillImage.fillAmount = 0;
        fillImage.color = normalColor;
        isBarFull = false;
    }
    public float GetFillAmount()
    {
        return fillImage.fillAmount;
    }
    public void SetFill(float amount)
    {
        fillImage.fillAmount = amount;
    }
}
