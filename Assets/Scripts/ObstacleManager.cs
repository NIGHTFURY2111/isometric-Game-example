using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform gridParent;
    public ObstacleData obstacleData;

    private void Start()
    {
        GenerateObstacles();
    }

    public void GenerateObstacles()
    {
        //---------------iterating over the grid---------------
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++) 
            {
                int index = y * 10 + x;
                if (obstacleData.obstacleGrid[index])
                {
                    // ---------------placing the obstacle above the tile---------------
                    Vector3 posi = new Vector3(x, 0.5f, y); 
                    GameObject obstacle = Instantiate(obstaclePrefab, posi, Quaternion.identity, gridParent);
                }
                
            }
        }
    }
}
