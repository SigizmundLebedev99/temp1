using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Input;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using temp1.Data;
using temp1.Models;
using temp1.PathFinding;

static class Extensions
{
    public static bool Contains(this StaticGrid grid, Point point)
    {
        return point.X < grid.width && point.Y < grid.height && point.X >= 0 && point.Y >= 0;
    }

    public static Point MapPosition(this MouseStateExtended state, OrthographicCamera camera)
    {
        var worldPos = camera.ScreenToWorld(state.Position.X, state.Position.Y);
        return (worldPos / 32).ToPoint();
    }

    public static Sprite GetSprite(this ContentManager content, string textureName, Region region = null)
    {
        var texture = content.Load<Texture2D>(textureName);
        return region == null ? new Sprite(texture) : new Sprite(new TextureRegion2D(texture, region));
    }

    public static AnimatedSprite GetAnimatedSprite(this ContentManager content, string name)
    {
        var ss = content.Load<SpriteSheet>(name, new JsonContentLoader());
        var sprite = new AnimatedSprite(ss);
        sprite.Play("idle");
        return sprite;
    }

    public static Sprite GetSprite(this ContentManager content, RenderObjectInfo spriteInfo)
    {
        Sprite sprite;

        if (spriteInfo == null)
            return null;

        if (spriteInfo.Path.EndsWith(".sf"))
            sprite = content.GetAnimatedSprite(spriteInfo.Path);
        else
            sprite = content.GetSprite(spriteInfo.Path, spriteInfo.Region);

        sprite.Origin = spriteInfo.Origin != null ? spriteInfo.Origin.ToVector2() : Vector2.Zero;

        return sprite;
    }
}