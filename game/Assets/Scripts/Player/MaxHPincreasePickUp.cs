using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxHPincreasePickUp : MonoBehaviour,IInteractable
{
    private GameManager gameMan;
    public int maxHPincrease;
    public GameObject canvas;
    public string pickUpMessage;
    // Start is called before the first frame update
    void Start()
    {
        gameMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void Interact()
    {
        gameMan.GetPlayer().GetComponent<PlayerHealthSystem>().IncreaseMaxHP(maxHPincrease);
        Debug.Log("interact");
        gameMan.SetMessage(pickUpMessage);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canvas.SetActive(true);
        gameMan.GetPlayer().GetComponent<PlayerInteract>().setObjectToInteract(this);
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            canvas.SetActive(false);
            gameMan.GetPlayer().GetComponent<PlayerInteract>().RemoveObjectToInteract();
    }
}
