using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Squid.Controls;
using ss = MonoGame.Squid.Structs;
using temp1.Screens;
using MonoGame.Squid.Util;

namespace temp1.UI.Controls
{
    class Options : Desktop
    {
        public Options(ScreenManager sm, Game game)
        {
            Skin = Styling.Skin;
            ShowCursor = true;
            CursorSet = Styling.Cursors;

            var label = new Label();
            label.Text = "Options";
            label.Style = "title";
            label.Dock = ss.DockStyle.CenterX;

            var outer = new Frame
            {
                Dock = ss.DockStyle.Fill,
                Margin = new ss.Margin(10),
                Style = "border"
            };
            var frame = new FlowLayoutFrame
            {
                Dock = ss.DockStyle.CenterX,
                FlowDirection = ss.FlowDirection.TopToBottom,
                AutoSize = ss.AutoSize.HorizontalVertical
            };
            var list = new ListBox();
            
            foreach(var (w,h) in Resolutions()){
                var item = new ListBoxItem();
                item.Text = $"{w} x {h}";
                list.Items.Add(item);
            }

            frame.Controls.Add(list);
            outer.Controls.Add(label);
            outer.Controls.Add(frame);

            Controls.Add(outer);
        }

        private Button Create(string text)
        {
            return new Button
            {
                Style = "button",
                Text = text,
                AutoSize = ss.AutoSize.Horizontal,
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