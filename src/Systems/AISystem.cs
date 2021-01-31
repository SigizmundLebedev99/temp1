using System;
using System.Linq;
using EpPathFinding.cs;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class AISystem : EntityProcessingSystem
    {
        ComponentMapper<IMovement> _moveMapper;
        ComponentMapper<Dot> _boxMapper;
        BaseGrid _grid;
        JumpPointParam jpParam;
        public AISystem(BaseGrid grid) : base(Aspect.All(typeof(Enemy)))
        {
            _grid = grid;
            jpParam = new JumpPointParam(_grid, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Never);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _moveMapper = mapperService.GetMapper<IMovement>();
            _boxMapper = mapperService.GetMapper<Dot>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var move = _moveMapper.Get(entityId);
            if (move != null)
                return;
            var random = new Random();
            Point point;
            point = new Point(random.Next(0, _grid.width), random.Next(0, _grid.height));
            if (!_grid.IsWalkableAt(point.X, point.Y))
                return;
            var box = _boxMapper.Get(entityId);
            var from = box.MapPosition;
            if (from == point)
                return;
            jpParam.Reset(new GridPos(from.X, from.Y), new GridPos(point.X, point.Y));
            var result = JumpPointFinder.FindPath(jpParam);
            _moveMapper.Put(entityId, new PolylineMovement(
                result.Select(e =>
                    new Vector2(e.x * 32 + 16, e.y * 32 + 16))
                    .ToArray(),
                1f));
        }
    }
}