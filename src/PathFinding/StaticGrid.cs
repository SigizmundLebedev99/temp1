using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace temp1.PathFinding
{
    public enum DiagonalMovement
    {
        Always,
        Never,
        IfAtLeastOneWalkable,
        OnlyWhenNoObstacles
    }

    public class StaticGrid
    {
        public int width { get; private set; }

        public int height { get; private set; }

        private Node[,] m_nodes;

        public Rectangle GridRect { get; private set; }

        public StaticGrid(int iWidth, int iHeight, sbyte[,] iMatrix = null)
        {
            width = iWidth;
            height = iHeight;
            this.m_nodes = buildNodes(iWidth, iHeight, iMatrix);
        }

        public StaticGrid(StaticGrid b)
        {
            sbyte[,] tMatrix = new sbyte[b.width, b.height];
            for (int widthTrav = 0; widthTrav < b.width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < b.height; heightTrav++)
                {
                    tMatrix[widthTrav, heightTrav] = b.GetValueAt(widthTrav, heightTrav);
                }
            }
            this.m_nodes = buildNodes(b.width, b.height, tMatrix);
        }

        private sbyte GetValueAt(int widthTrav, int heightTrav)
        {
            return m_nodes[widthTrav, heightTrav].value;
        }

        public List<Node> GetNeighbors(Node iNode, DiagonalMovement diagonalMovement)
        {
            int tX = iNode.x;
            int tY = iNode.y;
            List<Node> neighbors = new List<Node>();
            bool tS0 = false, tD0 = false,
                tS1 = false, tD1 = false,
                tS2 = false, tD2 = false,
                tS3 = false, tD3 = false;

            if (this.IsWalkableAt(tX, tY - 1))
            {
                neighbors.Add(GetNodeAt(tX, tY - 1));
                tS0 = true;
            }
            if (this.IsWalkableAt(tX + 1, tY))
            {
                neighbors.Add(GetNodeAt(tX + 1, tY));
                tS1 = true;
            }
            if (this.IsWalkableAt(tX, tY + 1))
            {
                neighbors.Add(GetNodeAt(tX, tY + 1));
                tS2 = true;
            }
            if (this.IsWalkableAt(tX - 1, tY))
            {
                neighbors.Add(GetNodeAt(tX - 1, tY));
                tS3 = true;
            }

            switch (diagonalMovement)
            {
                case DiagonalMovement.Always:
                    tD0 = true;
                    tD1 = true;
                    tD2 = true;
                    tD3 = true;
                    break;
                case DiagonalMovement.Never:
                    break;
                case DiagonalMovement.IfAtLeastOneWalkable:
                    tD0 = tS3 || tS0;
                    tD1 = tS0 || tS1;
                    tD2 = tS1 || tS2;
                    tD3 = tS2 || tS3;
                    break;
                case DiagonalMovement.OnlyWhenNoObstacles:
                    tD0 = tS3 && tS0;
                    tD1 = tS0 && tS1;
                    tD2 = tS1 && tS2;
                    tD3 = tS2 && tS3;
                    break;
                default:
                    break;
            }

            if (tD0 && this.IsWalkableAt(tX - 1, tY - 1))
            {
                neighbors.Add(GetNodeAt(tX - 1, tY - 1));
            }
            if (tD1 && this.IsWalkableAt(tX + 1, tY - 1))
            {
                neighbors.Add(GetNodeAt(tX + 1, tY - 1));
            }
            if (tD2 && this.IsWalkableAt(tX + 1, tY + 1))
            {
                neighbors.Add(GetNodeAt(tX + 1, tY + 1));
            }
            if (tD3 && this.IsWalkableAt(tX - 1, tY + 1))
            {
                neighbors.Add(GetNodeAt(tX - 1, tY + 1));
            }
            return neighbors;
        }

        private Node[,] buildNodes(int iWidth, int iHeight, sbyte[,] iMatrix)
        {
            Node[,] tNodes = new Node[iWidth, iHeight];
            for (int widthTrav = 0; widthTrav < iWidth; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < iHeight; heightTrav++)
                {
                    tNodes[widthTrav,heightTrav] = new Node(widthTrav, heightTrav, null);
                }
            }

            if (iMatrix == null)
            {
                return tNodes;
            }

            if(iWidth != iMatrix.GetLength(0) || iHeight != iMatrix.GetLength(1))
                throw new ArgumentException("width or height is to match matrix width or height");

            for (int widthTrav = 0; widthTrav < iWidth; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < iHeight; heightTrav++)
                {
                    tNodes[widthTrav,heightTrav].value = iMatrix[widthTrav,heightTrav];
                }
            }
            return tNodes;
        }

        public Node GetNodeAt(int iX, int iY)
        {
            return this.m_nodes[iX,iY];
        }

        public bool IsZeroAt(int iX, int iY)
        {
            return isInside(iX, iY) && this.m_nodes[iX,iY].value == 0;
        }

        public bool IsWalkableAt(int iX, int iY)
        {
            return isInside(iX, iY) && this.m_nodes[iX,iY].value >= 0;
        }

        protected bool isInside(int iX, int iY)
        {
            return (iX >= 0 && iX < width) && (iY >= 0 && iY < height);
        }

        public bool SetValueAt(int iX, int iY, sbyte value)
        {
            this.m_nodes[iX,iY].value = value;
            return true;
        }

        protected bool isInside(Point iPos)
        {
            return isInside(iPos.X, iPos.Y);
        }

        public Node GetNodeAt(Point iPos)
        {
            return GetNodeAt(iPos.X, iPos.Y);
        }

        public bool IsWalkableAt(Point iPos)
        {
            return IsWalkableAt(iPos.X, iPos.Y);
        }

        public bool SetWalkableAt(Point iPos, sbyte value)
        {
            return SetValueAt(iPos.X, iPos.Y, value);
        }

        public void Reset()
        {
            Reset(null);
        }

        public void Reset(sbyte[][] iMatrix)
        {
            for (int widthTrav = 0; widthTrav < width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < height; heightTrav++)
                {
                    m_nodes[widthTrav,heightTrav].Reset();
                }
            }

            if (iMatrix == null)
            {
                return;
            }
            if (iMatrix.Length != width || iMatrix[0].Length != height)
            {
                throw new System.Exception("Matrix size does not fit");
            }

            for (int widthTrav = 0; widthTrav < width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < height; heightTrav++)
                {      
                    m_nodes[widthTrav,heightTrav].value = iMatrix[widthTrav][heightTrav];
                }
            }
        }
    }


}
