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
        private SliderBox Slider;
        private ClipView ClipView;

        public Control Content { get => ClipView.Content; set { ClipView.Content = value ?? ClipView.Content; } }

        private float _scrollOffset;
        public float ScrollOffset
        {
            get => _scrollOffset;
            set
            {
                _scrollOffset = Math.Clamp(value, 0, Size.Y - Slider.Size.Y);
                Content.Offset = new Vector2(Content.Offset.X, -(Content.Size.Y - Size.Y) * ScrollOffset / (Size.Y - Slider.Size.Y));
            }
        }

        public Scroll()
        {
            Slider = new SliderBox(this);
            ClipView = new ClipView();
            Action sizeChanged = () =>
            {
                ClipView.Size = Size - new Vector2(Slider.Size.X, 0);
                Content.Size = new Vector2(ClipView.Size.X, Content.Size.Y);
            };

            this.OnSizeChanged += sizeChanged;

            Slider.OnSizeChanged += sizeChanged;
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            if (Slider.IsSelected)
            {
                ScrollOffset = mouse.Position.Y - position.Y - Slider.clickOffsetY;
            }

            base.Update(time, mouse, position);
            Slider.Update(time, mouse, Slider.ComputePosition(position, Size));
            ClipView.Update(time, mouse, position);
        }

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth = 0)
        {
            base.Draw(time, batch, position, depth);
            ClipView.Draw(time, batch, position);
            Slider.Draw(time, batch, Slider.ComputePosition(position, Size));
        }

        private class SliderBox : MouseControl
        {
            Scroll _scroll;
            public float clickOffsetY = 0;

            public SliderBox(Scroll scroll)
            {
                Size = new Vector2(15, 30);
                _scroll = scroll;
                OffsetFrom = Anchors.TopRight;
                DrawingPiece = new BackgroundColorPiece(Color.Red, this);
                MouseReaction = new ControlHover(new BackgroundColorPiece(Color.BlueViolet, this));
            }

            public override void Update(GameTime time, MouseState mouse, Vector2 position)
            {
                Offset = new Vector2(0, _scroll.ScrollOffset);
                Size = new Vector2(Size.X, Math.Clamp(_scroll.Size.Y * _scroll.Size.Y / _scroll.Content.Size.Y, 15, _scroll.Size.Y));
                base.Update(time, mouse, position);
                if (previousState != ControlMS.Pressed && State == ControlMS.Pressed)
                    clickOffsetY = mouse.Position.Y - position.Y;
            }
        }
    }
}