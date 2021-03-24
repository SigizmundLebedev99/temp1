﻿using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Squid.Skinning;
using MonoGame.Squid.Structs;
using MonoGame.Squid.Util;

namespace MonoGame.Squid.Controls
{
    /// <summary>
    /// A control that show a texture
    /// </summary>
    
    public class ImageControl : Control
    {
        private string _texture;

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>The texture.</value>
        
        public string Texture
        {
            get { return _texture; }
            set { _texture = value; TextureRect = new Rectangle(0, 0, 0, 0); }
        }

        private TextureRegion2D customRegion;

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        
        public int Color { get; set; }

        /// <summary>
        /// Gets or sets the texture rect.
        /// </summary>
        /// <value>The texture rect.</value>
        
        public Rectangle TextureRect { get; set; }

        /// <summary>
        /// Gets or sets the texture tiling
        /// </summary>
        
        public TextureMode Tiling { get; set; }

        /// <summary>
        /// Gets or sets the slice9 grid
        /// </summary>
        
        public Margin Grid { get; set; }

        public bool ExcludeFromAtlas = false;

        
        public Margin Inset { get; set; }

        public bool ColorByTint { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageControl"/> class.
        /// </summary>
        public ImageControl()
        {
            Color = -1;
            Inset = new Margin();
        }

        public void SetCustomTexture(Texture2D texture)
        {
            customRegion = new TextureRegion2D(texture, 0, 0, texture.Width, texture.Height);
        }

        public void SetCustomTexture(TextureRegion2D region){
            customRegion = region;
        }

        protected override void DrawStyle(Style style, float opacity)
        {
            base.DrawStyle(style, opacity);

            if (ColorByTint)
                Color = style.Tint;

            var color = Color;

            if (Tint != -1)
                color = ColorInt.Blend(Tint, color);

            color = ColorInt.FromArgb(opacity, color);

            if (TextureRect.IsEmpty())
            {
                var texsize = Gui.Renderer.GetTextureSize(Texture);
                TextureRect = new Rectangle(Point.Zero, texsize);
            }

            if (Tiling == TextureMode.Grid || Tiling == TextureMode.GridRepeat)
            {
                SliceTexture(Texture, Tiling, TextureRect, Grid, opacity, color);
            }
            else if (Tiling == TextureMode.Stretch)
            {
                Gui.Renderer.DrawTexture(Texture, Location.X + Inset.Left, Location.Y + Inset.Top, Size.X - (Inset.Left + Inset.Right), Size.Y - (Inset.Top + Inset.Bottom), TextureRect, color);
            }
            else if (Tiling == TextureMode.Center)
            {
                var center = Location + Size / 2;
                var rectsize = new Point(TextureRect.Width, TextureRect.Height);
                var pos = center - rectsize / 2;

                Gui.Renderer.DrawTexture(Texture, pos.X, pos.Y, rectsize.X, rectsize.Y, TextureRect, color);
            }
            else
            {
                RepeatTexture(Texture, Location, TextureRect, Tiling, opacity, color);
            }
        }
    }
}
