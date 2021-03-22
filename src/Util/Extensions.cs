using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.Extended.TextureAtlases;
using temp1.PathFinding;

static class Extensions
{
    public static bool Contains(this StaticGrid grid, Point point){
        return point.X < grid.width && point.Y < grid.height && point.X >= 0 && point.Y >= 0;
    }

    public static Point MapPosition(this MouseStateExtended state, OrthographicCamera camera){
        var worldPos = camera.ScreenToWorld(state.Position.X, state.Position.Y);
        return (worldPos / 32).ToPoint();
    }
}