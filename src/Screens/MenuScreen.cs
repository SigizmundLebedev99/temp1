using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using temp1.Factories;
using temp1.UI.Controls;

namespace temp1.Screens
{
    class MenuScreen : GameScreen
    {
        private Desktop _desktop;
        private ControlsFactory _factory;

        public MenuScreen(Game game) : base(game)
        {
            _factory = game.Services.GetService<ControlsFactory>();
            CreateMenu(game, game.Services.GetService<ScreenManager>());
        }

        public override void Draw(GameTime gameTime)
        {
            _desktop.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            _desktop.Update(gameTime);
        }

        public void CreateMenu(Game game, ScreenManager screenManager)
        {
            _desktop = new Desktop(game.GraphicsDevice);

            var root = new ContentControll();
            root.Size = _desktop.Size;
            
            var label = _factory.CreateLabel(1);
            label.OffsetFrom = Anchors.TopCenter;
            label.Offset = new Vector2(0, 20);
            label.Text = "My cool game";
            root.AddChild(label);

            var content = new ContentControll();
            content.OffsetFrom = Anchors.Center;

            var start = _factory.CreateTextButton(0);
            start.Text = "New game";
            start.MouseUp += (s,e) => {
                screenManager.LoadScreen(new PlayScreen(game));
            };

            var options = _factory.CreateTextButton(0);
            options.Offset = new Vector2(0, 60);
            options.Text = "Options";

            var exit = _factory.CreateTextButton(0);
            exit.Offset = new Vector2(0, 120);
            exit.Text = "Exit";

            content.AddChild(start);
            content.AddChild(options);
            content.AddChild(exit);

            content.ComputeSize(root.Size, Autosize.Content);

            root.AddChild(content);

            _desktop.Root = root;
        }
    }
}