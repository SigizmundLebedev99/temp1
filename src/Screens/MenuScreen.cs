using System;
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
            label.Text.Value = "My cool game";

            var content = new ContentControll();
            content.OffsetFrom = Anchors.Center;
            var offset = new Vector2(0, -90);

            MouseControl AddButton(string text, Action onClick)
            {
                var button = _factory.CreateTextButton(0);
                button.OffsetFrom = Anchors.Center;
                button.Offset = offset;
                button.Text.Value = text;
                button.MouseUp += (s, e) => onClick();
                offset.Y += 45;
                return button;
            }

            var start = AddButton("New game", LoadNewGame);

            var load = AddButton("Load game", () => {
                ScreenManager.LoadScreen(new SavesScreen(game));
            });

            var options = AddButton("Options", () => {});

            var exit = AddButton("Exit", () => Game.Exit());

            var border = new Control();
            border.Size = _desktop.Size - new Vector2(25);
            border.Background = new TexturePiece(game.Content.Load<Texture2D>("ui/background"), new DrawOptions(border.Size));
            border.OffsetFrom = Anchors.Center;

            var panel = _factory.CreatePanel(4);
            panel.OffsetFrom = Anchors.Center;

            content.Children.Add(panel);
            content.Children.Add(start);
            content.Children.Add(load);
            content.Children.Add(options);
            content.Children.Add(exit);

            content.ComputeSize();

            root.Children.Add(border);
            root.Children.Add(label);
            root.Children.Add(content);
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