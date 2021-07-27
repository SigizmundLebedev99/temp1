using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using temp1.UI.Controls;
using temp1.UI.DrawingPieces;
using temp1.UI.MouseReactions;

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
            },
            new string[]{   //8 - textures for 
                "ui/battle-hot",
            }
        };

        public ControlsFactory(ContentManager manager)
        {
            _content = manager;
        }

        public MouseControl CreateButton(int resourceSet)
        {
            var button = new MouseControl();
            button.Background = LoadTxr(resourceSet, 0);
            button.MouseReaction.AddReaction(new ControlHover(LoadTxr(resourceSet, 1)));
            button.MouseReaction.AddReaction(new ControlActive(LoadTxr(resourceSet, 2)));
            return button;
        }

        public T CreateButton<T>(int resourceSet) where T : MouseControl, new()
        {
            var button = new T();
            button.Background = LoadTxr(resourceSet, 0);
            button.MouseReaction.AddReaction(new ControlHover(LoadTxr(resourceSet, 1)));
            button.MouseReaction.AddReaction(new ControlActive(LoadTxr(resourceSet, 2)));
            return button;
        }

        public MouseControl CreateTextButton(int resourceSet)
        {
            var button = new MouseControl();
            button.Background = LoadTxr(resourceSet, 0);
            button.MouseReaction.AddReaction(new ControlHover(LoadTxr(resourceSet, 1)));
            button.MouseReaction.AddReaction(new ControlActive(LoadTxr(resourceSet, 2)));
            return button;
        }

        public Control CreateLabel(int resourceSet = -1, string fontName = null)
        {
            BitmapFont font;
            if(resourceSet != -1)
                font = LoadFnt(resourceSet, 0);
            else
                font = _content.Load<BitmapFont>(fontName);
            var label = new Control();
            label.Text.Font= font;
            return label;
        }

        public Control CreatePanel(int resourceSet = 0, string textureName = null)
        {
            var panel = new Control();
            IDrawingPiece texture;
            if(textureName == null)
                texture = LoadTxr(resourceSet, 0);
            else
                texture = new TexturePiece(_content.Load<Texture2D>(textureName));
                
            panel.Background = texture;
            return panel;
        }

        private IDrawingPiece LoadTxr(int setIndex, int index)
        {
            var set = ResorceSets[setIndex];
            if (set.Length <= index)
                return null;
            if (!string.IsNullOrEmpty(set[index]))
                return new TexturePiece(_content.Load<Texture2D>(set[index]));
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