using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using ComboFigther.Characters;
using ComboFigther.Decor;

namespace ComboFigther.Utility
{
    class Pathfinding
    {
        // List of Rectangle obstacles
        private List<Obstacle> obstacles;
        public int nodeSize; // Size of each step (grid-like movement but on open space)
        int issueResolveGab = 5;//if target cant be reach try with little gab
        // Node class for pathfinding
        private class Node
        {
            public Vector2 Position;
            public Node Parent;
            public float GCost; // Cost from start to current node
            public float HCost; // Heuristic cost from current node to the end node
            public float FCost { get { return GCost + HCost; } }

            public Node(Vector2 position)
            {
                Position = position;
            }
        }

        public Pathfinding(List<Obstacle> obstacles, int nodeSize = 20)
        {
            this.obstacles = obstacles;
            this.nodeSize = nodeSize;
        }

        // A* Pathfinding algorithm
        public List<Vector2> FindPath(Vector2 start, Vector2 target)
        {
            if (IsObstacle(target))
            {
                Obstacle blockingObstacle = null;
                foreach (var obstacle in Manager.Obstacles)
                {
                    if (obstacle.Rectangle.Intersects(new Rectangle((int)target.X, (int)target.Y, (int)nodeSize, (int)nodeSize)))
                    {
                        blockingObstacle = obstacle;
                        break;
                    }
                }
                if (blockingObstacle != null)
                {
                    Vector2 direction = new Vector2();

                    if (blockingObstacle.Rectangle.X > target.X) direction.X = -1;
                    else direction.X = 1;
                    if (blockingObstacle.Rectangle.Y > target.Y) direction.Y = -1;
                    else direction.Y = 1;

                    while (IsObstacle(target))
                    {
                        target.X += issueResolveGab * direction.X;
                        target.Y += issueResolveGab * direction.Y;
                    }

                }
                else { return new List<Vector2>(); }
            }
            Node startNode = new Node(start);
            Node targetNode = new Node(target);


            List<Node> openList = new List<Node>(); // Nodes to explore
            HashSet<Node> closedList = new HashSet<Node>(); // Nodes already explored

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                // Get the node with the lowest FCost
                Node currentNode = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].FCost < currentNode.FCost ||
                        openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost)
                    {
                        currentNode = openList[i];
                    }
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // If we reached the target, reconstruct the path
                if (Vector2.Distance(currentNode.Position, targetNode.Position) < nodeSize)
                {
                    return ReconstructPath(currentNode);
                }

                // Explore neighbors
                foreach (Vector2 neighborPosition in GetNeighbors(currentNode.Position))
                {
                    if (IsObstacle(neighborPosition) || ContainsNode(closedList, neighborPosition))
                    {
                        continue; // Skip if it's an obstacle or already evaluated
                    }

                    Node neighborNode = new Node(neighborPosition);
                    float newMovementCostToNeighbor = currentNode.GCost + Vector2.Distance(currentNode.Position, neighborNode.Position);

                    if (newMovementCostToNeighbor < neighborNode.GCost || !ContainsNode(openList, neighborPosition))
                    {
                        neighborNode.GCost = newMovementCostToNeighbor;
                        neighborNode.HCost = Vector2.Distance(neighborNode.Position, targetNode.Position);
                        neighborNode.Parent = currentNode;

                        if (!ContainsNode(openList, neighborPosition))
                        {
                            openList.Add(neighborNode);
                        }
                    }
                }
            }

            // Return an empty list if no path was found
            return new List<Vector2>();
        }

        // Reconstruct the path by backtracking from the target node to the start node
        private List<Vector2> ReconstructPath(Node endNode)
        {
            List<Vector2> path = new List<Vector2>();
            Node currentNode = endNode;

            while (currentNode != null)
            {
                path.Add(currentNode.Position);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }

        // Get neighbors of the current node (up, down, left, right, diagonals)
        private List<Vector2> GetNeighbors(Vector2 nodePosition)
        {
            List<Vector2> neighbors = new List<Vector2>();

            Vector2[] directions = {
            new Vector2(0, nodeSize),   // Up
            new Vector2(0, -nodeSize),  // Down
            new Vector2(nodeSize, 0),   // Right
            new Vector2(-nodeSize, 0),  // Left
            new Vector2(nodeSize, nodeSize),   // Diagonal Up-Right
            new Vector2(-nodeSize, nodeSize),  // Diagonal Up-Left
            new Vector2(nodeSize, -nodeSize),  // Diagonal Down-Right
            new Vector2(-nodeSize, -nodeSize)  // Diagonal Down-Left
        };

            foreach (Vector2 direction in directions)
            {
                Vector2 neighborPos = nodePosition + direction;
                neighbors.Add(neighborPos);
            }

            return neighbors;
        }

        // Check if a position is blocked by any obstacle (Rectangle)
        private bool IsObstacle(Vector2 position)
        {
            foreach (var obstacle in Manager.Obstacles)
            {
                if (obstacle.Rectangle.Intersects(new Rectangle((int)position.X, (int)position.Y, (int)nodeSize, (int)nodeSize)))
                {
                    return true;
                }
            }

            return false;
        }

        // Check if the list contains a node at a given position
        private bool ContainsNode(IEnumerable<Node> nodeList, Vector2 position)
        {
            foreach (var node in nodeList)
            {
                if (node.Position == position)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
