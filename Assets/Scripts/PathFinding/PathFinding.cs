using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AStarPathfinding
{
    public static List<Vector3> FindPath(Vector2Int start, Vector2Int target, bool[] obstacles, int gridSize)
    {
        List<Vector3> path = new List<Vector3>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        PriorityQueue<Node> openSet = new PriorityQueue<Node>();
        Dictionary<Vector2Int, Node> allNodes = new Dictionary<Vector2Int, Node>();

        Node startNode = new Node(start, null, 0, Heuristic(start, target));
        openSet.Enqueue(startNode);
        allNodes[start] = startNode;

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.Dequeue();
            if (currentNode.Position == target)
            {
                Node node = currentNode;
                while (node != null)
                {
                    path.Add(new Vector3(node.Position.x, 1.5f, node.Position.y));
                    node = node.Parent;
                }
                path.Reverse();
                break;
            }

            closedSet.Add(currentNode.Position);

            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighbor = currentNode.Position + direction;
                if (neighbor.x >= 0 && neighbor.x < gridSize && neighbor.y >= 0 && neighbor.y < gridSize)
                {
                    int index = neighbor.y * gridSize + neighbor.x;
                    if (!closedSet.Contains(neighbor) && !obstacles[index])
                    {
                        float newCostToNeighbor = currentNode.GCost + 1;

                        if (!allNodes.TryGetValue(neighbor, out Node neighborNode))
                        {
                            neighborNode = new Node(neighbor, currentNode, newCostToNeighbor, Heuristic(neighbor, target));
                            allNodes[neighbor] = neighborNode;
                            openSet.Enqueue(neighborNode);
                        }
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

    private static float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

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