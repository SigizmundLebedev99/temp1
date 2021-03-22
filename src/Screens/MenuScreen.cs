using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Squid.Controls;
using temp1.UI.Controls;

namespace temp1.Screens
{
    class MenuScreen : GameScreen
    {
        Desktop _desktop;
        public MenuScreen(ScreenManager manager,Game game) : base(game)
        {
            _desktop = new MainMenu(manager, game);
        }

        public override void Draw(GameTime gameTime)
        {
            _desktop.Draw();
        }

        public override void Update(GameTime gameTime)
        { 
            _desktop.Update();
        }
    }
}