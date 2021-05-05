using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class CanopySystem : EntityProcessingSystem
    {
        Mapper<Canopy> _hullMapper;
        Mapper<MapObject> _moMapper;
        Mapper<RenderingObject> _spriteMapper;

        public CanopySystem() : base(Aspect.All(typeof(Canopy), typeof(RenderingObject)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _hullMapper = mapperService.Get<Canopy>();
            _moMapper = mapperService.Get<MapObject>();
            _spriteMapper = mapperService.Get<RenderingObject>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var hull = _hullMapper.Get(entityId);
            var mapObjects = GameContext.World.MapObjects;
            var playerMO = _moMapper.Get(GameContext.PlayerId);
            var position = playerMO.MapPosition;
            var layer = hull.Layer;
            var tile = layer.GetTile((ushort)position.X, (ushort)position.Y);
            var sprite = _spriteMapper.Get(entityId);
            sprite.Visible = tile.IsBlank;
            
            if (!hull.IsInterier)
                return;

            RenderingObject renderingObject;

            for (var i = 0; i < mapObjects.Count; i++)
            {
                var id = mapObjects[i];
                if(id == GameContext.PlayerId)
                    continue;
                var mapObject = _moMapper.Get(id);
                renderingObject = _spriteMapper.Get(id);
                if(renderingObject == null)
                    continue;
                position = mapObject.MapPosition;
                layer = hull.Layer;
                tile = layer.GetTile((ushort)position.X, (ushort)position.Y);
                renderingObject.Visible = !sprite.Visible || tile.IsBlank;
            }

        }
    }
}