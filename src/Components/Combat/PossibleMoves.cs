using System.Collections.Generic;
using EpPathFinding.cs;
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

        public bool TryGetPath(Point to, out List<GridPos> path)
        {
            path = new List<GridPos>();
            for (var i = 0; i < Moves.Count; i++)
            {
                if (to == Moves[i].point){
                    var cell = Moves[i];
                    while(cell != null){
                        path.Add(new GridPos(cell.point.X, cell.point.Y));
                        cell = cell.from;
                    }
                    path.Reverse();
                    return true;
                }
            }
            return false;
        }
    }

    class Cell
    {
        public Cell from;
        public int distance;
        public Point point;
    }
}