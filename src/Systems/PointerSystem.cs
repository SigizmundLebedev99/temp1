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
        AnimatedSprite mark;

        private Mapper<MapObject> _dotMapper;
        private Mapper<Sprite> _spriteMapper;
        private Mapper<AnimatedSprite> _aSpriteMapper;

        private SpriteBatch _spriteBatch;
        private GameContext _context;
        OrthographicCamera _camera;

        public PointerSystem(SpriteBatch sb, GameContext context) : base(Aspect.All(typeof(MapObject)).One(typeof(Sprite), typeof(AnimatedSprite)))
        {
            _context = context;
            _spriteBatch = sb;
            _camera = context.Camera;
            mark = _context.GetAnimatedSprite("animations/mark.sf");
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _dotMapper = mapperService.Get<MapObject>();
            _spriteMapper = mapperService.Get<Sprite>();
            _aSpriteMapper = mapperService.Get<AnimatedSprite>();
        }

        public void Update(GameTime gameTime)
        {
            if( _context.GameState != GameState.Default)
                return;
            var state = MouseExtended.GetState();
            var worldPos = _camera.ScreenToWorld(state.Position.X, state.Position.Y);
            var point = (worldPos / 32).ToPoint();
            if (!_context.CollisionGrid.Contains(point))
            {
                inMap = false;
                return;
            }

            position = point.ToVector2() * 32 + new Vector2(16);
            
            _context.PointedId = -1;

            if (!_context.CollisionGrid.IsWalkableAt(point.X, point.Y))
                mark.Play("no");
            else if (!HandlePoint(worldPos))
                mark.Play("idle");
            inMap = true;
            mark.Update(gameTime);
        }

        private bool HandlePoint(Vector2 pos)
        {
            for (var i = 0; i < ActiveEntities.Count; i++)
            {
                var sprite = _spriteMapper.Get(ActiveEntities[i]) ?? _aSpriteMapper.Get(ActiveEntities[i]);
                var dot = _dotMapper.Get(ActiveEntities[i]);
                var id =  ActiveEntities[i];
                var bounds = new Rectangle(0, 0, sprite.TextureRegion.Width, sprite.TextureRegion.Height);

                if (bounds.Contains(pos - dot.Position + sprite.Origin))
                {
                    if (dot.Flag == MapObjectFlag.Storage)
                        mark.Play("hand");
                    else if (dot.Flag == MapObjectFlag.Enemy)
                        mark.Play("sword");
                    else
                        continue;
                    position = pos;
                    _context.PointedId = id;
                    return true;
                }
            }
            return false;
        }

        public void Draw(GameTime gameTime)
        {
            if (!inMap || _context.GameState != GameState.Default)
                return;
            _spriteBatch.Draw(mark, position);
        }
    }
}