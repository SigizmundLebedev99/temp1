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
        private TextAlign vertical = TextAlign.Center;
        public TextAlign Vertical { get => vertical; set { vertical = value; ComputeTextLayout(); } }

        private TextAlign horizontal = TextAlign.Center;
        public TextAlign Horizontal { get => horizontal; set { horizontal = value; ComputeTextLayout(); } }

        private BitmapFont font;
        public BitmapFont Font { get => font; set { font = value; ComputeTextLayout(); } }

        private Control control;
        private List<TextSpan> paragrath = new List<TextSpan>();
        private float paragrathHeight;

        private string text = "";
        public string Value { get => text; set { text = value ?? ""; ComputeTextLayout(); } }

        public Vector2 Size => new Vector2(control.Size.X, paragrathHeight);

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
            var pOffset = new Vector2(0, ComputeOffset(paragrathHeight, control.Size.Y, Vertical));
            for (var i = 0; i < paragrath.Count; i++)
            {
                var span = paragrath[i];
                batch.DrawString(font, span.Text, position + span.Offset + pOffset, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, depth);
            }
        }

        public void ComputeTextLayout()
        {
            var lines = text.Split('\n');
            paragrath.Clear();

            float lineY = 0;
            foreach (var line in lines)
            {
                var words = line.Split(' ').AsSpan();
                if (words.Length == 0)
                    continue;
                while (words.Length > 0)
                {
                    var span = CutOffLine(words, lineY, out int wordsConsumed);
                    paragrath.Add(span);
                    lineY += span.Height;
                    words = words.Slice(wordsConsumed);
                }
            }
            paragrathHeight = lineY;
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
                Offset = new Vector2(ComputeOffset(lineWidth, control.Size.X, Horizontal), lineY),
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
        public Vector2 Offset;
    }
}