using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using temp1.UI;

namespace temp1.UI.DrawingPieces
{
    class TexturePiece : IDrawingPiece
    {
        private Texture2D texture;
        private DrawOptions options;

        public TexturePiece(Texture2D texture)
        {
            this.texture = texture;
            options = new DrawOptions();
        }

        public TexturePiece(Texture2D texture, DrawOptions options)
        {
            if (options.Mode == StretchMode.BorderFill || options.Mode == StretchMode.BorderRepeat)
                this.texture = DrawBorders(texture, options);
            else
                this.texture = texture;

            this.options = options;
        }

        public Vector2? Size => options.Size ?? texture.Bounds.Size.ToVector2();

        public void Dispose()
        { }

        public void Draw(SpriteBatch batch, Vector2 position, float depth = 0)
        {
            var destination = new Rectangle(position.ToPoint(), (Size.Value * options.Scale).ToPoint());
            if (options.Mode == StretchMode.Repeat)
                Draw(batch, texture, options.Source, destination, true);
            else
                batch.Draw(this.texture, destination, options.Source, Color.White, 0, options.Origin, SpriteEffects.None, depth);
        }

        public void Update(GameTime time)
        { }

        private Texture2D DrawBorders(Texture2D texture, DrawOptions options)
        {
            if (texture == null)
                throw new InvalidOperationException("Tecture for stretching is null");
            var device = texture.GraphicsDevice;
            var batch = new SpriteBatch(device);
            var size = options.Size?.ToPoint() ?? texture.Bounds.Size;
            var renderTarget = new RenderTarget2D(device, size.X, size.Y);

            device.SetRenderTarget(renderTarget);
            device.Clear(Color.Transparent);

            batch.Begin();

            Vector2 destPoint;
            Rectangle source;
            var b = options.BorderThickness;

            var repeat = options.Mode == StretchMode.BorderRepeat;

            if (b.Left != 0 && b.Top != 0)
            {
                //draw top left
                batch.Draw(texture, Vector2.Zero, new Rectangle(0, 0, b.Left, b.Top), Color.White);
            }

            if (b.Right != 0 && b.Top != 0)
            {
                //draw top right
                destPoint = new Vector2(size.X - b.Right, 0);
                source = new Rectangle(texture.Width - b.Right, 0, b.Right, b.Top);
                batch.Draw(texture, destPoint, source, Color.White);
            }

            if (b.Left != 0 && b.Bottom != 0)
            {
                //draw bottom left
                destPoint = new Vector2(0, size.Y - b.Bottom);
                source = new Rectangle(0, texture.Height - b.Bottom, b.Left, b.Bottom);
                batch.Draw(texture, destPoint, source, Color.White);
            }

            if (b.Right != 0 && b.Bottom != 0)
            {
                //draw bottom right
                destPoint = size.ToVector2() - new Vector2(b.Right, b.Bottom);
                source = new Rectangle(texture.Width - b.Right, texture.Height - b.Bottom, b.Left, b.Bottom);
                batch.Draw(texture, destPoint, source, Color.White);
            }

            Rectangle destRect;

            if (b.Left > 0)
            {
                destRect = new Rectangle(0, b.Top, b.Left, size.Y - b.Vertical);
                source = new Rectangle(0, b.Top, b.Left, texture.Height - b.Vertical);
                Draw(batch, texture, source, destRect, repeat);
            }

            if (b.Right > 0)
            {
                destRect = new Rectangle(size.X - b.Right, b.Top, b.Right, size.Y - b.Vertical);
                source = new Rectangle(texture.Width - b.Right, b.Top, b.Right, texture.Height - b.Vertical);
                Draw(batch, texture, source, destRect, repeat);
            }

            if (b.Top > 0)
            {
                destRect = new Rectangle(b.Left, 0, size.X - b.Horizontal, b.Top);
                source = new Rectangle(b.Left, 0, texture.Width - b.Horizontal, b.Top);
                Draw(batch, texture, source, destRect, repeat);
            }

            if (b.Bottom > 0)
            {
                destRect = new Rectangle(b.Left, size.Y - b.Bottom, size.X - b.Horizontal, b.Bottom);
                source = new Rectangle(b.Left, texture.Height - b.Bottom, texture.Width - b.Horizontal, b.Bottom);
                Draw(batch, texture, source, destRect, repeat);
            }

            destRect = new Rectangle(b.Left, b.Top, size.X - b.Horizontal, size.Y - b.Vertical);
            source = new Rectangle(b.Left, b.Top, texture.Width - b.Horizontal, texture.Height - b.Vertical);
            Draw(batch, texture, source, destRect, repeat);

            batch.End();

            device.SetRenderTarget(null);
            batch.Dispose();

            return renderTarget;
        }

        private void Draw(SpriteBatch batch, Texture2D texture, Rectangle? sourceRect, Rectangle destinationRect, bool repeat)
        {
            var source = sourceRect ?? texture.Bounds;
            if (!repeat || (source.Width >= destinationRect.Width && source.Height >= destinationRect.Height))
            {
                batch.Draw(texture, destinationRect, source, Color.White);
                return;
            }

            var horizontal = (int)Math.Ceiling((float)destinationRect.Width / source.Width);
            var vertical = (int)Math.Ceiling((float)destinationRect.Height / source.Height);
            var taken = new Point();

            for (var i = 0; i < horizontal; i++)
            {
                var xToTake = Math.Min(source.Width, destinationRect.Width - taken.X);
                taken.Y = 0;
                for (var j = 0; j < vertical; j++)
                {
                    var yToTake = Math.Min(source.Height, destinationRect.Height - taken.Y);
                    var dest = new Rectangle(destinationRect.X + taken.X, destinationRect.Y + taken.Y, xToTake, yToTake);
                    batch.Draw(texture, dest, new Rectangle(source.X, source.Y, xToTake, yToTake), Color.White);
                    taken.Y += yToTake;
                }
                taken.X += xToTake;
            }
        }
    }
}