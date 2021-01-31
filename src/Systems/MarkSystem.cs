using EpPathFinding.cs;
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
    class MarkSystem : EntitySystem, IDrawSystem, IUpdateSystem
    {
        private Vector2 position;
        private bool inMap = false;
        AnimatedSprite mark;

        private ComponentMapper<Dot> _boxMapper;
        private SpriteBatch _spriteBatch;
        private GameContext _context;
        OrthographicCamera _camera;
        public MarkSystem(SpriteBatch sb, GameContext context, OrthographicCamera camera) : base(Aspect.One(typeof(Dot)))
        {
            _context = context;
            _spriteBatch = sb;
            _camera = camera;
            mark = _context.GetAnimatedSprite("animations/mark.sf");
            mark.Play("idle");
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _boxMapper = mapperService.GetMapper<Dot>();
        }

        public void Update(GameTime gameTime)
        {
            var state = MouseExtended.GetState();
            var point = (_camera.ScreenToWorld(state.Position.X, state.Position.Y) / 32).ToPoint();
            if(!_context.Grid.Contains(point) || !_context.Grid.IsWalkableAt(point.X, point.Y)){
                inMap = false;
                return;
            }
            inMap = true;
            position = point.ToVector2() * 32 + new Vector2(16);
            mark.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {   
            if(!inMap)
                return;
            _spriteBatch.Draw(mark, position);
        }
    }
}