using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{

    private GameManager man;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        man = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
       man.GetPlayer().GetComponent<IDamagable>().TakeDamage(damage);
       man.GetPlayer().GetComponent<IDamagable>().Knockback();
    }
}
