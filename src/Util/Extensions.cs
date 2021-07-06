using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Input;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using temp1;
using temp1.Models;
using temp1.PathFinding;

static class Extensions
{
    public static bool Contains(this StaticGrid grid, Point point)
    {
        return point.X < grid.width && point.Y < grid.height && point.X >= 0 && point.Y >= 0;
    }

    public static Point GridCell(this Point position)
    {
        return (position.ToVector2() / 32).ToPoint();
    }

    public static Vector2 MapPosition(this Point position)
    {
        return position.ToVector2() * 32 + new Vector2(16);
    }

    public static Point GridCell(this Vector2 position)
    {
        return (position / 32).ToPoint();
    }
}