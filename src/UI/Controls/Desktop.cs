using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace temp1.UI.Controls
{
    public class Desktop
    {
        private GraphicsDevice _device;
        private SpriteBatch _batch;
        public Vector2 Size;

        public Control Root;

        public Desktop(GraphicsDevice device)
        {
            _device = device;
            _batch = new SpriteBatch(device);
            Size = new Vector2(device.Viewport.Width, device.Viewport.Height);
        }

        public void Draw(GameTime time)
        {
            _batch.Begin();
            Root.Draw(time, _batch, Root.ComputePosition(Vector2.Zero, Size));
            _batch.End();
        }

        public void Update(GameTime time)
        {
            Root.Update(time, Mouse.GetState(), Root.ComputePosition(Vector2.Zero, Size));
        }
    }
}