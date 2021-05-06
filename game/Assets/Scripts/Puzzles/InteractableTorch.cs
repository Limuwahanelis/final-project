using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTorch : MonoBehaviour,IInteractable
{
    private Light mainLight;
    public GameObject fire;
    private GameManager gameMan;
    public GameObject canvas;
    public bool fireActive = false;
    public InteractableTorch torch1;
    public InteractableTorch torch2;
    public int torchIndex;
    public LogicPuzzle1 LogicPuzzle1;
    // Start is called before the first frame update
    void Awake()
    {
        mainLight = transform.GetComponentInChildren<Light>();
    }
    void Start()
    {
        gameMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        LogicPuzzle1 = transform.GetComponentInParent<LogicPuzzle1>();
        //light.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (mainLight.enabled) return;
        mainLight.enabled = true;
        fireActive = !fireActive;
        fire.SetActive(fireActive);
        torch1.OtherTorchInteract();
        torch2.OtherTorchInteract();
        LogicPuzzle1.CheckIfTorchesAreLit(torchIndex);
    }
    public void OtherTorchInteract()
    {
        fireActive = !fireActive;
        fire.SetActive(fireActive);
        mainLight.enabled = !mainLight.enabled;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
            canvas.SetActive(true);
            gameMan.GetPlayer().GetComponent<PlayerInteract>().setObjectToInteract(this);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
            canvas.SetActive(false);
            gameMan.GetPlayer().GetComponent<PlayerInteract>().RemoveObjectToInteract();
    }
    public bool GetState()
    {
        return fireActive;
    }
    public void LightUp()
    {
        mainLight.enabled = true;
        fire.SetActive(true);
    }
}
