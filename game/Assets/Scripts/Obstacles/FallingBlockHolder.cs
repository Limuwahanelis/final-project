using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlockHolder : MonoBehaviour
{
    public Transform blockToFall;

    public Vector3 blockStartingPos;

    public float speed=3;

    private bool movingBack = false;
    // Start is called before the first frame update
    void Start()
    {
        blockStartingPos = blockToFall.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingBack) MoveBlockBack();
        if (Vector3.Distance(blockToFall.position, blockStartingPos) < 0.05) movingBack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if(!movingBack) blockToFall.GetComponent<FallingBlock>().EnableFall();
    }
    void MoveBlockBack()
    {
        blockToFall.transform.position = Vector3.MoveTowards(blockToFall.transform.position, blockStartingPos, speed * Time.deltaTime);
    }
    public void EnableReturn()
    {
        movingBack = true;
    }
}
