using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Squid.Interfaces;
using MonoGame.Squid.Util;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MonoGame.Squid
{
    public class MonoGameSquidRenderer : ISquidRenderer
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly ContentManager _contentManager;

        [DllImport("user32.dll")]
        private static extern int GetKeyboardLayout(int dwLayout);
        [DllImport("user32.dll")]
        private static extern int GetKeyboardState(ref byte pbKeyState);
        [DllImport("user32.dll", EntryPoint = "MapVirtualKeyEx")]
        private static extern int MapVirtualKeyExA(int uCode, int uMapType, int dwhkl);
        [DllImport("user32.dll")]
        private static extern int ToAsciiEx(int uVirtKey, int uScanCode, ref byte lpKeyState, ref short lpChar, int uFlags, int dwhkl);

        private static int _keyboardLayout;
        private readonly byte[] _keyStates;
        private readonly Dictionary<string, BitmapFont> _fonts = new Dictionary<string, BitmapFont>();

        private readonly Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();

        private readonly SpriteBatch _batch;

        private readonly Texture2D _blankTexture;

        private readonly RasterizerState _rasterizer;
        private readonly SamplerState _sampler;

        public MonoGameSquidRenderer(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            _graphicsDevice = graphicsDevice;
            _contentManager = contentManager;

            _batch = new SpriteBatch(graphicsDevice);

            _blankTexture = new Texture2D(graphicsDevice, 1, 1);
            _blankTexture.SetData(new[] { new Color(255, 255, 255, 255) });

            _fonts.Add(Font.Default, _contentManager.Load<BitmapFont>("fonts/commodore64"));
            _fonts.Add("sativa", _contentManager.Load<BitmapFont>("fonts/sativa"));
            _keyboardLayout = GetKeyboardLayout(0);
            _keyStates = new byte[0x100];

            _rasterizer = new RasterizerState { ScissorTestEnable = true };

            _sampler = new SamplerState { Filter = TextureFilter.Anisotropic };
        }

        public static int VirtualKeyToScancode(int key)
        {
            return MapVirtualKeyExA(key, 0, _keyboardLayout);
        }

        public bool TranslateKey(int code, ref char character)
        {
            short lpChar = 0;
            if (GetKeyboardState(ref _keyStates[0]) == 0)
                return false;

            var result = ToAsciiEx(MapVirtualKeyExA(code, 1, _keyboardLayout), code, ref _keyStates[0], ref lpChar, 0, _keyboardLayout);
            if (result == 1)
            {
                character = (char)((ushort)lpChar);
                return true;
            }

            return false;
        }

        private Color ColorFromtInt32(int color)
        {
            var bytes = BitConverter.GetBytes(color);

            return new Color(bytes[2], bytes[1], bytes[0], bytes[3]);
        }

        public global::MonoGame.Squid.Structs.Point GetTextSize(string text, string font)
        {
            if (string.IsNullOrEmpty(text))
                return new global::MonoGame.Squid.Structs.Point();
            var f = _fonts[font];
            var size = f.MeasureString(text);
            return new global::MonoGame.Squid.Structs.Point((int)size.Width, (int)size.Height);
        }

        public global::MonoGame.Squid.Structs.Point GetTextureSize(string texture)
        {
            var tex = GetTexture(texture);
            return new global::MonoGame.Squid.Structs.Point(tex.Width, tex.Height);
        }

        public void DrawBox(int x, int y, int w, int h, int color)
        {
            var destination = new Rectangle(x, y, w, h);
            _batch.Draw(_blankTexture, destination, destination, ColorFromtInt32(color));
        }

        public void DrawText(string text, int x, int y, string font, int color)
        {
            if (!_fonts.ContainsKey(font))
                return;

            var f = _fonts[font];
            _batch.DrawString(f, text, new Vector2(x, y), ColorFromtInt32(color));
        }

        public void DrawTexture(string texture, int x, int y, int w, int h, global::MonoGame.Squid.Structs.Rectangle rect, int color)
        {
            if (!_textures.TryGetValue(texture, out var t))
                return;

            var destination = new Rectangle(x, y, w, h);
            var source = new Rectangle();

            source.X = rect.Left;
            source.Y = rect.Top;
            source.Width = rect.Width;
            source.Height = rect.Height;

            _batch.Draw(t, destination, source, ColorFromtInt32(color));
        }

        public void Scissor(int x, int y, int w, int h)
        {
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            _graphicsDevice.ScissorRectangle = new Rectangle(x, y, w, h);
        }

        public void StartBatch()
        {
            _batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, _sampler, null, _rasterizer);
        }

        public void EndBatch(bool final)
        {
            _batch.End();
        }

        Texture2D GetTexture(string texture)
        {
            if (_textures.TryGetValue(texture, out var t))
                return t;
            t = _textures[texture] = _contentManager.Load<Texture2D>(Path.ChangeExtension(texture, null));
            return t;
        }
    }
}
