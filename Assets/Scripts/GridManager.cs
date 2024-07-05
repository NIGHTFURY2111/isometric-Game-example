using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridSize;
    void Start () 
    {
        GenerateGrid();
    }
    void GenerateGrid()
    {
        //---------------generates a grid of size "gridSize" from origin---------------
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                TileInfo tileInfo = tile.GetComponent<TileInfo>();
                tileInfo.SetPosition(x, y);
                tile.name = "Tile_" + x + "_" + y;
            }
        }
    }
}
