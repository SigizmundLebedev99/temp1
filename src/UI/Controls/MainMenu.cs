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
            var button = new Button();
            button.Style = "button";
            button.Text = "New game";
            button.Dock = ss.DockStyle.Center;
            button.MouseClick += (s, e) =>
            {
                sm.LoadScreen(new PlayScreen(game));
            };
            Controls.Add(button);
        }
    }
}