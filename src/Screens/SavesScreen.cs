using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using temp1.UI.Controls;

namespace temp1.Screens
{
    class SavesScreen : GameScreen
    {
        Desktop _desktop;
        
        public SavesScreen(Game game) : base(game)
        {
            _desktop = new Desktop(((Game1)Game).Batch);

            var root = _desktop.Root;

            var savesScroll = new Scroll();
            savesScroll.Size = new Vector2(200, 400);
            savesScroll.OffsetFrom = Anchors.Center;

            
        }

        public override void Draw(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}