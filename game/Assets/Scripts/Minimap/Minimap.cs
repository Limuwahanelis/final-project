using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private MinimapTriggerElement el1;
    private MinimapTriggerElement el2;
    public GameObject miniCamaera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetElement(MinimapTriggerElement el)
    {
        if (el1 == null)
        {
            el1 = el;
            el1.StartBlinking();
            //miniCamaera.transform.position = el1.transform.position;
        }
        else
        {
            
            el2 = el;
        }
    }
    public void SwitchPlaces()
    {
        el1.StopBlinking();
        el1 = el2;
        el1.StartBlinking();
        
        miniCamaera.transform.position = transform.GetComponent<MinimapConfig>().GetSpritePos(el1.GetIndex());
        el2 = null;
    }
}
