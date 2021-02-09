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
        Mapper<AnimatedSprite> _aSpriteMapper;
        Mapper<Sprite> _spriteMapper;
        Mapper<MapObject> _dotMapper;
        Mapper<Hull> _hullMapper;
        GameContext _context;

        Dictionary<int, (int, int, Color[])> _cache = new Dictionary<int, (int, int, Color[])>();

        public TransparensySystem(GameContext context) : base(Aspect.All(typeof(Sprite), typeof(Hull)))
        {
            _context = context;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _dotMapper = mapperService.Get<MapObject>();
            _spriteMapper = mapperService.Get<Sprite>();
            _aSpriteMapper = mapperService.Get<AnimatedSprite>();
            _hullMapper = mapperService.Get<Hull>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var playerPos = _dotMapper.Get(_context.PlayerId).position;
            var hullPos = _dotMapper.Get(entityId).position;
            if (playerPos.Y > hullPos.Y)
                return;
            var playerRect = _aSpriteMapper.Get(_context.PlayerId).GetBoundingRectangle(playerPos, 0, Vector2.One);
            
            _hullMapper.Get(entityId).isPlayerIn = Overlaps(entityId, hullPos, playerRect);
        }

        bool Overlaps(int id, Vector2 hullPos, RectangleF player)
        {
            var sprite = _spriteMapper.Get(id);
            var playerRect = player.ToRectangle();
            var hullRect = sprite.GetBoundingRectangle(hullPos, 0, Vector2.One).ToRectangle();
            if (!hullRect.Intersects(playerRect) && !hullRect.Contains(playerRect))
                return false;

            if (!_cache.ContainsKey(id))
            {
                var texture = sprite.TextureRegion.Texture;
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
                return a > 200;
            }

            if (ContainsPoint(player.BottomLeft) || ContainsPoint(player.BottomRight))
                return true;

            return false;
        }
    }
}