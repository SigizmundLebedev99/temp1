using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Squid.Controls;
using MonoGame.Squid.Structs;
using MonoGame.Squid.Util;
using temp1.UI;
using s = MonoGame.Squid.Structs;

namespace temp1.Screens
{
    class OptionsScreen : GameScreen
    {
        Desktop _desktop;
        public OptionsScreen(ScreenManager manager,Game game) : base(game)
        {
            CreateOptions(manager, game);
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

        public void CreateOptions(ScreenManager sm, Game game)
        {
            _desktop = new Desktop();
            _desktop.Skin = Styling.Skin;
            _desktop.ShowCursor = true;
            _desktop.CursorSet = Styling.Cursors;

            var label = new Label();
            label.Text = "Options";
            label.Style = "title";
            label.AutoSize = AutoSize.HorizontalVertical;
            label.Dock = DockStyle.CenterX;

            var outer = new Frame
            {
                Dock = DockStyle.Fill,
                Margin = new Margin(10),
                Style = "border"
            };
            var frame = new FlowLayoutFrame
            {
                Dock = DockStyle.CenterX,
                FlowDirection = FlowDirection.TopToBottom,
                AutoSize = AutoSize.HorizontalVertical
            };

            var list = new ListBox();
            list.Size = new s.Point(200,200);
            
            foreach(var (w,h) in Resolutions()){
                var item = new ListBoxItem();
                item.Margin = new Margin(5);
                item.Padding = new Margin(3);
                item.TextAlign = Alignment.MiddleCenter;
                item.Text = $"{w} x {h}";
                item.Style = "label";
                list.Items.Add(item);
            }
            
            frame.Controls.Add(label);
            frame.Controls.Add(list);
            
            outer.Controls.Add(frame);

            _desktop.Controls.Add(outer);
        }

        private Button Create(string text)
        {
            return new Button
            {
                Text = text,
                AutoSize = AutoSize.Horizontal,
                Cursor = CursorNames.Link
            };
        }

        private (int, int)[] Resolutions(){
            return new (int, int)[]{
                (800,600),
                (1024,768),
                (1280, 768)
            };
        }
    }
}