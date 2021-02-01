using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using temp1.Components;

namespace temp1.Systems
{
    class StaticSpriteRenderSystem : EntityDrawSystem
    {
        private Mapper<Sprite> _spriteMapper;
        private Mapper<Dot> _boxMapper;
        private SpriteBatch _spriteBatch;
        public StaticSpriteRenderSystem(SpriteBatch sb) : base(Aspect.All(typeof(Sprite), typeof(Dot)))
        {
            _spriteBatch = sb;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteMapper = mapperService.Get<Sprite>();
            _boxMapper = mapperService.Get<Dot>();
        }

        public override void Draw(GameTime gameTime)
        {
            var entities = this.ActiveEntities;
            for(var i = 0; i < entities.Count; i ++){
                var sprite = _spriteMapper.Get(entities[i]);
                var box = _boxMapper.Get(entities[i]);
                _spriteBatch.Draw(
                    sprite.TextureRegion.Texture,
                    box.Position,
                    new Rectangle(sprite.TextureRegion.X, sprite.TextureRegion.Y, sprite.TextureRegion.Width, sprite.TextureRegion.Height),
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