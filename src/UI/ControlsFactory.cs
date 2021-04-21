using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using temp1.UI.Controls;

namespace temp1.UI
{
    public class ControlsFactory
    {
        private ContentManager _content;

        private static string[][] ResorceSets = new string[][]{
            new string[]{   // 0 - textures for menu button
                "button_default",
                "button_down",
                "button_hot",
                "fonts/commodore64"
            },
            new string[]{   // 0 - textures for menu button
                "fonts/sativa"
            },
            new string[]{   // 0 - textures for menu button
                "inventory",
                "inventory-hot",
                "inventory-pressed"
            }
        };

        public ControlsFactory(ContentManager manager)
        {
            _content = manager;
        }

        public Button CreateButton(int resourceSet)
        {
            var textures = ResorceSets[resourceSet];
            var button = new Button();
            button.Texture = LoadTxr(textures, 0);
            if (button.Texture == null)
                throw new InvalidOperationException("Button texture not found, resource set - " + resourceSet);
            button.TextureHot = LoadTxr(textures, 1);
            button.TexturePress = LoadTxr(textures, 2);
            button.Size = new Vector2(button.Texture.Width, button.Texture.Height);
            return button;
        }

        public TextButton CreateTextButton(int resourceSet)
        {
            var resources = ResorceSets[resourceSet];
            var button = new TextButton();
            button.Texture = LoadTxr(resources, 0);
            if (button.Texture == null)
                throw new InvalidOperationException("Button texture not found, resource set - " + resourceSet);
            button.TextureHot = LoadTxr(resources, 1);
            button.TexturePress = LoadTxr(resources, 2);
            button.Size = new Vector2(button.Texture.Width, button.Texture.Height);
            button.Font = LoadFnt(resources, 3);
            return button;
        }

        public Label CreateLabel(int resoueceSet)
        {
            var fonts = ResorceSets[resoueceSet];
            var font = LoadFnt(fonts,0);
            var label = new Label();
            label.Font = font;
            label.Size = new Vector2(200, 50);
            return label;
        }

        private Texture2D LoadTxr(string[] set, int index)
        {
            if (set.Length <= index)
                return null;
            if (!string.IsNullOrEmpty(set[index]))
                return _content.Load<Texture2D>(set[index]);
            return null;
        }

        private BitmapFont LoadFnt(string[] set, int index)
        {
            if (set.Length <= index)
                return null;
            if (!string.IsNullOrEmpty(set[index]))
                return _content.Load<BitmapFont>(set[index]);
            return null;
        }
    }
}