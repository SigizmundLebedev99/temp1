using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using temp1.Components;

namespace temp1.Systems
{
    class TransparensySystem : EntityProcessingSystem
    {
        Mapper<RenderingObject> _spriteMapper;
        Mapper<MapObject> _moMapper;

        Dictionary<int, (int, int, Color[])> _cache = new Dictionary<int, (int, int, Color[])>();

        public TransparensySystem() : base(Aspect.All(typeof(RenderingObject), typeof(Hull)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _moMapper = mapperService.Get<MapObject>();
            _spriteMapper = mapperService.Get<RenderingObject>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var playerPos = _moMapper.Get(GameContext.PlayerId).Position;
            var hullPos = _moMapper.Get(entityId).Position;
            if (playerPos.Y > hullPos.Y)
                return;
            
            var playerSprite = _spriteMapper.Get(GameContext.PlayerId);
            var hullSprite = _spriteMapper.Get(entityId);
            var playerRect = playerSprite.Sprite.GetBoundingRectangle(playerPos, 0, Vector2.One);
            
            hullSprite.Visible = !Overlaps(entityId, hullSprite, hullPos, playerRect);
        }

        bool Overlaps(int id, RenderingObject sprite, Vector2 hullPos, RectangleF player)
        {
            var playerRect = player.ToRectangle();
            var hullRect = sprite.Sprite.GetBoundingRectangle(hullPos, 0, Vector2.One).ToRectangle();
            if (!hullRect.Intersects(playerRect) && !hullRect.Contains(playerRect))
                return false;

            if (!_cache.ContainsKey(id))
            {
                var texture = sprite.Texture;
                var data = new Color[texture.Width * texture.Height];
                texture.GetData(data);
                _cache.Add(id, (texture.Width, texture.Height, data));
            }
            var (textureWidth, textureHeight, pixels) = _cache[id];
            var center = new Vector2(player.Center.X, player.Center.Y);

            bool ContainsPoint(Vector2 v)
            {
                var point = (v - new Vector2(hullRect.Left, hullRect.Top)).ToPoint();
                var i = point.X + point.Y * textureWidth;
                if (point.X > textureWidth || point.Y > textureHeight || i < 0 || i >= pixels.Length)
                    return false;
                var a = pixels[i].A;
                return a > 150;
            }

            if (ContainsPoint(player.BottomLeft) || ContainsPoint(player.BottomRight))
                return true;

            return false;
        }
    }
}