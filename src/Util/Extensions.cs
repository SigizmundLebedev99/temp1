using EpPathFinding.cs;
using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;
using Myra.Graphics2D.TextureAtlases;

static class Extensions
{
    public static bool Contains(this BaseGrid grid, Point point){
        return point.X < grid.width && point.Y < grid.height && point.X >= 0 && point.Y >= 0;
    }

    public static TextureRegion ToMyra(this TextureRegion2D region){
        return new TextureRegion(region.Texture, region.Bounds);
    }
}