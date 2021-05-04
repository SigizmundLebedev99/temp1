using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class CoverSystem : EntityProcessingSystem
    {
        Mapper<Hull> _hullMapper;
        Mapper<MapObject> _moMapper;
        Mapper<RenderingObject> _spriteMapper;

        public CoverSystem() : base(Aspect.All(typeof(Hull), typeof(RenderingObject)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _hullMapper = mapperService.Get<Hull>();
            _moMapper = mapperService.Get<MapObject>();
            _spriteMapper = mapperService.Get<RenderingObject>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var hull = _hullMapper.Get(entityId);
            var playerMO = _moMapper.Get(GameContext.PlayerId);
            var sprite = _spriteMapper.Get(entityId);
            var position = playerMO.MapPosition;
            var layer = hull.Layer;
            var tile = layer.GetTile((ushort)position.X, (ushort)position.Y);
            sprite.Visible = tile.IsBlank;
        }
    }
}