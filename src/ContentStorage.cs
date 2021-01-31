using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;

static class ContentStorage
{
    public static AnimatedSprite Circles => GetMarker();
    public static AnimatedSprite Enemy => GetActorSprite(enemySprites);
    public static AnimatedSprite Player => GetActorSprite(playerSprites);
    public static AnimatedSprite Portal => GetActorSprite(portalSprites);

    private static SpriteSheet circles;
    private static SpriteSheet enemySprites;
    private static SpriteSheet playerSprites;
    private static SpriteSheet portalSprites;

    public static void Load(ContentManager content)
    {
        var cl = new JsonContentLoader();
        playerSprites = content.Load<SpriteSheet>("player.sf", cl);
        circles = content.Load<SpriteSheet>("circles.sf", cl);
        enemySprites = content.Load<SpriteSheet>("enemy.sf", cl);
        portalSprites = content.Load<SpriteSheet>("portal.sf", cl);
    }

    
    private static AnimatedSprite GetActorSprite(SpriteSheet sprites)
    {
        var sprite = new AnimatedSprite(sprites);
        sprite.Depth = 0;
        sprite.Origin = new Vector2(sprite.TextureRegion.Width / 2, sprite.TextureRegion.Height * 0.9f);
        sprite.Play("idle");
        return sprite;
    }

    private static AnimatedSprite GetMarker()
    {
        var sprite = new AnimatedSprite(circles);
        sprite.Depth = 0.5f;
        return sprite;
    }
}