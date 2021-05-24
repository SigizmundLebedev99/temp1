using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using temp1.Components;

namespace temp1.Systems
{
    [With(typeof(RenderingObject))]
    class SpriteRenderSystem : AEntitySetSystem<GameTime>
    {
        private SpriteBatch _spriteBatch;

        public SpriteRenderSystem(World world, SpriteBatch sb) : base(world)
        {
            _spriteBatch = sb;
        }

        protected override void Update(GameTime gameTime, System.ReadOnlySpan<Entity> entities)
        {
            for (var i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                var sprite = entity.Get<RenderingObject>();
                sprite.Update(gameTime);
                if (!sprite.Visible)
                    continue;

                bool hasMO = entity.Has<MapObject>();
                MapObject mo = null;
                if (hasMO) mo = entity.Get<MapObject>();
                var position = hasMO ? mo.Position : Vector2.Zero;

                var depth = hasMO ? 0.1f / mo.MapPosition.Y : sprite.Depth;
                _spriteBatch.Draw(
                    sprite.Texture,
                    position,
                    sprite.Bounds,
                    Color.White,
                    0,
                    sprite.Origin,
                    1,
                    SpriteEffects.None,
                    depth
                );
            }
        }
    }
}