using DefaultEcs;
using Microsoft.Xna.Framework;
using temp1.Components;
using temp1.Models.Movement;

namespace temp1.PathFinding
{
    class DefaultPathFinder
    {
        AStarParam _param;

        public DefaultPathFinder()
        {
            _param = new AStarParam(GameContext.Map.MovementGrid, DiagonalMovement.Never);
        }

        public bool TryGetPath(in Entity objectToMove, Point to, out MoveAction first, out MoveAction last)
        {
            first = null;
            last = null;
            var position = objectToMove.Get<Position>();
            var from = position.Value;
            _param.Reset(position.GridCell, to, GameContext.Map.MovementGrid);
            var path = AStarFinder.FindPath(_param);
            if (path.Count < 2)
                return false;
            var node = first = new MoveAction(
                position,
                new LinearMovement(from, path[1].MapPosition(), 2)
            );
            for (var i = 2; i < path.Count; i++)
            {
                var move = new MoveAction(
                    position,
                    new LinearMovement(path[i - 1].MapPosition(), path[i].MapPosition(), 2)
                );
                node.After = move;
                last = node = move;
            }

            return true;
        }
    }
}