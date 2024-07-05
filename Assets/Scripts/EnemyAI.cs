using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IAI
{
    public float moveSpeed; // Speed at which the enemy moves
    public ObstacleData obstacleData; // Reference to the obstacle data
    public float UpdateFrequency; // How often the path should be updated

    private int gridSize;
    public bool isMoving = false; // Flag to check if the enemy is currently moving
    private GameObject player; // Reference to the player
    private Queue<Vector3> path; // Path calculated for the enemy

    private void Start()
    {
        gridSize = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>().gridSize;
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(UpdatePathRoutine()); // Start updating the path at regular intervals
    }

    private void Update()
    {
        if (!isMoving)
        {
            MoveAlongPath(); // Move along the path if not already moving
        }
    }

    public void MoveTowardsTarget(Vector2Int targetPosition)
    {
        Vector2Int startGridPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        Vector2Int playerGridPos = new Vector2Int(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.z));

        // Pass the player's position as a dynamic obstacle to the pathfinding method
        path = new Queue<Vector3>(AStarPathfinding.FindPath(startGridPos, targetPosition, obstacleData.obstacleGrid, gridSize, new List<Vector2Int> { playerGridPos }));
    }

    public void MoveAlongPath()
    {
        if (path != null && path.Count > 0)
        {
            isMoving = true;
            StartCoroutine(MoveToPosition(path.Dequeue())); // Move to the next position in the path
        }
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime); // Move towards the target
            yield return null;
        }
        isMoving = false; // Set isMoving to false once the target is reached
    }

    private IEnumerator UpdatePathRoutine()
    {
        while (true)
        {
            if (!isMoving && !player.GetComponent<PlayerMovement>().IsMoving)
            {
                Vector2Int playerGridPos = new Vector2Int(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.z));
                List<Vector2Int> adjacentPositions = new List<Vector2Int>
                {
                    playerGridPos + Vector2Int.up,
                    playerGridPos + Vector2Int.down,
                    playerGridPos + Vector2Int.left,
                    playerGridPos + Vector2Int.right
                };

                Vector2Int closestPosition = playerGridPos;
                float minDistance = float.MaxValue;

                foreach (var pos in adjacentPositions)
                {
                    if (pos.x >= 0 && pos.x < gridSize && pos.y >= 0 && pos.y < gridSize)
                    {
                        int index = pos.y * gridSize + pos.x;
                        if (!obstacleData.obstacleGrid[index])
                        {
                            float distance = Mathf.Abs(pos.x - transform.position.x) + Mathf.Abs(pos.y - transform.position.z); // Calculate Manhattan distance

                            if (distance < minDistance)
                            {
                                closestPosition = pos;
                                minDistance = distance;
                            }
                        }
                    }
                }
                MoveTowardsTarget(closestPosition); // Move towards the closest valid position
            }
            yield return new WaitForSecondsRealtime(UpdateFrequency); // Wait for the update frequency duration before updating the path again
        }
    }
}
