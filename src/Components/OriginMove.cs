using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;

namespace temp1.Components
{
    interface IOriginMove
    {
        void Update(GameTime gameTime);
    }

    class JumpOriginMove : IOriginMove
    {
        Sprite _sprite;
        Vector2 _origin;
        public JumpOriginMove(Sprite sprite)
        {
            _sprite = sprite;
            _origin = sprite.Origin;
        }

        public void Update(GameTime gameTime)
        {
            _sprite.Origin = _origin + new Vector2(0, (float)Math.Abs(Math.Sin(gameTime.TotalGameTime.Seconds) * 3));
        }
    }

    class SinOriginMove : IOriginMove
    {
        Sprite _sprite;
        public SinOriginMove(Sprite sprite)
        {
            _sprite = sprite;
        }

        public void Update(GameTime gameTime)
        {
            _sprite.Origin += new Vector2(0, (float)Math.Sin(gameTime.TotalGameTime.Seconds) * 3);
        }
    }
}