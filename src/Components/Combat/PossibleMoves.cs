using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class PossibleMoves
    {
        public List<Cell> Value;

        public bool Contains(Point p)
        {
            for (var i = 0; i < Value.Count; i++)
            {
                if (p == Value[i].point)
                    return true;
            }
            return false;
        }
    }

    class Cell
    {
        public int distance;
        public Point point;
    }
}