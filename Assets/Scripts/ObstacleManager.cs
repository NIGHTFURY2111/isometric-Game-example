using System.Collections;
using System.Collections.Generic;
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

    // Generates obstacles on the grid based on the data in obstacleData
    public void GenerateObstacles()
    {
        // Iterate over a 10x10 grid
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                int index = y * 10 + x;
                if (obstacleData.obstacleGrid[index])
                {
                    // Place the obstacle at the corresponding grid position
                    Vector3 posi = new Vector3(x, 0.5f, y);
                    Instantiate(obstaclePrefab, posi, Quaternion.identity, gridParent);
                }
            }
        }
    }
}
