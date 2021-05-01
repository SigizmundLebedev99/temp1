using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;

namespace temp1.Components
{
    class JumpOriginMove : Expired
    {
        Sprite _sprite;
        Vector2 _origin;
        public JumpOriginMove(Sprite sprite)
        {
            _sprite = sprite;
            _origin = sprite.Origin;
        }

        public override bool Update(GameTime gameTime)
        {
            _sprite.Origin = _origin + new Vector2(0, (float)Math.Abs(Math.Sin(gameTime.TotalGameTime.Milliseconds / 64) * 10));
            return true;
        }
    }

    class SinOriginMove : Expired
    {
        Sprite _sprite;
        public SinOriginMove(Sprite sprite)
        {
            _sprite = sprite;
        }

        public override bool Update(GameTime gameTime)
        {
            _sprite.Origin += new Vector2(0, (float)Math.Sin(gameTime.TotalGameTime.Milliseconds / 64) * 3);
            return true;
        }
    }
}