using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{   
    //x and y coordinates of the tile
    int _x, _y; 

    public void SetPosition(int x, int y)
    {
        this._x = x;
        this._y = y;
    }

    #region Getters
    public int X { get =>_x; }
    public int Y { get =>_y; }
    #endregion
}
