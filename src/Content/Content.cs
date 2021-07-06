using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using temp1.Models;

namespace temp1
{
    static class Content
    {
        static ContentManager _content;
        
        public static void Initialize(ContentManager manager)
        {
            _content = manager;
        }

        public static T Load<T>(string asset)
        {
            return _content.Load<T>(asset);
        }

        public static T Load<T>(string asset, JsonContentLoader loader)
        {
            return _content.Load<T>(asset, loader);
        }

        public static Sprite GetSprite(string textureName, Region region = null)
        {
            var texture = _content.Load<Texture2D>(textureName);
            return region == null ? new Sprite(texture) : new Sprite(new TextureRegion2D(texture, region));
        }

        public static AnimatedSprite GetAnimatedSprite(string name)
        {
            var ss = _content.Load<SpriteSheet>(name, new JsonContentLoader());
            var sprite = new AnimatedSprite(ss);
            sprite.Play("idle");
            return sprite;
        }

        public static Sprite GetSprite(RenderObjectInfo spriteInfo)
        {
            Sprite sprite;

            if (spriteInfo == null)
                return null;

            if (spriteInfo.Path.EndsWith(".sf"))
                sprite = GetAnimatedSprite(spriteInfo.Path);
            else
                sprite = GetSprite(spriteInfo.Path, spriteInfo.Region);

            sprite.Origin = spriteInfo.Origin != null ? spriteInfo.Origin.ToVector2() : Vector2.Zero;

            return sprite;
        }
    }
}