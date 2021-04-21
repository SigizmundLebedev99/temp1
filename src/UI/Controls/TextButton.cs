using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace temp1.UI.Controls
{
    public class TextButton : Button
    {
        private Label label;

        public TextButton()
        {
            this.label = new Label();
        }

        public BitmapFont Font
        {
            get => label.Font; set
            {
                label.Font = value;
            }
        }

        public string Text
        {
            get => label.Text; set
            {
                label.Text = value;
            }
        }

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position)
        {
            base.Draw(time, batch, position);
            label.Draw(time, batch, position);
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            base.Update(time, mouse, position);
            label.ComputeSize(Size, Autosize.Fill);
            label.Update(time, mouse, position);
        }
    }
}