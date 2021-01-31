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
        BaseGrid _searchGrid;
        public SpawnSystem(BaseGrid searchGrid, Box playerPos) : base(Aspect.One(typeof(Enemy), typeof(Portal)))
        {
            _searchGrid = searchGrid;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (ActiveEntities.Count > 0)
                return;
            var portal = CreateEntity();
            var random = new Random();
            Point point = new Point(random.Next(0, _searchGrid.width), random.Next(0, _searchGrid.height));
            if (!_searchGrid.IsWalkableAt(point.X, point.Y))
                return;
            
            portal.Attach(new Box
            {
                Position = point.ToVector2() * 32 + new Vector2(16)
            });
            portal.Attach(ContentStorage.Portal);
            portal.Attach(new Portal());
            portal.Attach<IExpired>(new ExpiretionCallback(1.5f, () =>
            {
                var enemy = CreateEntity();
                enemy.Attach(ContentStorage.Enemy);
                enemy.Attach(new Box{
                    Position = point.ToVector2() * 32 + new Vector2(16)
                });
                enemy.Attach(new Enemy());
                enemy.Attach(new AllowedToAct());
                enemy.Attach(new Direction());
            }, true));
        }
    }
}