namespace temp1.UI
{
    public struct Margin
    {
        public int Top;
        public int Right;
        public int Bottom;
        public int Left;

        public int Vertical { get => Top + Bottom; }
        public int Horizontal { get => Left + Right; }

        public Margin(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public Margin(int ver, int hor)
        {
            Top = Bottom = ver;
            Right = Left = hor;
        }
    }
}