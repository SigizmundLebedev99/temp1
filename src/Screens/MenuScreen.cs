using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using temp1.Factories;
using temp1.UI;
using temp1.UI.Controls;
using temp1.UI.DrawingPieces;

namespace temp1.Screens
{
    class MenuScreen : GameScreen
    {
        private Desktop _desktop;
        private ControlsFactory _factory;

        public MenuScreen(Game1 game) : base(game)
        {
            _factory = game.Services.GetService<ControlsFactory>();
            CreateMenu(game);
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

        public void CreateMenu(Game1 game)
        {
            _desktop = new Desktop(game.Batch);

            var root = _desktop.Root;

            var label = _factory.CreateLabel(1);
            label.OffsetFrom = Anchors.TopCenter;
            label.Offset = new Vector2(0, 40);
            label.Size = new Vector2(300, 0);
            label.Text.Value = "My cool game";

            var content = new ContentControll();
            content.OffsetFrom = Anchors.Center;

            var start = _factory.CreateTextButton(0);
            start.OffsetFrom = Anchors.Center;
            start.Offset = new Vector2(0, -80);
            start.Text.Value = "New game";
            start.MouseUp += (s, e) =>
            {
                LoadNewGame();
            };

            var options = _factory.CreateTextButton(0);
            options.OffsetFrom = Anchors.Center;
            options.Text.Value = "Options";

            var exit = _factory.CreateTextButton(0);
            exit.OffsetFrom = Anchors.Center;
            exit.Offset = new Vector2(0, 80);
            exit.Text.Value = "Exit";

            var border = new Control();
            border.Size = _desktop.Size - new Vector2(25);
            border.DrawingPiece = new TexturePiece(game.Content.Load<Texture2D>("ui/border"), border.Size.ToPoint(), new Margin(40, 40));
            border.OffsetFrom = Anchors.Center;

            var panel = _factory.CreatePanel(4);
            panel.OffsetFrom = Anchors.Center;

            content.Children.Add(panel);
            content.Children.Add(start);
            content.Children.Add(options);
            content.Children.Add(exit);

            content.ComputeSize();

            var clipView = new Scroll { Size = new Vector2(200, 50), OffsetFrom = Anchors.TopRight, Offset = new Vector2(-50, 50) };
            clipView.Content.Text.Value = "Is there any way to enable a temporary clipping mask in XNA while using a SpriteBatch? I'm drawing a bunch of Texture2D objects and Sprites in a given rectangle, and I want to make sure that said objects aren't drawn outside of that rectangle. If it's of any help, I'm already drawing my content within a Viewport, I'm not sure if I can draw, for instance, in another Viewport within that Viewport.";
            clipView.Content.Size = clipView.Content.Text.Size;
            
            root.Children.Add(border);
            root.Children.Add(label);
            root.Children.Add(content);
            root.Children.Add(clipView);
        }

        void LoadNewGame()
        {
            ScreenManager.LoadScreen(new LoadingScreen((Game1)Game, () =>
            {
                GameContext.Init((Game1)Game);

                SaveContext.LoadGame("gamedata/worldConfig.xml");

                ScreenManager.LoadScreen(new PlayScreen(Game));
            }));
        }
    }
}