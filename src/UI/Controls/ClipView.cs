using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace temp1.UI.Controls
{
    class ClipView : Control
    {
        private Control _content;
        public Control Content
        {
            get => _content ?? (_content = new Control() { Size = this.Size });
            set { _content = value; }
        }

        private static RasterizerState _rasterState = new RasterizerState()
        {
            ScissorTestEnable = true
        };

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            Content.Update(time, mouse, Content.ComputePosition(position, Size));
            base.Update(time, mouse, position);
        }

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth = 0)
        {
            batch.End();

            batch.GraphicsDevice.ScissorRectangle = new Rectangle(position.ToPoint(), Size.ToPoint());
            batch.Begin(rasterizerState: _rasterState);

            Content.Draw(time, batch, ComputePosition(position, Size), depth);

            batch.End();
            batch.GraphicsDevice.ScissorRectangle = default;
            batch.Begin();

            base.Draw(time, batch, position, depth);
        }
    }
}