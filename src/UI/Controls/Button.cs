using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace temp1.UI.Controls
{
    public class Button : MouseControl
    {
        public Texture2D Texture;
        public Texture2D TextureHot;
        public Texture2D TexturePress;

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth = 0)
        {
            if (State == ControlMS.Pressed && TexturePress != null)
                batch.Draw(TexturePress, position, null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);
            else if (State != ControlMS.None && TextureHot != null)
                batch.Draw(TextureHot, position, null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);
            else if (Texture != null)
                batch.Draw(Texture, position, null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            base.Update(time, mouse, position);
        }
    }
}