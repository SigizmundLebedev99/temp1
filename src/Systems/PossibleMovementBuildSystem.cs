using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class PossibleMovementBuildSystem : EntityProcessingSystem
    {
        Mapper<PossibleMoves> _moveMap;
        Mapper<MapObject> _dotMap;
        GameContext _context;
        public PossibleMovementBuildSystem(GameContext context) : base(Aspect.All(typeof(TurnPartitioner)).Exclude(typeof(PossibleMoves)))
        {
            _context = context;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _moveMap = mapperService.Get<PossibleMoves>();
            _dotMap = mapperService.Get<MapObject>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var position = _dotMap.Get(entityId).MapPosition;
            var possibleMoves = BuildMoves(position);
            if(possibleMoves.Moves.Count > 0){
                _moveMap.Put(entityId, possibleMoves);
            }
        }

        private PossibleMoves BuildMoves(Point mapPosition)
        {
            var limit = 5;
            var grid = _context.MovementGrid;
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
                from = null,
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
                    frontier.Enqueue(new Cell{
                        distance = current.distance + 1,
                        point = current.point + directions[i],
                        from = current
                    });
                }
            }

            return new PossibleMoves{
                Moves = result
            };
        }
    }
}