using System;
using EpPathFinding.cs;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class SpawnSystem : EntityUpdateSystem
    {
        GameContext _context;
        bool spawned = false;
        public SpawnSystem(GameContext context) : base(Aspect.One())
        {
            _context = context;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (spawned)
                return;
            spawned = true;
            var portal = CreateEntity();
            var random = new Random();
            var grid = _context.CollisionGrid;
            Point point = new Point(random.Next(0, grid.width), random.Next(0, grid.height));
            if (!grid.IsWalkableAt(point.X, point.Y))
                return;
            
            portal.Attach(new MapObject
            {
                Position = point.ToVector2() * 32 + new Vector2(16)
            });
            portal.Attach(_context.GetAnimatedSprite("animations/portal.sf"));
            portal.Attach<IExpired>(new Timer(1.5f, () =>
            {
                _context.CreateEntity("enemy", point.ToVector2() * 32 + new Vector2(16));
            }, true));
        }
    }
}