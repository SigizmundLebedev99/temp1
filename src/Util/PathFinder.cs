using Microsoft.Xna.Framework;
using temp1.Components;
using temp1.PathFinding;

namespace temp1.Util
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

        public bool FindPath(MapObject actor, Point to, out WalkAction first, out WalkAction last)
        {
            first = null;
            last = null;
            var from = actor.MapPosition;
            _param.Reset(from, to, _context.MovementGrid);
            var path = AStarFinder.FindPath(_param);
            if (path.Count < 2)
                return false;
            var node = first = new WalkAction(
                path[0],
                path[1], 
                actor, 
                1f/16f
            );
            for (var i = 2; i < path.Count; i++)
            {
                var move = new WalkAction(
                    path[i - 1], 
                    path[i], 
                    actor,
                    1f/16f
                );
                node.After = move;
                last = node = move;
            }

            return true;
        }
    }
}