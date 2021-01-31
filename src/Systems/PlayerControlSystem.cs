using System.Linq;
using EpPathFinding.cs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Input;
using MonoGame.Extended.Sprites;
using temp1.Components;

namespace temp1.Systems
{
    class PlayerControlSystem : EntityProcessingSystem
    {
        ComponentMapper<IMovement> _movementMapper;
        ComponentMapper<Dot> _boxMapper;
        OrthographicCamera _camera;
        JumpPointParam jpParam;
        GameContext _context;

        public PlayerControlSystem(OrthographicCamera camera, GameContext context) : base(Aspect.All(typeof(Player), typeof(AnimatedSprite)))
        {
            _camera = camera;
            _context = context;
            jpParam = new JumpPointParam(_context.Grid, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Never);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _movementMapper = mapperService.GetMapper<IMovement>();
            _boxMapper = mapperService.GetMapper<Dot>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var state = MouseExtended.GetState();
            if (!state.WasButtonJustUp(MouseButton.Left))
                return;
            var pointOnMap = _camera.ScreenToWorld(state.X, state.Y);
            var to = (pointOnMap / 32).ToPoint();
            if (!_context.Grid.Contains(to) || !_context.Grid.IsWalkableAt(to.X, to.Y))
                return;

            var from = _boxMapper.Get(entityId).MapPosition;
            jpParam.Reset(new GridPos(from.X, from.Y), new GridPos(to.X, to.Y));
            var result = JumpPointFinder.FindPath(jpParam);
            if (result.Count < 2)
                return;

            var movement = new PolylineMovement(
                    result.Select(e => new Vector2(e.x * 32 + 16, e.y * 32 + 16)).ToArray(),
                3f);

            CreateMarker(to);

            _movementMapper.Put(entityId, movement);
        }

        void CreateMarker(Point to)
        {
            var marker = CreateEntity();
            marker.Attach<IExpired>(new TimeExpired(1.2f));
            marker.Attach(new Dot
            {
                Position = to.ToVector2() * 32 + new Vector2(16)
            });
            var sprite = _context.GetAnimatedSprite("animations/circles.sf");
            sprite.Play("idle");
            marker.Attach(sprite);
        }
    }
}