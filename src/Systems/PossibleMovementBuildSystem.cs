using System;
using System.Collections.Generic;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using temp1.Components;

namespace temp1.Systems
{
    [With(typeof(AllowedToAct))]
    [Without(typeof(BaseAction))]
    class PossibleMovementBuildSystem : AEntitySetSystem<GameTime>
    {
        HashSet<Point> searched = new HashSet<Point>();

        public PossibleMovementBuildSystem(World world) : base(world)
        { }

        protected override void Update(GameTime gameTime, in Entity entity)
        {
            if (GameContext.GameState == GameState.Peace)
                return;
            var position = entity.Get<Position>().GridCell;
            var possibleMoves = BuildMoves(position, entity);
            if (possibleMoves.Value.Count > 0)
            {
                entity.Set(possibleMoves);
            }
        }

        private PossibleMoves BuildMoves(Point mapPosition, Entity entity)
        {
            searched.Clear();
            var limit = entity.Get<ActionPoints>().Remain + 1;
            var grid = GameContext.Map.MovementGrid;
            var frontier = new Queue<Cell>(limit * limit);
            var result = new List<Cell>(limit * limit);

            Span<Point> directions = stackalloc Point[4];
            directions[0] = new Point(1, 0);
            directions[1] = new Point(-1, 0);
            directions[2] = new Point(0, 1);
            directions[3] = new Point(0, -1);

            frontier.Enqueue(new Cell
            {
                point = mapPosition,
                distance = 0
            });

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                if (!grid.IsWalkableAt(current.point.X, current.point.Y) || current.distance == limit)
                    continue;
                result.Add(current);
                for (var i = 0; i < directions.Length; i++)
                {
                    var point = current.point + directions[i];
                    if (searched.Contains(point))
                        continue;
                    frontier.Enqueue(new Cell
                    {
                        distance = current.distance + 1,
                        point = point
                    });
                    searched.Add(point);
                }
            }

            return new PossibleMoves
            {
                Value = result
            };
        }
    }
}