using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace temp1.UI.Controls
{
    public class Panel : ContentControll
    {
        public Texture2D Texture;

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position)
        {
            batch.Draw(Texture, position, Color.White);
        }
    }
}