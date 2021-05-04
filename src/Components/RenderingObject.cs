using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;

namespace temp1.Components
{
    class RenderingObject
    {
        public RenderingObject(Sprite sprite)
        {
            Sprite = sprite;
            Depth = sprite.Depth;
        }

        public bool Visible = true;
        public Sprite Sprite;

        public Rectangle Bounds => Sprite.TextureRegion.Bounds;
        public Texture2D Texture => Sprite.TextureRegion.Texture;
        public Vector2 Origin => Sprite.Origin;
        public float Depth;

        public void Play(string animationName, Action OnCompleted = null)
        {
            if(!(Sprite is AnimatedSprite animation))
                return;
            animation.Play(animationName, OnCompleted);
        }
    }
}