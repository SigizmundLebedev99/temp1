using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using temp1.Components;

namespace temp1.Systems
{
    class AnimationRenderSystem : EntitySystem, IDrawSystem, IUpdateSystem
    {
        private ComponentMapper<AnimatedSprite> _spriteMapper;
        private ComponentMapper<Box> _boxMapper;
        private SpriteBatch _spriteBatch;
        public AnimationRenderSystem(SpriteBatch sb) : base(Aspect.All(typeof(AnimatedSprite), typeof(Box)))
        {
            _spriteBatch = sb;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteMapper = mapperService.GetMapper<AnimatedSprite>();
            _boxMapper = mapperService.GetMapper<Box>();
        }

        public void Update(GameTime gameTime)
        {
            var entities = this.ActiveEntities;
            for(var i = 0; i < entities.Count; i ++){
                var sprite = _spriteMapper.Get(entities[i]);
                sprite.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            var entities = this.ActiveEntities;
            for(var i = 0; i < entities.Count; i ++){
                var sprite = _spriteMapper.Get(entities[i]);
                var box = _boxMapper.Get(entities[i]);
                _spriteBatch.Draw(sprite, box.Position);
            }
        }
    }
}