using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace temp1.UI.Controls
{
    public class ContentControll : Control
    {
        private List<Control> Children = new List<Control>();

        public void AddChild(Control child)
        {
            if (child.Size.Length() == 0)
                child.Size = this.Size - child.Offset;
            Children.Add(child);
        }

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position)
        {
            for (var i = 0; i < Children.Count; i++)
                Children[i].Draw(time, batch, Children[i].ComputePosition(position, Size));
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            for (var i = 0; i < Children.Count; i++){
                Children[i].Update(time, mouse, Children[i].ComputePosition(position, Size));
            }
        }

        public override void ComputeSize(Vector2 size, Autosize autosize)
        {
            switch (autosize)
            {
                case Autosize.None:
                    {
                        return;
                    }
                case Autosize.Fill:
                    {
                        Size = size - Offset;
                        break;
                    }
                case Autosize.Content:
                    {
                        Size = ComputeContentBounds();
                        break;
                    }
            }
        }

        protected Vector2 ComputeContentBounds()
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
            return size;
        }
    }
}