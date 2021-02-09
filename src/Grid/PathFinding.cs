using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace temp1.GridSystem
{
    static class PathFinding
    {
        static SortedSet<PathNode> _openList = new SortedSet<PathNode>(new PathNodeComparer());
        static HashSet<int> _closedList = new HashSet<int>();

        public static Point[] FindPath(Point from, Point to, Grid grid)
        {
            _openList.Clear();
            _closedList.Clear();
            int index(Point p) => p.X + p.Y * grid.Width;
            Span<PathNode> nodes = stackalloc PathNode[grid.Width * grid.Height];
            Span<Point> neighbours = stackalloc Point[8];
            SetNeighbours(neighbours);

            for (var i = 0; i < grid.Width; i++)
            {
                for (var j = 0; j < grid.Height; j++)
                {
                    var node = new PathNode(i, j, grid, to);
                    nodes[node.index] = node;
                }
            }
            var startNode = nodes[index(from)];
            var endIndex = index(to);
            startNode.gCost = 0;
            nodes[startNode.index] = startNode;
            _openList.Add(startNode);

            while (_openList.Count > 0)
            {
                var currentNode = _openList.Min;
                if (currentNode.index == endIndex)
                    break;

                _openList.Remove(currentNode);
                _closedList.Add(currentNode.index);

                for (int i = 0; i < neighbours.Length; i++)
                {
                    var direction = neighbours[i];
                    var neighbour = new Point(currentNode.x + direction.X, currentNode.y + direction.Y);

                    if (!grid.Contains(neighbour.X, neighbour.Y))
                        //Point is outside grid
                        continue;

                    int neighbourIndex = index(neighbour);
                    var neighbourNode = nodes[neighbourIndex];
                    if (_closedList.Contains(neighbourIndex))
                        //Already searched this node
                        continue;

                    if (!neighbourNode.isWalkable)
                        //Not walkable
                        continue;

                    var currentPoint = new Point(currentNode.x, currentNode.y);
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentPoint, neighbour);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFrom = currentNode.index;
                        neighbourNode.gCost = tentativeGCost;
                        nodes[neighbourIndex] = neighbourNode;
                        if (!_openList.Contains(neighbourNode))
                        {
                            _openList.Add(neighbourNode);
                        }
                    }
                }
            }

            var endnode = nodes[endIndex];
            if (endnode.cameFrom == -1)
                return new Point[0];
            return GetPath(nodes, endnode);
        }

        private static Point[] GetPath(Span<PathNode> nodes, PathNode endnode)
        {
            var result = new Stack<Point>(32);
            result.Push(new Point(endnode.x, endnode.y));
            while (endnode.cameFrom != -1)
            {
                var node = nodes[endnode.cameFrom];
                result.Push(new Point(node.x, node.y));
                endnode = node;
            }
            return result.ToArray();
        }

        private static int CalculateDistanceCost(Point from, Point to)
        {
            int xDistance = Math.Abs(from.X - to.X);
            int yDistance = Math.Abs(from.Y - to.Y);
            int remain = Math.Abs(xDistance - yDistance);
            return (14 * Math.Min(xDistance, yDistance)) + (10 * remain);
        }

        private static void SetNeighbours(Span<Point> span)
        {
            span[0] = new Point(-1, 0);
            span[1] = new Point(+1, 0);
            span[2] = new Point(0, +1);
            span[3] = new Point(0, -1);
            span[4] = new Point(-1, +1);
            span[5] = new Point(-1, -1);
            span[6] = new Point(+1, +1);
            span[7] = new Point(+1, -1);
        }

        private struct PathNode
        {
            public int x;
            public int y;
            public int gCost;
            public int hCost;
            public bool isWalkable;
            public int cameFrom;
            public int index;
            public int fCost => gCost + hCost;

            public PathNode(int x, int y, Grid grid, Point to)
            {
                this.x = x;
                this.y = y;
                index = x + y * grid.Width;
                gCost = int.MaxValue;
                hCost = CalculateDistanceCost(new Point(x, y), to);
                isWalkable = grid.ValueAt(x, y);
                cameFrom = -1;
            }

            public override int GetHashCode()
            {
                return index;
            }
        }

        private struct PathNodeComparer : IComparer<PathNode>
        {
            public int Compare([AllowNull] PathNode x, [AllowNull] PathNode y)
            {
                return x.fCost - y.fCost;
            }
        }
    }
}