using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace temp1.UI.Controls
{
    public class Desktop
    {
        private GraphicsDevice _device;
        private SpriteBatch _batch;
        
        protected SpriteSortMode SortMode;

        public Vector2 Size;

        public ContentControll Root;

        public Desktop(SpriteBatch batch, SpriteSortMode sortMode = SpriteSortMode.Deferred)
        {
            _device = batch.GraphicsDevice;
            SortMode = sortMode;
            _batch = batch;
            Size = new Vector2(_device.Viewport.Width, _device.Viewport.Height);
            Root = new ContentControll();
            Root.Size = Size;
        }

        public virtual void Draw(GameTime time)
        {
            _batch.Begin(sortMode: SortMode);
            Root.Draw(time, _batch, Root.ComputePosition(Vector2.Zero, Size));
            _batch.End();
        }

        public virtual void Update(GameTime time)
        {
            Root.Update(time, Mouse.GetState(), Root.ComputePosition(Vector2.Zero, Size));
        }
    }
}