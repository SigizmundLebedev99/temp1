using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Squid.Controls;
using ss = MonoGame.Squid.Structs;
using temp1.Screens;

namespace temp1.UI.Controls
{
    class MainMenu : Desktop
    {
        public MainMenu(ScreenManager sm, Game game)
        {
            Skin = Styling.Skin;
            var label = new Label();
            label.Text = "My awesome game";
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
                Dock = ss.DockStyle.Center,
                FlowDirection = ss.FlowDirection.TopToBottom,
                AutoSize = ss.AutoSize.Vertical
            };
            var ng = new Button()
            {
                Style = "button",
                Text = "New game",
                AutoSize = ss.AutoSize.Horizontal
            };
            ng.MouseClick += (s, e) =>
            {
                sm.LoadScreen(new PlayScreen(game));
            };

            var st = new Button
            {
                Style = "button",
                Text = "Options",
                AutoSize = ss.AutoSize.Horizontal
            };

            var qt = new Button
            {
                Style = "button",
                Text = "Quit",
                AutoSize = ss.AutoSize.Horizontal,
            };
            qt.MouseClick += (s, e) =>
            {
                game.Exit();
            };


            frame.Controls.Add(ng);
            frame.Controls.Add(st);
            frame.Controls.Add(qt);
            outer.Controls.Add(label);
            outer.Controls.Add(frame);
            
            Controls.Add(outer);
        }
    }
}