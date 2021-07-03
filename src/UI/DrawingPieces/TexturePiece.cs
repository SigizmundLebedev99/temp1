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
        private TextureRegion2D texture;
        private bool dispose = false;

        public TexturePiece(Texture2D texture)
        {
            this.texture = new TextureRegion2D(texture);
        }

        public TexturePiece(Texture2D texture, Point size, Margin margin)
        {
            this.texture = new TextureRegion2D(StretchTexture(texture, size, margin));
            dispose = true;
        }

        public TexturePiece(TextureRegion2D texture)
        {
            this.texture = texture;
        }

        public int Width => texture.Width;

        public Rectangle Bounds => texture.Bounds;

        public int Height => texture.Height;

        public void Dispose()
        { 
            if(dispose)
                texture.Texture.Dispose();
        }

        public void Draw(SpriteBatch batch, Vector2 position, float depth = 0)
        {
            batch.Draw(this.texture, position, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);
        }

        public void Draw(SpriteBatch batch, Vector2 position, Vector2 scale, float depth = 0)
        {
            batch.Draw(this.texture, position, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, depth);
        }

        public void Draw(SpriteBatch batch, Vector2 position, Vector2 scale, float rotation, float depth = 0)
        {
            batch.Draw(this.texture, position, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, depth);
        }

        public void Update(GameTime time)
        { }

        private Texture2D StretchTexture(Texture2D texture, Point size, Margin b, bool repeat = true, SamplerState sampler = null)
        {
            if (texture == null)
                throw new InvalidOperationException("Tecture for stretching is null");
            var device = texture.GraphicsDevice;
            var batch = new SpriteBatch(device);
            var renderTarget = new RenderTarget2D(device, size.X, size.Y);

            device.SetRenderTarget(renderTarget);
            device.Clear(Color.Transparent);

            batch.Begin(samplerState: sampler);

            Vector2 destPoint;
            Rectangle source;

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

        private void Draw(SpriteBatch batch, Texture2D texture, Rectangle source, Rectangle destination, bool repeat)
        {
            if (!repeat || (source.Width >= destination.Width && source.Height >= destination.Height))
            {
                batch.Draw(texture, destination, source, Color.White);
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