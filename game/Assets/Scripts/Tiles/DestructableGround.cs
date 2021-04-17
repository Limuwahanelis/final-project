using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructableGround : MonoBehaviour
{
    enum direction
    {
        LEFT,
        DOWN,
        RIGHT
    }
    [SerializeField]
    private ContactPoint2D[] contacts = new ContactPoint2D[10];
    private Tilemap map;
    private bool destroyTiles = false;
    private Vector3Int firstTileToDestroy;
    private direction dir;
    //bool checkDir = false;
    public GameObject explosion;
    public LayerMask bombLayer;
    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();
        map.SetTile(map.WorldToCell(Vector3.zero), null);
    }

    // Update is called once per frame
    void Update()
    {
        if(destroyTiles)
        {
            switch(dir)
            {
                case direction.RIGHT: StartCoroutine(DestroyTilesRight());break;
                case direction.LEFT: StartCoroutine(DestroyTilesLeft());break;
                case direction.DOWN: StartCoroutine(DestroyTilesDown());break;

            }
            destroyTiles = false;
        }
    }

    public void Destroy(float triggerRadius, Vector3 bombPos)
    {
       // Debug.Log(collision.gameObject.layer);
       // if (collision.gameObject.layer == 14)
       // {
            Debug.Log("expl");
            //float triggerRadius = collision.gameObject.GetComponent<CircleCollider2D>().radius;

            //Vector3 bombPos = collision.transform.position;

            if (map.GetTile(map.WorldToCell(new Vector3(bombPos.x - triggerRadius, bombPos.y, 0))))
            {
                firstTileToDestroy = map.WorldToCell(new Vector3(bombPos.x - triggerRadius, bombPos.y, 0));
                dir = direction.LEFT;
            }
            if (map.GetTile(map.WorldToCell(new Vector3(bombPos.x + triggerRadius, bombPos.y, 0))))
            {
                firstTileToDestroy = map.WorldToCell(new Vector3(bombPos.x + triggerRadius, bombPos.y, 0));
                dir = direction.RIGHT;
            }
            if (map.GetTile(map.WorldToCell(new Vector3(bombPos.x, bombPos.y - triggerRadius, 0))))
            {
                firstTileToDestroy = map.WorldToCell(new Vector3(bombPos.x, bombPos.y - triggerRadius, 0));
                dir = direction.DOWN;
            }
            destroyTiles = true;
            //checkDir = true;
        //}
    }
    IEnumerator DestroyTilesRight()
    {
        Vector3Int curTile = firstTileToDestroy; //map.WorldToCell(new Vector3(firstTileToDestroy.x + 1.01f, firstTileToDestroy.y, 0));
        while (map.GetTile(curTile))
        {
            map.SetTile(curTile, null);
            Vector3Int tileHigher = map.WorldToCell(new Vector3(curTile.x, curTile.y + 1.01f, 0));
            Vector3Int tileLower = map.WorldToCell(new Vector3(curTile.x, curTile.y - 0.5f, 0));
            if (map.GetTile(tileHigher))
            {
                yield return new WaitForSeconds(0.1f);
                DestroyTileAtCellPos(tileHigher);
            }
            if (map.GetTile(tileLower))
            {
                yield return new WaitForSeconds(0.1f);
                DestroyTileAtCellPos(tileLower);
            }
            DestroyTileAtCellPos(curTile);
            yield return new WaitForSeconds(0.3f);
            curTile = map.WorldToCell(new Vector3(curTile.x + 1.01f, firstTileToDestroy.y, 0));
        }
    }
    IEnumerator DestroyTilesLeft()
    {
        Vector3Int curTile = firstTileToDestroy; //map.WorldToCell(new Vector3(firstTileToDestroy.x + 1.01f, firstTileToDestroy.y, 0));
        while (map.GetTile(curTile))
        {
            map.SetTile(curTile, null);
            Vector3Int tileHigher = map.WorldToCell(new Vector3(curTile.x, curTile.y + 1.01f, 0));
            Vector3Int tileLower = map.WorldToCell(new Vector3(curTile.x, curTile.y - 0.5f, 0));
            if (map.GetTile(tileHigher))
            {
                yield return new WaitForSeconds(0.1f);
                DestroyTileAtCellPos(tileHigher);
            }
            if (map.GetTile(tileLower))
            {
                yield return new WaitForSeconds(0.1f);
                DestroyTileAtCellPos(tileLower);
            }
            DestroyTileAtCellPos(curTile);
            yield return new WaitForSeconds(0.3f);
            curTile = map.WorldToCell(new Vector3(curTile.x - 0.5f, firstTileToDestroy.y, 0));
        }
    }

    IEnumerator DestroyTilesDown()
    {
        Vector3Int curTile = firstTileToDestroy; //map.WorldToCell(new Vector3(firstTileToDestroy.x + 1.01f, firstTileToDestroy.y, 0));
        while (map.GetTile(curTile))
        {
            map.SetTile(curTile, null);
            Vector3Int tileLeft = map.WorldToCell(new Vector3(curTile.x-0.5f, curTile.y , 0));
            Vector3Int tileRight = map.WorldToCell(new Vector3(curTile.x+1.01f, curTile.y , 0));
            if (map.GetTile(tileRight))
            {
                yield return new WaitForSeconds(0.1f);
                DestroyTileAtCellPos(tileRight);
            }
            if (map.GetTile(tileLeft))
            {
                yield return new WaitForSeconds(0.1f);
                DestroyTileAtCellPos(tileLeft);
            }
            DestroyTileAtCellPos(curTile);
            yield return new WaitForSeconds(0.3f);
            curTile = map.WorldToCell(new Vector3(curTile.x , curTile.y - 0.5f, 0));
        }
    }


    void DestroyTileAtCellPos(Vector3Int cellPos)
    {
        int explosionsNum = 2;
        GameObject[] explosions = new GameObject[explosionsNum];
        for (int i = 0; i < explosionsNum; i++)
        {
            explosions[i] = Instantiate(explosion, map.CellToWorld(cellPos) + new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f)), explosion.transform.rotation, transform);
            Destroy(explosions[i], 1f);
        }

        map.SetTile(cellPos, null);
    }
}
