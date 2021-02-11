using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Input;
using MonoGame.Extended.Sprites;
using temp1.Components;
using temp1.Data;

namespace temp1.Systems
{
    class CursorSystem : EntitySystem, IDrawSystem, IUpdateSystem
    {
        Vector2 position;
        float scale;
        bool shouldShow = false;
        AnimatedSprite mark;

        Mapper<MapObject> _dotMapper;
        Mapper<Sprite> _spriteMapper;
        Mapper<AnimatedSprite> _aSpriteMapper;
        Mapper<PossibleMoves> _possibleMovesMap;

        SpriteBatch _spriteBatch;
        GameContext _context;
        OrthographicCamera _camera;

        public CursorSystem(SpriteBatch sb, GameContext context) : base(Aspect.All(typeof(MapObject)).One(typeof(Sprite), typeof(AnimatedSprite)))
        {
            _context = context;
            _spriteBatch = sb;
            _camera = context.Camera;
            mark = _context.GetAnimatedSprite("images/mark.sf");
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _dotMapper = mapperService.Get<MapObject>();
            _spriteMapper = mapperService.Get<Sprite>();
            _aSpriteMapper = mapperService.Get<AnimatedSprite>();
            _possibleMovesMap = mapperService.Get<PossibleMoves>();
        }

        public void Update(GameTime gameTime)
        {
            if (_context.HudState != HudState.Default || _context.Hud.IsMouseOnHud)
            {
                shouldShow = false;
                return;
            }
            var state = MouseExtended.GetState();
            var worldPos = _camera.ScreenToWorld(state.Position.X, state.Position.Y);
            var point = (worldPos / 32).ToPoint();
            position = point.ToVector2() * 32 + new Vector2(16);
            _context.PointedId = -1;
            if (_context.GameState == GameState.Peace)
            {
                if (!_context.MovementGrid.Contains(point))
                {
                    shouldShow = false;
                    return;
                }

                if (!HandlePoint(worldPos))
                {
                    if (_context.MovementGrid.IsWalkableAt(point.X, point.Y))
                        mark.Play("idle");
                    else
                        mark.Play("no");
                }
            }
            else
            {
                var possibleMoves = _possibleMovesMap.Get(_context.PlayerId);
                if (possibleMoves.Contains(point))
                {
                    if (!HandlePoint(worldPos))
                        mark.Play("idle");
                }
                else
                {
                    mark.Play("no");
                }
            }
            shouldShow = true;
            mark.Update(gameTime.ElapsedGameTime.Milliseconds);
            scale = (float)(Math.Sin(gameTime.TotalGameTime.Milliseconds / 64) * 0.1 + 0.9);
        }

        private bool HandlePoint(Vector2 pos)
        {
            for (var i = 0; i < ActiveEntities.Count; i++)
            {
                var sprite = _spriteMapper.Get(ActiveEntities[i]) ?? _aSpriteMapper.Get(ActiveEntities[i]);
                var dot = _dotMapper.Get(ActiveEntities[i]);
                var id = ActiveEntities[i];
                var bounds = new Rectangle(0, 0, sprite.TextureRegion.Width, sprite.TextureRegion.Height);

                if (bounds.Contains(pos - dot.Position + sprite.Origin))
                {
                    if (GetCursor(dot, out var animation))
                        mark.Play(animation);
                    else
                        continue;
                    position = pos;
                    _context.PointedId = id;
                    return true;
                }
            }
            return false;
        }

        private bool GetCursor(MapObject obj, out string animation)
        {
            animation = obj.Type switch
            {
                GameObjectType.Storage => "hand",
                GameObjectType.Enemy => "sword",
                GameObjectType.Item => "hand",
                _ => null
            };
            return animation != null;
        }

        public void Draw(GameTime gameTime)
        {
            if (!shouldShow)
                return;
            _spriteBatch.Draw(
                mark.TextureRegion.Texture,
                position,
                mark.TextureRegion.Bounds,
                Color.White,
                0,
                new Vector2(16),
                scale, SpriteEffects.None, 0f);
        }
    }
}