using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace temp1.PathFinding
{
    public class Node : IComparable<Node>
    {
        public int x;
        public int y;
        public sbyte value;
        public float heuristicStartToEndLen; // which passes current node
        public float startToCurNodeLen;
        public float? heuristicCurNodeToEndLen;
        public bool isOpened;
        public bool isClosed;
        public Object parent;

        public Node(int iX, int iY, sbyte? iWalkable = null)
        {
            this.x = iX;
            this.y = iY;
            this.value = (iWalkable.HasValue ? iWalkable.Value : (sbyte)0);
            this.heuristicStartToEndLen = 0;
            this.startToCurNodeLen = 0;
            // this must be initialized as null to verify that its value never initialized
            // 0 is not good candidate!!
            this.heuristicCurNodeToEndLen = null;
            this.isOpened = false;
            this.isClosed = false;
            this.parent = null;
        }

        public Node(Node b)
        {
            this.x = b.x;
            this.y = b.y;
            this.value = b.value;
            this.heuristicStartToEndLen = b.heuristicStartToEndLen;
            this.startToCurNodeLen = b.startToCurNodeLen;
            this.heuristicCurNodeToEndLen = b.heuristicCurNodeToEndLen;
            this.isOpened = b.isOpened;
            this.isClosed = b.isClosed;
            this.parent = b.parent;
        }

        public void Reset(sbyte? iWalkable = null)
        {
            if (iWalkable.HasValue)
                value = iWalkable.Value;
            this.heuristicStartToEndLen = 0;
            this.startToCurNodeLen = 0;
            // this must be initialized as null to verify that its value never initialized
            // 0 is not good candidate!!
            this.heuristicCurNodeToEndLen = null;
            this.isOpened = false;
            this.isClosed = false;
            this.parent = null;
        }

        public int CompareTo(Node iObj)
        {
            float result = this.heuristicStartToEndLen - iObj.heuristicStartToEndLen;
            if (result > 0.0f)
                return 1;
            else if (result == 0.0f)
                return 0;
            return -1;
        }


        public static List<Point> Backtrace(Node iNode)
        {
            List<Point> path = new List<Point>();
            path.Add(new Point(iNode.x, iNode.y));
            while (iNode.parent != null)
            {
                iNode = (Node)iNode.parent;
                path.Add(new Point(iNode.x, iNode.y));
            }
            path.Reverse();
            return path;
        }


        public override int GetHashCode()
        {
            return x ^ y;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Node p = obj as Node;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (x == p.x) && (y == p.y);
        }

        public bool Equals(Node p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (x == p.x) && (y == p.y);
        }

        public static bool operator ==(Node a, Node b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Node a, Node b)
        {
            return !(a == b);
        }

    }
}