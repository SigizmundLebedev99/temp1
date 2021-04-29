using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;

namespace temp1.UI.Controls
{
    public struct Margin
    {
        public int Top;
        public int Right;
        public int Bottom;
        public int Left;

        public int Vertical { get => Top + Bottom; }
        public int Horizontal { get => Left + Right; }

        public Margin(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public Margin(int ver, int hor)
        {
            Top = Bottom = ver;
            Right = Left = hor;
        }
    }

    public class Panel : Control
    {
        public Texture2D Texture;

        public Texture2D StretchedTexture;

        public Margin Border;

        public Panel()
        {
            Border = new Margin(3, 3);
        }

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth = 0)
        {
            Texture2D texture = StretchedTexture ?? Texture;
            batch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);
        }

        public override void ComputeSize(Vector2 size, Autosize autosize)
        {
            Texture2D texture = StretchedTexture ?? Texture;
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
                        Size = new Vector2(texture.Width, texture.Height);
                        break;
                    }
            }
        }

        public void StretchTexture(bool repeat = true, SamplerState sampler = null)
        {
            if (Texture == null)
                throw new InvalidOperationException("Tecture for stretching is null");

            var device = Texture.GraphicsDevice;
            var batch = new SpriteBatch(device);
            var renderTarget = new RenderTarget2D(device, (int)Size.X, (int)Size.Y);

            device.SetRenderTarget(renderTarget);
            device.Clear(Color.Transparent);

            batch.Begin(samplerState: sampler);

            var b = Border;

            Vector2 destPoint;
            Rectangle source;

            if (b.Left != 0 && b.Top != 0)
            {
                //draw top left
                batch.Draw(Texture, Vector2.Zero, new Rectangle(0, 0, b.Left, b.Top), Color.White);
            }

            if (b.Right != 0 && b.Top != 0)
            {
                //draw top right
                destPoint = new Vector2(Size.X - b.Right, 0);
                source = new Rectangle(Texture.Width - b.Right, 0, b.Right, b.Top);
                batch.Draw(Texture, destPoint, source, Color.White);
            }

            if (b.Left != 0 && b.Bottom != 0)
            {
                //draw bottom left
                destPoint = new Vector2(0, Size.Y - b.Bottom);
                source = new Rectangle(0, Texture.Height - b.Bottom, b.Left, b.Bottom);
                batch.Draw(Texture, destPoint, source, Color.White);
            }

            if (b.Right != 0 && b.Bottom != 0)
            {
                //draw bottom right
                destPoint = Size - new Vector2(b.Right, b.Bottom);
                source = new Rectangle(Texture.Width - b.Right, Texture.Height - b.Bottom, b.Left, b.Bottom);
                batch.Draw(Texture, destPoint, source, Color.White);
            }

            Rectangle destRect;

            if (b.Left > 0)
            {
                destRect = new Rectangle(0, b.Top, b.Left, (int)Size.Y - b.Vertical);
                source = new Rectangle(0, b.Top, b.Left, Texture.Height - b.Vertical);
                Draw(batch, Texture, source, destRect, repeat);
            }

            if (b.Right > 0)
            {
                destRect = new Rectangle((int)Size.X - b.Right, b.Top, b.Right, (int)Size.Y - b.Vertical);
                source = new Rectangle(Texture.Width - b.Right, b.Top, b.Right, Texture.Height - b.Vertical);
                Draw(batch, Texture, source, destRect, repeat);
            }

            if (b.Top > 0)
            {
                destRect = new Rectangle(b.Left, 0, (int)Size.X - b.Horizontal, b.Top);
                source = new Rectangle(b.Left, 0, Texture.Width - b.Horizontal, b.Top);
                Draw(batch, Texture, source, destRect, repeat);
            }

            if (b.Bottom > 0)
            {
                destRect = new Rectangle(b.Left, (int)Size.Y - b.Bottom, (int)Size.X - b.Horizontal, b.Bottom);
                source = new Rectangle(b.Left, Texture.Height - b.Bottom, Texture.Width - b.Horizontal, b.Bottom);
                Draw(batch, Texture, source, destRect, repeat);
            }

            destRect = new Rectangle(b.Left, b.Top, (int)Size.X - b.Horizontal, (int)Size.Y - b.Vertical);
            source = new Rectangle(b.Left, b.Top, Texture.Width - b.Horizontal, Texture.Height - b.Vertical);
            Draw(batch, Texture, source, destRect, repeat);

            batch.End();

            if (StretchedTexture != null)
                StretchedTexture.Dispose();

            StretchedTexture = renderTarget;

            device.SetRenderTarget(null);
            batch.Dispose();
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        { }

        private void Draw(SpriteBatch batch, Texture2D texture, Rectangle source, Rectangle destination, bool repeat)
        {
            if (!repeat || (source.Width >= destination.Width && source.Height >= destination.Height))
            {
                batch.Draw(Texture, destination, source, Color.White);
                return;
            }

            var horizontal = (int)Math.Ceiling((float)destination.Width / source.Width);
            var vertical = (int)Math.Ceiling((float)destination.Height / source.Height);
            var taken = new Point();

            for (var i = 0; i < horizontal; i++)
            {
                var xToTake = Math.Min(source.Width, destination.Width - taken.X);
                taken.Y = 0;
                for (var j = 0; j < vertical; j++)
                {
                    var yToTake = Math.Min(source.Height, destination.Height - taken.Y);
                    var dest = new Rectangle(destination.X + taken.X, destination.Y + taken.Y, xToTake, yToTake);
                    batch.Draw(texture, dest, new Rectangle(source.X, source.Y, xToTake, yToTake), Color.White);
                    taken.Y += yToTake;
                }
                taken.X += xToTake;
            }
        }
    }
}