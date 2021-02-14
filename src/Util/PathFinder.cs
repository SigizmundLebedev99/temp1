using EpPathFinding.cs;
using Microsoft.Xna.Framework;
using temp1.Components;

namespace temp1.Util
{
    class PathFinder
    {
        GameContext _context;
        AStarParam _jpParam;

        public PathFinder(GameContext context)
        {
            _context = context;
            _jpParam = new AStarParam(context.MovementGrid, 0f, DiagonalMovement.Never);
        }

        public bool FindPath(MapObject actor, Point to, out WalkAction first, out WalkAction last)
        {
            first = null;
            last = null;
            var from = actor.MapPosition;
            _jpParam.Reset(new GridPos(from.X, from.Y), new GridPos(to.X, to.Y), _context.MovementGrid);
            var path = AStarFinder.FindPath(_jpParam);
            if (path.Count < 2)
                return false;
            var node = first = new WalkAction(
                new Point(path[0].x, path[0].y),
                new Point(path[1].x, path[1].y), 
                actor, 
                1f/16f
            );
            for (var i = 2; i < path.Count; i++)
            {
                var move = new WalkAction(
                    new Point(path[i - 1].x, path[i - 1].y), 
                    new Point(path[i].x, path[i].y), 
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