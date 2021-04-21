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

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position)
        {
            if (State == ControlMS.Pressed && TexturePress != null)
                batch.Draw(TexturePress, position, Color.White);
            else if (State != ControlMS.None && TextureHot != null)
                batch.Draw(TextureHot, position, Color.White);
            else if (Texture != null)
                batch.Draw(Texture, position, Color.White);
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            base.Update(time, mouse, position);
        }
    }
}