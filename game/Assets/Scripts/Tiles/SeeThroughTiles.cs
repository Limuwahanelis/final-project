using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SeeThroughTiles : MonoBehaviour
{
    List<Vector3Int> cellPositions;
    private Tilemap map;
    private Color basicColor = new Color(1f, 1f, 1f, 1f);
    private Color TransparentColor = new Color(1f, 1f, 1f, 0.5f);
    private GameManager man;
    public float playerGroundColCenterX;
    public float extent;
    private bool playerInside = false;
    // Start is called before the first frame update
    void Start()
    {

        cellPositions = new List<Vector3Int>();
        man = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        map=transform.GetComponent<Tilemap>();
        Debug.Log("man is" + man);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerInside)
        {
            playerInside = true;

            BoxCollider2D playerGroundCol = man.GetPlayer().GetComponentInChildren<BoxCollider2D>();
            Debug.Log(playerGroundCol);
            playerGroundColCenterX = playerGroundCol.bounds.center.x;
            Debug.Log(playerGroundColCenterX);
            extent = playerGroundCol.bounds.extents.x;
            Debug.Log(extent);
            Vector3Int tempTile = map.WorldToCell(new Vector3(playerGroundColCenterX - extent - 0.5f, playerGroundCol.bounds.center.y));
            Debug.Log(tempTile);
            if (map.GetTile(tempTile))
            {
                GetTilesLeft(tempTile);
            }
            if (map.GetTile(map.WorldToCell(new Vector3(playerGroundColCenterX + extent + 0.5f, playerGroundCol.bounds.center.y))))
            {
                tempTile = map.WorldToCell(new Vector3(playerGroundColCenterX + extent + 0.5f, playerGroundCol.bounds.center.y));
                GetTilesRight(tempTile);
            }
            MakeTilesTransparent();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.transform.CompareTag("PlayerTWall"))
        //{
            RemoveTransparency();
            playerInside = false;
        //}
    }
    void GetTilesRight(Vector3Int firstTileToShow)
    {
        Vector3Int curTileCellPos = firstTileToShow;
        while (map.GetTile(curTileCellPos))
        {
            cellPositions.Add(curTileCellPos);
            GetTilesUp(curTileCellPos);
            GetTilesDown(curTileCellPos);
            curTileCellPos = map.WorldToCell(new Vector3(curTileCellPos.x + 1.2f, curTileCellPos.y, 0));
        }
    }
    void GetTilesLeft ( Vector3Int firstTileToShow)
    {
        Vector3Int curTileCellPos = firstTileToShow;
        while (map.GetTile(curTileCellPos))
        {
            cellPositions.Add(curTileCellPos);
            GetTilesUp(curTileCellPos);
            GetTilesDown(curTileCellPos);
            curTileCellPos = map.WorldToCell(new Vector3(curTileCellPos.x - 0.5f, curTileCellPos.y, 0));   
        }
    }
    void GetTilesUp(Vector3Int curTile)
    {
        //map.WorldToCell(new Vector3(firstTileToDestroy.x + 1.01f, firstTileToDestroy.y, 0));
        curTile = map.WorldToCell(new Vector3(curTile.x, curTile.y + 1.2f, 0));
        while (map.GetTile(curTile))
        {
            cellPositions.Add(curTile);
            curTile = map.WorldToCell(new Vector3(curTile.x, curTile.y+1.2f, 0));
        }
    }
    void GetTilesDown(Vector3Int curTile)
    {
        //map.WorldToCell(new Vector3(firstTileToDestroy.x + 1.01f, firstTileToDestroy.y, 0));
        curTile = map.WorldToCell(new Vector3(curTile.x, curTile.y - 0.5f, 0));
        while (map.GetTile(curTile))
        {
            cellPositions.Add(curTile);
            curTile = map.WorldToCell(new Vector3(curTile.x, curTile.y - 0.5f, 0));
        }
    }
    void MakeTilesTransparent()
    {

        for (int i = 0; i < cellPositions.Count; i++)
        {
            map.RemoveTileFlags(cellPositions[i], TileFlags.LockColor);
            map.SetColor(cellPositions[i], TransparentColor);
        }
    }

    void RemoveTransparency()
    {
        foreach(Vector3Int pos in cellPositions)
        {
            map.SetColor(pos, basicColor);
        }
        cellPositions.Clear();
    }
}
