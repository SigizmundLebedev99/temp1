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
        Mapper<MapObject> _moMap;
        Mapper<ActionPoints> _pointsMap;

        MapContext _context;

        HashSet<Point> searched = new HashSet<Point>();

        public PossibleMovementBuildSystem(MapContext context) : base(Aspect.All(typeof(AllowedToAct)).Exclude(typeof(BaseAction)))
        {
            _context = context;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _moveMap = mapperService.Get<PossibleMoves>();
            _moMap = mapperService.Get<MapObject>();
            _pointsMap = mapperService.Get<ActionPoints>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            if(GameContext.GameState == GameState.Peace)
                return;
            var position = _moMap.Get(entityId).MapPosition;
            var possibleMoves = BuildMoves(position, entityId);
            if (possibleMoves.Moves.Count > 0)
            {
                _moveMap.Put(entityId, possibleMoves);
            }
        }

        private PossibleMoves BuildMoves(Point mapPosition, int entityId)
        {
            searched.Clear();
            var limit = _pointsMap.Get(entityId).Remain + 1;
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
                Moves = result
            };
        }
    }
}