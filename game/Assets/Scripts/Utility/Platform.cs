using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Platform : MonoBehaviour
{
    public float platformX;
    public float platformY;


    public BoxCollider2D col;
    public SpriteRenderer spriteRend;
    // Update is called once per frame
    void Update()
    {
        spriteRend.size = new Vector2(platformX, platformY);
        col.size= new Vector2 (spriteRend.size.x, platformY);
    }
}
