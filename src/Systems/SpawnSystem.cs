using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Serialization;
using temp1.Components;

namespace temp1.Systems
{
    class SpawnSystem : EntityUpdateSystem
    {
        MapContext _map;
        GameObjectsContext _gameObjects;
        ContentManager _content;

        bool spawned = false;
        
        public SpawnSystem(MapContext map, GameObjectsContext gameObjects, ContentManager content) : base(Aspect.One())
        {
            _map = map;
            _gameObjects = gameObjects;
            _content = content;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (spawned)
                return;
            
            var portal = CreateEntity();
            var random = new Random();
            var grid = _map.MovementGrid;
            Point point = new Point(random.Next(0, grid.width), random.Next(0, grid.height));
            if (!grid.IsWalkableAt(point.X, point.Y))
                return;
            spawned = true;
            portal.Attach(new MapObject
            {
                Position = point.ToVector2() * 32 + new Vector2(16)
            });
            portal.Attach(new RenderingObject(_content.GetAnimatedSprite("images/portal.sf")));
            portal.Attach<Expired>(new Timer(1.5f, () =>
            {
                _gameObjects.CreateMapObject("enemy", point.ToVector2() * 32 + new Vector2(16));
            }, true));
        }
    }
}