using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using temp1.UI.Controls;

namespace temp1.Factories
{
    public class ControlsFactory
    {
        private ContentManager _content;

        public ContentManager Content => _content;

        private static string[][] ResorceSets = new string[][]{
            new string[]{   // 0 - textures for menu button
                "ui/button_default",
                "ui/button_down",
                "ui/button_hot",
                "fonts/commodore64"
            },
            new string[]{   // 1 - font for label
                "fonts/sativa"
            },
            new string[]{   // 2 - textures for inventory button
                "ui/inventory",
                "ui/inventory-hot",
                "ui/inventory-pressed"
            },
            new string[]{   // 3 - texture for panel
                "ui/border"
            },
            new string[]{   // 4 - texture for panel
                "ui/panel1"
            },
            new string[]{   // 5 - texture for panel
                "ui/panel2"
            },
            new string[]{   // 6 - font for label
                "fonts/minor"
            },
            new string[]{   //7 - texture for inventory item
                "ui/cell",
                "ui/cell-hot"
            }
        };

        public ControlsFactory(ContentManager manager)
        {
            _content = manager;
        }

        public T CreateButton<T>(int resourceSet) where T : Button, new()
        {
            var button = new T();
            button.Texture = LoadTxr(resourceSet, 0);
            if (button.Texture == null)
                throw new InvalidOperationException("Button texture not found, resource set - " + resourceSet);
            button.TextureHot = LoadTxr(resourceSet, 1);
            button.TexturePress = LoadTxr(resourceSet, 2);
            button.Size = new Vector2(button.Texture.Width, button.Texture.Height);
            return button;
        }

        public TextButton CreateTextButton(int resourceSet)
        {
            var button = new TextButton();
            button.Texture = LoadTxr(resourceSet, 0);
            if (button.Texture == null)
                throw new InvalidOperationException("Button texture not found, resource set - " + resourceSet);
            button.TextureHot = LoadTxr(resourceSet, 1);
            button.TexturePress = LoadTxr(resourceSet, 2);
            button.Size = new Vector2(button.Texture.Width, button.Texture.Height);
            button.Font = LoadFnt(resourceSet, 3);
            return button;
        }

        public Label CreateLabel(int resourceSet = -1, string fontName = null)
        {
            BitmapFont font;
            if(resourceSet != -1)
                font = LoadFnt(resourceSet, 0);
            else
                font = _content.Load<BitmapFont>(fontName);
            var label = new Label();
            label.Font = font;
            return label;
        }

        public Panel CreatePanel(int resourceSet = 0, string textureName = null)
        {
            var panel = new Panel();
            Texture2D texture;
            if(textureName == null)
                texture = LoadTxr(resourceSet, 0);
            else
                texture = _content.Load<Texture2D>(textureName);
                
            if (texture == null)
                throw new InvalidOperationException("Border texture not found, resource set - " + resourceSet);
            panel.Texture = texture;
            panel.ComputeSize(Vector2.Zero, Autosize.Content);
            return panel;
        }

        private Texture2D LoadTxr(int setIndex, int index)
        {
            var set = ResorceSets[setIndex];
            if (set.Length <= index)
                return null;
            if (!string.IsNullOrEmpty(set[index]))
                return _content.Load<Texture2D>(set[index]);
            return null;
        }

        private BitmapFont LoadFnt(int setIndex, int index)
        {
            var set = ResorceSets[setIndex];
            if (set.Length <= index)
                return null;
            if (!string.IsNullOrEmpty(set[index]))
                return _content.Load<BitmapFont>(set[index]);
            return null;
        }
    }
}