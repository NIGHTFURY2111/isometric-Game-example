using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding
{
    // Finds a path from the start position to the target position on a grid, considering static and dynamic obstacles
    public static List<Vector3> FindPath(Vector2Int start, Vector2Int target, bool[] obstacles, int gridSize, List<Vector2Int> dynamicObstacles = null)
    {
        List<Vector3> path = new List<Vector3>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        PriorityQueue<Node> openSet = new PriorityQueue<Node>();
        Dictionary<Vector2Int, Node> allNodes = new Dictionary<Vector2Int, Node>();

        // Initialize the start node and add it to the open set
        Node startNode = new Node(start, null, 0, Heuristic(start, target));
        openSet.Enqueue(startNode);
        allNodes[start] = startNode;

        // Main loop for pathfinding
        while (openSet.Count > 0)
        {
            // Dequeue the node with the lowest F cost
            Node currentNode = openSet.Dequeue();
            if (currentNode.Position == target)
            {
                // If the target is reached, backtrack to create the path
                Node node = currentNode;
                while (node != null)
                {
                    path.Add(new Vector3(node.Position.x, 1.5f, node.Position.y));
                    node = node.Parent;
                }
                path.Reverse(); // Reverse to get the path from start to target
                break;
            }

            closedSet.Add(currentNode.Position);

            // Check each of the four possible directions (up, down, left, right)
            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighbor = currentNode.Position + direction;
                if (neighbor.x >= 0 && neighbor.x < gridSize && neighbor.y >= 0 && neighbor.y < gridSize)
                {
                    int index = neighbor.y * gridSize + neighbor.x;
                    bool isDynamicObstacle = dynamicObstacles != null && dynamicObstacles.Contains(neighbor);

                    // Check if the neighbor is not an obstacle and not in the closed set
                    if (!closedSet.Contains(neighbor) && !obstacles[index] && !isDynamicObstacle)
                    {
                        float newCostToNeighbor = currentNode.GCost + 1;

                        // If the neighbor node is not yet created, create and enqueue it
                        if (!allNodes.TryGetValue(neighbor, out Node neighborNode))
                        {
                            neighborNode = new Node(neighbor, currentNode, newCostToNeighbor, Heuristic(neighbor, target));
                            allNodes[neighbor] = neighborNode;
                            openSet.Enqueue(neighborNode);
                        }
                        // If the new path to the neighbor is shorter, update its costs and parent
                        else if (newCostToNeighbor < neighborNode.GCost)
                        {
                            neighborNode.Parent = currentNode;
                            neighborNode.GCost = newCostToNeighbor;
                            neighborNode.HCost = Heuristic(neighbor, target);

                            openSet.UpdateItem(neighborNode);
                        }
                    }
                }
            }
        }

        return path;
    }

    // Heuristic function to estimate the cost from a to b (Manhattan distance)
    private static float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    // Node class represents a point in the grid for pathfinding
    private class Node : IHeapItem<Node>
    {
        public Vector2Int Position { get; set; }
        public Node Parent { get; set; }
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost => GCost + HCost;
        public int HeapIndex { get; set; }

        public Node(Vector2Int position, Node parent, float gCost, float hCost)
        {
            Position = position;
            Parent = parent;
            GCost = gCost;
            HCost = hCost;
        }

        public int CompareTo(Node other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(other.HCost);
            }
            return compare;
        }
    }
}
