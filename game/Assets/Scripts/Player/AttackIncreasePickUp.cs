using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIncreasePickUp : MonoBehaviour,IInteractable
{
    private GameManager gameMan;
    public int attackIncrease;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        gameMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Interact()
    {
        gameMan.GetPlayer().GetComponent<Player>().IncraseAttackDamage(attackIncrease);
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
