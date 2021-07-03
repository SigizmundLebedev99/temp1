using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using temp1.Factories;
using temp1.UI.Controls;

namespace temp1.Screens
{
    class LoadingScreen : GameScreen
    {
        private Desktop _desktop;
        private ControlsFactory _factory;
        Action afterAction;

        public LoadingScreen(Game1 game, Action action) : base(game)
        {
            _factory = game.Services.GetService<ControlsFactory>();
            _desktop = new Desktop(game.Batch);
            var label = _factory.CreateLabel(fontName: "fonts/commodore64");
            label.OffsetFrom = Anchors.Center;
            label.Text.Value = "Loading...";
            label.ComputeSize(Vector2.Zero, Autosize.Content);
            _desktop.Root.Children.Add(label);
            afterAction = action;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);
            
            _desktop.Draw(gameTime);
        }

        private int Delay = 2;

        public override void Update(GameTime gameTime)
        {
            _desktop.Update(gameTime);
            Delay --;
            if(Delay == 0)
                afterAction();
        }
    }
}