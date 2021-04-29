using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using temp1.Factories;
using temp1.UI.Controls;

namespace temp1.Screens
{
    class MenuScreen : GameScreen
    {
        private Desktop _desktop;
        private ControlsFactory _factory;

        public MenuScreen(Game1 game) : base(game)
        {
            _factory = game.Services.GetService<ControlsFactory>();
            CreateMenu(game, game.Services.GetService<ScreenManager>());
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);
            
            _desktop.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            _desktop.Update(gameTime);
        }

        public void CreateMenu(Game1 game, ScreenManager screenManager)
        {
            _desktop = new Desktop(game.Batch);

            var root = new ContentControll();
            root.Size = _desktop.Size;

            var label = _factory.CreateLabel(1);
            label.OffsetFrom = Anchors.TopCenter;
            label.Offset = new Vector2(0, 20);
            label.Text = "My cool game";

            var content = new ContentControll();
            content.OffsetFrom = Anchors.Center;

            var start = _factory.CreateTextButton(0);
            start.OffsetFrom = Anchors.Center;
            start.Offset = new Vector2(0, -80);
            start.Text = "New game";
            start.MouseUp += (s, e) =>
            {
                screenManager.LoadScreen(new PlayScreen(game));
            };

            var options = _factory.CreateTextButton(0);
            options.OffsetFrom = Anchors.Center;
            options.Text = "Options";

            var exit = _factory.CreateTextButton(0);
            exit.OffsetFrom = Anchors.Center;
            exit.Offset = new Vector2(0, 80);
            exit.Text = "Exit";

            var border = _factory.CreatePanel(3);
            border.Border = new Margin(40, 40);
            border.ComputeSize(_desktop.Size - new Vector2(25), Autosize.Fill);
            border.OffsetFrom = Anchors.Center;
            border.StretchTexture();

            var panel = _factory.CreatePanel(4);
            panel.ComputeSize(_desktop.Size, Autosize.Content);

            content.Children.Add(panel);
            content.Children.Add(start);
            content.Children.Add(options);
            content.Children.Add(exit);

            content.ComputeSize(root.Size, Autosize.Content);

            root.Children.Add(border);
            root.Children.Add(label);
            root.Children.Add(content);

            _desktop.Root = root;
        }
    }
}