using Microsoft.Xna.Framework;

namespace temp1.GridSystem
{
    class Grid
    {
        public int Width => _width;
        public int Height => _height;

        int _width;
        int _height;
        float _cellSize;
        bool[,] gridArray;

        public Grid(int width, int height, float cellSize)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            gridArray = new bool[width, height];
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    gridArray[i, j] = true;
                }
            }
        }

        public bool Contains(int x, int y)
        {
            return x < _width && y < _height && x >= 0 && y >= 0;
        }

        public Vector2 GetWorldPosition(int x, int y)
        {
            return new Vector2(x, y) * _cellSize + new Vector2(_cellSize / 2);
        }

        public Point GetXY(Vector2 worldPos)
        {
            return (worldPos / 32).ToPoint();
        }

        public void SetValue(int x, int y, bool value)
        {
            if (!Contains(x, y))
                return;
            gridArray[x, y] = value;
        }

        public void SetValue(Vector2 worldPos, bool value)
        {
            var point = GetXY(worldPos);
            if(!Contains(point.X, point.Y))
                return;
            SetValue(point.X, point.Y, value);
        }

        public bool ValueAt(int x, int y)
        {
            if (!Contains(x, y))
                return false;
            return gridArray[x, y];
        }
    }
}