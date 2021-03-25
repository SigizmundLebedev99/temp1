using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Squid.Controls;
using MonoGame.Squid.Structs;
using MonoGame.Squid.Util;
using temp1.UI;
using temp1.UI.Controls;

namespace temp1.Screens
{
    class MenuScreen : GameScreen
    {
        Desktop _desktop;
        public MenuScreen(ScreenManager manager,Game game) : base(game)
        {
            CreateMenu(manager, game);
        }

        public override void Draw(GameTime gameTime)
        {
            _desktop.Draw();
        }

        public override void Update(GameTime gameTime)
        { 
            _desktop.Size = new MonoGame.Squid.Structs.Point(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            _desktop.Update();
        }

        public void CreateMenu(ScreenManager sm, Game game)
        {
            _desktop = new Desktop();
            _desktop.Skin = Styling.Skin;
            _desktop.ShowCursor = true;
            _desktop.CursorSet = Styling.Cursors;

            var label = new Label();
            label.Text = "My awesome game";
            label.Style = "title";
            label.Dock = DockStyle.CenterX;

            var outer = new Frame
            {
                Dock = DockStyle.Fill,
                Margin = new Margin(10),
                Style = "border"
            };
            var frame = new FlowLayoutFrame
            {
                Dock = DockStyle.Center,
                FlowDirection = FlowDirection.TopToBottom,
                AutoSize = AutoSize.HorizontalVertical
            };
            var ng = Create("New game");
            ng.MouseClick += (s, e) =>
            {
                sm.LoadScreen(new PlayScreen(game));
            };

            var st = Create("Settings");
            st.MouseClick += (s,e) => {
                sm.LoadScreen(new OptionsScreen(sm, game));
            };

            var qt = Create("Quit");
            qt.MouseClick += (s, e) =>
            {
                game.Exit();
            };


            frame.Controls.Add(ng);
            frame.Controls.Add(st);
            frame.Controls.Add(qt);
            outer.Controls.Add(label);
            outer.Controls.Add(frame);

            _desktop.Controls.Add(outer);
        }

        private Button Create(string text)
        {
            return new Button
            {
                Style = "mainButton",
                Text = text,
                AutoSize = AutoSize.Horizontal,
                Cursor = CursorNames.Link
            };
        }
    }
}