using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour
{

    private GameObject UICanvas;
    public GameObject startingInvicibilityBar;
    private InvincibilityBar invincibilityBar;

    private bool isInvincible = false;
    public Color32 invincColorBlinking;
    public Color32 normalColor;
    public float invincTime;
    public int numberOfBlinks = 10;

    void Start()
    {
        UICanvas =GameObject.FindGameObjectWithTag("UICanvas") ;
        invincibilityBar =startingInvicibilityBar.GetComponent<InvincibilityBar>();
    }

    public void IncreaseProgress(float amount)
    {
        invincibilityBar.IncreaseFill(amount);
    }
    public bool CheckIfBarFull()
    {
        return invincibilityBar.CheckIfBarFull();
    }
    public void UseInvicibility()
    {
        invincibilityBar.UseInvicibility();
    }
    public IEnumerator InvincCor(SpriteRenderer sprite)
    {
        isInvincible = true;
        UseInvicibility();
        for (int i = 0; i < numberOfBlinks; i++)
        {
            sprite.color = invincColorBlinking;
            yield return new WaitForSeconds((float)((invincTime / 2) / numberOfBlinks));
            sprite.color = normalColor;
            yield return new WaitForSeconds((float)((invincTime / 2) / numberOfBlinks));
        }
        isInvincible = false;
        Debug.Log(Time.time);
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
