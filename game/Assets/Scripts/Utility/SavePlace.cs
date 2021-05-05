using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlace : MonoBehaviour
{
    private GameManager gamMan;
    public GameObject message;
    bool isPlayerIn = false;
    // Start is called before the first frame update
    void Start()
    {
        gamMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerIn)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                gamMan.Save();
                Debug.Log("saveddddd");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        message.SetActive(true);
        isPlayerIn = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        message.SetActive(false);
        isPlayerIn = false;
    }
}
