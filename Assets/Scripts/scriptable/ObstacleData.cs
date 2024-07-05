using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//creates a menu entry for the scriptable objects
[CreateAssetMenu(fileName = "ObstacleData", menuName = "ScriptableObjects/ObstacleData", order = 1)] 
public class ObstacleData : ScriptableObject
{
    public bool[] obstacleGrid = new bool[100]; //10 x 10 grid
}
