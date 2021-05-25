using Microsoft.Xna.Framework;
using temp1.Components;
using temp1.PathFinding;

namespace temp1.PathFinding
{
    class PathFinder
    {
        MapContext _context;
        AStarParam _param;

        public PathFinder(MapContext context)
        {
            _context = context;
            _param = new AStarParam(context.MovementGrid, DiagonalMovement.Never);
        }

        public bool TryGetPath(Position objectToMove, Point to, out WalkAction first, out WalkAction last, float speed)
        {
            first = null;
            last = null;
            var from = objectToMove.Value;
            _param.Reset(objectToMove.GridCell, to, _context.MovementGrid);
            var path = AStarFinder.FindPath(_param);
            if (path.Count < 2)
                return false;
            var node = first = new WalkAction(
                from,
                path[1].ToVector2() * 32 + new Vector2(16),
                objectToMove,
                speed
            );
            for (var i = 2; i < path.Count; i++)
            {
                var move = new WalkAction(
                    path[i - 1].ToVector2() * 32 + new Vector2(16), 
                    path[i].ToVector2() * 32 + new Vector2(16), 
                    objectToMove,
                    speed
                );
                node.After = move;
                last = node = move;
            }

            return true;
        }
    }
}