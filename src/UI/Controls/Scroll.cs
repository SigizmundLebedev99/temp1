using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using temp1.UI.DrawingPieces;
using temp1.UI.MouseReactions;

namespace temp1.UI.Controls
{
    class Scroll : Control
    {
        public MouseControl Slider { get; }
        private ClipView ClipView;
        public Control Content { get => ClipView.Content; }

        public Scroll()
        {
            Action sizeChanged = () =>
            {
                ClipView.Size = Size - new Vector2(Slider.Size.X, 0);
                Content.Size = new Vector2(ClipView.Size.X, Content.Size.Y); 
            };

            this.OnSizeChanged += sizeChanged;

            Slider = new MouseControl()
            {
                Size = new Vector2(10, 40),
                DrawingPiece = new BackgroundColorPiece(Color.Blue, Slider),
                MouseReaction = new ControlHover(new BackgroundColorPiece(Color.BlueViolet, Slider))
            };
            Slider.OnSizeChanged += sizeChanged;
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            base.Update(time, mouse, position);
            Slider.Update(time, mouse, position);
            ClipView.Update(time, mouse, position);
        }

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth = 0)
        {
            base.Draw(time, batch, position, depth);
            ClipView.Draw(time, batch, position);
            Slider.Draw(time, batch, position);
        }
    }
}