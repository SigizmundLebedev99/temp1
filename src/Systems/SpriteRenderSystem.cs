using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using temp1.Components;

namespace temp1.Systems
{
    class SpriteRenderSystem : EntitySystem, IDrawSystem, IUpdateSystem
    {
        private Mapper<RenderingObject> _spriteMapper;
        private Mapper<MapObject> _mapObjectMapper;
        private SpriteBatch _spriteBatch;

        public SpriteRenderSystem(SpriteBatch sb) : base(Aspect.All(typeof(RenderingObject), typeof(MapObject)))
        {
            _spriteBatch = sb;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteMapper = mapperService.Get<RenderingObject>();
            _mapObjectMapper = mapperService.Get<MapObject>();
        }

        public void Update(GameTime gameTime)
        {
            var entities = this.ActiveEntities;
            for(var i = 0; i < entities.Count; i ++){
                var sprite = _spriteMapper.Get(entities[i]);
                if(sprite.Sprite is AnimatedSprite animated)
                    animated.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            var entities = this.ActiveEntities;
            for(var i = 0; i < entities.Count; i ++){
                var sprite = _spriteMapper.Get(entities[i]);
                if(!sprite.Visible)
                    continue;
                var box = _mapObjectMapper.Get(entities[i]);
                _spriteBatch.Draw(
                    sprite.Texture,
                    box.Position,
                    sprite.Bounds,
                    Color.White,
                    0, 
                    sprite.Origin,
                    1,
                    SpriteEffects.None,
                    sprite.Depth + 0.1f / box.Position.Y
                );
            }
        }
    }
}