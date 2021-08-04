using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using temp1.UI.Controls;

namespace temp1.UI.Text
{
    public class TextPiece
    {
        public TextAlign Vertical { get; set; } = TextAlign.Center;
        public TextAlign Horizontal { get; set; } = TextAlign.Center;

        private BitmapFont font;
        public BitmapFont Font { get => font; set { font = value; ComputeTextLayout(); } }

        private Control control;
        private List<TextSpan> paragrath = new List<TextSpan>();

        private string _text = "";
        public string Value { get => _text; set { _text = value ?? ""; ComputeTextLayout(); } }

        private Vector2 _size = new Vector2();
        public Vector2 Size => _size;

        public TextPiece(Control control, BitmapFont font)
        {
            if (font == null)
                throw new ArgumentNullException("font");
            this.control = control;
            this.font = font;
        }

        public void Draw(SpriteBatch batch, Vector2 position, float depth = 0)
        {
            Draw(batch, position, Vector2.One, 0, depth);
        }

        public void Draw(SpriteBatch batch, Vector2 position, Vector2 scale, float depth = 0)
        {
            Draw(batch, position, scale, 0, depth);
        }

        public void Draw(SpriteBatch batch, Vector2 position, Vector2 scale, float rotation, float depth = 0)
        {
            if (_text == String.Empty)
                return;
            var paragrathOffset = new Vector2(0, ComputeOffset(_size.Y, control.Size.Y, Vertical));
            for (var i = 0; i < paragrath.Count; i++)
            {
                var span = paragrath[i];
                var spanOffset = new Vector2(ComputeOffset(span.Width, control.Size.X, Horizontal), i * span.Height);
                batch.DrawString(font, span.Text, position + spanOffset + paragrathOffset, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, depth);
            }
        }

        public void ComputeTextLayout()
        {
            var lines = _text.Split('\n');
            paragrath.Clear();

            float totalHeight = 0;
            float maxWidth = 0;
            foreach (var line in lines)
            {
                var words = line.Split(' ').AsSpan();
                if (words.Length == 0)
                    continue;
                while (words.Length > 0)
                {
                    var span = CutOffLine(words, totalHeight, out int wordsConsumed);
                    paragrath.Add(span);
                    maxWidth = Math.Max(span.Width, maxWidth);
                    totalHeight += span.Height;
                    words = words.Slice(wordsConsumed);
                }
            }
            _size = new Vector2(maxWidth, totalHeight);
        }

        private TextSpan CutOffLine(Span<string> words, float lineY, out int wordsConsumed)
        {
            wordsConsumed = 0;
            float lineWidth = 0;
            float lineHeight = 0;
            bool firstWord = true;
            var line = new StringBuilder();
            var spaceWidth = font.MeasureString(" ").Width;
            foreach (var word in words)
            {
                var wordSize = font.MeasureString(word);
                if (lineWidth + wordSize.Width >= control.Size.X * 0.85f && !firstWord)
                    break;
                else
                {
                    firstWord = false;
                    wordsConsumed++;
                    if (line.Length > 0)
                    {
                        line.Append(' ');
                        lineWidth += spaceWidth;
                    }
                    lineHeight = Math.Max(lineHeight, wordSize.Height);
                    line.Append(word);
                    lineWidth += wordSize.Width;
                }
            }

            return new TextSpan
            {
                Width = lineWidth,
                Height = lineHeight,
                Text = line.ToString()
            };
        }

        private float ComputeOffset(float size, float total, TextAlign align)
        {
            return align switch
            {
                TextAlign.Start => 0,
                TextAlign.Center => total / 2 - size / 2,
                TextAlign.End => total - size,
                _ => 0
            };
        }
    }

    struct TextSpan
    {
        public string Text;
        public float Width;
        public float Height;
    }
}