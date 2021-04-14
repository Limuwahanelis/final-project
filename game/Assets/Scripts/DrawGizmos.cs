using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireCube(GetComponentInParent<Player>().wallCheck1.position, new Vector3(GetComponentInParent<Player>().wallCheckX, GetComponentInParent<Player>().wallCheckY));
    }
}
