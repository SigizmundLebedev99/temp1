using C5;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace temp1.PathFinding
{
    public class AStarParam
    {
        public float Weight = 0f;

        public AStarParam(StaticGrid iGrid, Point iStartPos, Point iEndPos, DiagonalMovement iDiagonalMovement) : this(iGrid, iDiagonalMovement)
        {
            m_startNode = m_searchGrid.GetNodeAt(iStartPos.X, iStartPos.Y);
            m_endNode = m_searchGrid.GetNodeAt(iEndPos.X, iEndPos.Y);
            if (m_startNode == null)
                m_startNode = new Node(iStartPos.X, iStartPos.Y, 0);
            if (m_endNode == null)
                m_endNode = new Node(iEndPos.X, iEndPos.Y, 0);
        }

        public AStarParam(StaticGrid iGrid, DiagonalMovement iDiagonalMovement)
        {
            m_searchGrid = iGrid;
            DiagonalMovement = iDiagonalMovement;
            m_startNode = null;
            m_endNode = null;
        }

        public void Reset(Point iStartPos, Point iEndPos, StaticGrid iSearchGrid = null)
        {
            m_startNode = null;
            m_endNode = null;

            if (iSearchGrid != null)
                m_searchGrid = iSearchGrid;
            m_searchGrid.Reset();
            m_startNode = m_searchGrid.GetNodeAt(iStartPos.X, iStartPos.Y);
            m_endNode = m_searchGrid.GetNodeAt(iEndPos.X, iEndPos.Y);
            if (m_startNode == null)
                m_startNode = new Node(iStartPos.X, iStartPos.Y, 0);
            if (m_endNode == null)
                m_endNode = new Node(iEndPos.X, iEndPos.Y, 0);
        }

        public DiagonalMovement DiagonalMovement;

        public StaticGrid SearchGrid => m_searchGrid;

        public Node StartNode => m_startNode;

        public Node EndNode => m_endNode;


        protected StaticGrid m_searchGrid;
        protected Node m_startNode;
        protected Node m_endNode;
    }
    public static class AStarFinder
    {
        public static List<Point> FindPath(AStarParam iParam)
        {
            object lo = new object();
            //var openList = new IntervalHeap<Node>(new NodeComparer());
            var openList = new IntervalHeap<Node>();
            var startNode = iParam.StartNode;
            var endNode = iParam.EndNode;
            var grid = iParam.SearchGrid;
            var diagonalMovement = iParam.DiagonalMovement;
            var weight = iParam.Weight;


            startNode.startToCurNodeLen = 0;
            startNode.heuristicStartToEndLen = 0;

            openList.Add(startNode);
            startNode.isOpened = true;

            while (openList.Count != 0)
            {
                var node = openList.DeleteMin();
                node.isClosed = true;

                if (node == endNode)
                {
                    return Node.Backtrace(endNode);
                }

                var neighbors = grid.GetNeighbors(node);

                foreach (var neighbor in neighbors)
                {
                    if (neighbor == endNode)
                    {
                        neighbor.parent = node;
                        return Node.Backtrace(neighbor);
                    }
                    if (neighbor.isClosed || neighbor.value < 0) continue;
                    var X = neighbor.x;
                    var Y = neighbor.y;
                    float ng = node.startToCurNodeLen + (float)((X - node.x == 0 || Y - node.y == 0) ? 1 : Math.Sqrt(2));

                    if (!neighbor.isOpened || ng < neighbor.startToCurNodeLen)
                    {
                        neighbor.startToCurNodeLen = ng;
                        if (neighbor.heuristicCurNodeToEndLen == null) neighbor.heuristicCurNodeToEndLen = neighbor.value + Heuristic.Manhattan(Math.Abs(X - endNode.x), Math.Abs(Y - endNode.y));
                        neighbor.heuristicStartToEndLen = neighbor.startToCurNodeLen + neighbor.heuristicCurNodeToEndLen.Value;
                        neighbor.parent = node;
                        if (!neighbor.isOpened)
                        {
                            lock (lo)
                            {
                                openList.Add(neighbor);
                            }
                            neighbor.isOpened = true;
                        }
                        else
                        {

                        }
                    }
                }
            }
            return new List<Point>();

        }
    }
}
