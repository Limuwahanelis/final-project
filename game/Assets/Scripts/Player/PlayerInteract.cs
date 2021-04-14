using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private IInteractable objectToInteract;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact"))
        {
            if(objectToInteract!=null)
            {
                objectToInteract.Interact();
            }
        }
    }
    public void setObjectToInteract(IInteractable obj)
    {
        objectToInteract = obj;
    }
    public void RemoveObjectToInteract()
    {
        objectToInteract = null;
    }
}
