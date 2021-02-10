using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class PossibleMoves
    {
        public List<Cell> Moves;
    }

    class Cell
    {
        public Cell from;
        public int distance;
        public Point point;
    }
}