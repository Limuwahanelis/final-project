using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrystal : MonoBehaviour
{
    public float radius = 20;
    public float curAngle = 0;
    public float angleToMove = 5;
    public float fireDelay = 0.5f;
    
    public Vector3 positionToMove;
    public Vector3 positionToShootAt;
    public GameObject sprite;
    public GameObject beam;
    public Transform beamPos;

    public bool readyTofire = false;
    public bool endAttack = false;
    public bool isBackInPos = true;

    public float speed = 5f;

    private Vector3 pos;

    private float timetoEnd = 0.6f;
    private bool isGoingBack = false;
    private bool rotate = true;
    private bool rotateToPos=false;
    private bool moveToPos = false;
    private bool rotateToTarget = false;



    //Coroutine cor;
    // Start is called before the first frame update
    void Start()
    {
        sprite.transform.localPosition = new Vector3(Mathf.Sin(Convert(curAngle)) * radius, Mathf.Cos(Convert(curAngle)) * radius);
        sprite.transform.up = sprite.transform.localPosition;
        beam.transform.position = beamPos.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        beam.transform.position = beamPos.position;
        if (rotate)
        {
            curAngle += angleToMove * Time.deltaTime;
            pos = new Vector3(Mathf.Sin(Convert(curAngle)) * radius, Mathf.Cos(Convert(curAngle)) * radius);
            sprite.transform.localPosition = pos;
            sprite.transform.up = sprite.transform.localPosition;
            if (curAngle >= 360) curAngle = 0;
        }
        else
        {
            if (!isGoingBack)
            {
                if (rotateToPos)
                {
                    sprite.transform.up = positionToMove - sprite.transform.position;
                    rotateToPos = false;
                }
                if (moveToPos)
                {
                    if (Vector3.Distance(sprite.transform.position, positionToMove) > 0.001f)
                    {
                        float step = speed * Time.deltaTime;
                        sprite.transform.position = Vector3.MoveTowards(sprite.transform.position, positionToMove, step);
                    }
                    else
                    {
                        Debug.Log("in position");
                        moveToPos = false;
                        rotateToTarget = true;
                    }
                }
                if (rotateToTarget)
                {
                    sprite.transform.up = positionToShootAt - sprite.transform.position;
                    readyTofire = true;
                }
            }
            else
            {
                sprite.transform.up = pos - sprite.transform.position;
                float step = speed * Time.deltaTime;
                if (Vector3.Distance(sprite.transform.localPosition,pos) > 0.0001f)
                {
                    sprite.transform.localPosition = Vector3.MoveTowards(sprite.transform.localPosition, pos, step);
                }
                else
                {
                    isGoingBack = false;
                    sprite.transform.localPosition = pos;
                    sprite.transform.up = sprite.transform.localPosition;
                    isBackInPos = true;
                }

            }
        }
    }

    private float Convert(float angleInDeg)
    {
        return Mathf.Deg2Rad * angleInDeg;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    IEnumerator Attack1Cor()
    {
        yield return new WaitForSeconds(fireDelay);
        beam.transform.up = sprite.transform.up;
        beam.SetActive(true);
        float time = 0;
        while (beam.transform.localScale.y < 25 && time<timetoEnd)
        {
            beam.transform.localScale = new Vector3(beam.transform.localScale.x, beam.transform.localScale.y + 0.1f, 1);
            yield return new WaitForSeconds(0.008f);
            time += 0.008f;
        }
        beam.SetActive(false);
        endAttack = true;
    }
    public void SetRotate(bool val)
    {
        rotate = val;
    }
    public void SetPostionToMove(Vector3 pos)
    {
        rotate = false;
        positionToMove = pos;
        moveToPos = true;
        rotateToPos = true;
        readyTofire = false;
        endAttack = false;
        rotateToTarget = false;
        isBackInPos = false;
        beam.transform.localScale = new Vector3(2, 1, 1);
        //if (cor != null) StopCoroutine(cor);
    }
    public void SetTarget(Vector3 target)
    {
        positionToShootAt = target;
    }
    public void FireBeam()
    {
        StartCoroutine(Attack1Cor());
    }
    public void MoveBack()
    {
        isGoingBack = true;
    }
    public void SetAttackDuration(float attackTime)
    {
        timetoEnd = attackTime;
    }
}
