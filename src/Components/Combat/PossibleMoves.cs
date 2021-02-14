using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class PossibleMoves
    {
        public List<Cell> Moves;

        public bool Contains(Point p)
        {
            for (var i = 0; i < Moves.Count; i++)
            {
                if (p == Moves[i].point)
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