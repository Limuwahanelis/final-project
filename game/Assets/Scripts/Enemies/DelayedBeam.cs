using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedBeam : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainBeam;
    public Collider2D boxTrigger;
    public int dmg;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator BeamCor()
    {
        yield return new WaitForSeconds(0.8f);
        mainBeam.SetActive(true);
        boxTrigger.enabled = true;

    }
    public void SetCor()
    {
        StartCoroutine(BeamCor());
    }
    public void DisableCor()
    {
        mainBeam.SetActive(false);
        boxTrigger.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        collision.transform.GetComponentInParent<PlayerCombat>().Knockback();
        collision.transform.GetComponentInParent<IDamagable>().TakeDamage(dmg);
    }
}
