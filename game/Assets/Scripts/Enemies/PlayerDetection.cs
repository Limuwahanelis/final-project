using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public Enemy parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        parent.SetPlayerInRange();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        parent.SetPlayerNotInRange();
    }
}
