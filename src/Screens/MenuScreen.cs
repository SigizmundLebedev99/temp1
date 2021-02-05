using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using Myra;
using Myra.Graphics2D.UI;
using temp1.UI;

namespace temp1.Screens
{
    class MenuScreen : GameScreen
    {
        private Desktop _desktop;
        public MenuScreen(ScreenManager manager,Game game) : base(game)
        {
            MyraEnvironment.Game = game;
            _desktop = new Desktop();
            _desktop.Root = new MainMenu(manager, game);
        }

        public override void Draw(GameTime gameTime)
        {
            _desktop.Render();
        }

        public override void Update(GameTime gameTime)
        { }
    }
}