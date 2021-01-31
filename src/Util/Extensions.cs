using EpPathFinding.cs;
using Microsoft.Xna.Framework;

static class Extensions
{
    public static bool Contains(this BaseGrid grid, Point point){
        return point.X < grid.width && point.Y < grid.height && point.X >= 0 && point.Y >= 0;
    }
}