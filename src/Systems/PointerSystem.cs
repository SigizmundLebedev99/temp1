using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Input;
using MonoGame.Extended.Sprites;
using temp1.Components;

namespace temp1.Systems
{
    class PointerSystem : EntitySystem, IDrawSystem, IUpdateSystem
    {
        private Vector2 position;
        private bool inMap = false;
        private int pointed = -1;
        AnimatedSprite mark;

        private Mapper<Dot> _dotMapper;
        private Mapper<Storage> _storageMapper;
        private Mapper<Enemy> _enemyMapper;
        private Mapper<Pointed> _pointedMapper;
        private Mapper<Sprite> _spriteMapper;
        private Mapper<AnimatedSprite> _aSpriteMapper;

        private SpriteBatch _spriteBatch;
        private GameContext _context;
        OrthographicCamera _camera;
        public PointerSystem(SpriteBatch sb, GameContext context, OrthographicCamera camera) : base(Aspect.All(typeof(Dot)).One(typeof(Sprite), typeof(AnimatedSprite)))
        {
            _context = context;
            _spriteBatch = sb;
            _camera = camera;
            mark = _context.GetAnimatedSprite("animations/mark.sf");
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _storageMapper = mapperService.Get<Storage>();
            _enemyMapper = mapperService.Get<Enemy>();
            _dotMapper = mapperService.Get<Dot>();
            _pointedMapper = mapperService.Get<Pointed>();
            _spriteMapper = mapperService.Get<Sprite>();
            _aSpriteMapper = mapperService.Get<AnimatedSprite>();
        }

        public void Update(GameTime gameTime)
        {
            var state = MouseExtended.GetState();
            var worldPos = _camera.ScreenToWorld(state.Position.X, state.Position.Y);
            var point = (worldPos / 32).ToPoint();
            if (!_context.Grid.Contains(point))
            {
                inMap = false;
                TryClearPointed();
                return;
            }
            position = point.ToVector2() * 32 + new Vector2(16);
            if (!_context.Grid.IsWalkableAt(point.X, point.Y))
                mark.Play("no");
            else if (!GetTarget(worldPos, out int id))
                mark.Play("idle");

            inMap = true;
            mark.Update(gameTime);
        }

        private bool GetTarget(Vector2 pos, out int id)
        {
            for (var i = 0; i < ActiveEntities.Count; i++)
            {
                var sprite = _spriteMapper.Get(ActiveEntities[i]) ?? _aSpriteMapper.Get(ActiveEntities[i]);
                var dot = _dotMapper.Get(ActiveEntities[i]);

                var bounds = new Rectangle(0, 0, sprite.TextureRegion.Width, sprite.TextureRegion.Height);
                if (bounds.Contains(pos - dot.Position + sprite.Origin)
                    && HandlePoint(ActiveEntities[i], pos))
                {
                    id = ActiveEntities[i];
                    if (pointed == -1)
                        _pointedMapper.Put(id, new Pointed());
                    else if (id != pointed)
                    {
                        _pointedMapper.Delete(pointed);
                        _pointedMapper.Put(id, new Pointed());
                    }
                    pointed = id;
                    return true;
                }
            }
            TryClearPointed();
            id = -1;
            return false;
        }

        private bool HandlePoint(int id, Vector2 worldPos)
        {
            if (_storageMapper.Has(id))
                mark.Play("hand");

            else if (_enemyMapper.Has(id))
                mark.Play("sword");

            else
                return false;

            position = worldPos;
            return true;
        }

        private void TryClearPointed()
        {
            if (pointed != -1)
            {
                _pointedMapper.Delete(pointed);
                pointed = -1;
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (!inMap)
                return;
            _spriteBatch.Draw(mark, position);
        }
    }
}