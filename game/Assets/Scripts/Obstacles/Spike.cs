using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    int dmg = 40;
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("spikesall");
        if (collision.transform.CompareTag("Player"))
        {
            IDamagable player= collision.transform.GetComponent<Player>();
            player.TakeDamage(dmg);
            player.Knockback();
            Debug.Log("spikes");
        }
    }
}
