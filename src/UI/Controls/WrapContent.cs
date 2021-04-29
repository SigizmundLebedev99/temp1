
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace temp1.UI.Controls
{
    enum Orientation
    {
        Vertical,
        Horizontal
    }

    class WrapContent : ContentControll
    {
        public Orientation Orientation;

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position)
        {
            ComputePositions(position, (c, p) => c.Draw(time, batch, p));
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            ComputePositions(position, (c, p) => c.Update(time, mouse, p));
        }

        private void ComputePositions(Vector2 position, Action<Control, Vector2> perform)
        {
            var start = position.X;
            var maxHeight = 0f;
            for (var i = 0; i < Children.Count; i++)
            {
                if(position.X + Children[i].Size.X > start + Size.X){
                    position.X = start;
                    position.Y += maxHeight;
                    maxHeight = 0;
                }
                perform(Children[i], position);
                maxHeight = Math.Max(maxHeight, Children[i].Size.Y);
                position.X += Children[i].Size.X;
            }
        }
    }
}