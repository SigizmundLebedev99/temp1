using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace temp1.UI.Controls
{
    public class ContentControll : Control
    {
        public List<Control> Children { get; } = new List<Control>();

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth = 0)
        {
            base.Draw(time, batch, position, depth);
            for (var i = 0; i < Children.Count; i++)
                Children[i].Draw(time, batch, Children[i].ComputePosition(position, Size), depth);
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            base.Update(time, mouse, position);
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Update(time, mouse, Children[i].ComputePosition(position, Size));
            }
        }

        public void ComputeSize()
        {
            Vector2 size = Vector2.Zero;
            for (var i = 0; i < this.Children.Count; i++)
            {
                var child = Children[i];
                var childSpace = child.Size + child.Offset;
                if (childSpace.X > size.X)
                    size.X = childSpace.X;
                if (childSpace.Y > size.Y)
                    size.Y = childSpace.Y;
            }
            Size = size;
        }
    }
}