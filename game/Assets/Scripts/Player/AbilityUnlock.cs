using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlock : MonoBehaviour
{
    private GameManager man;
    public GameManager.ability abilityToUnlock;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        man = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        man.UnlockAbility(abilityToUnlock);
        Destroy(gameObject);
    }
}
