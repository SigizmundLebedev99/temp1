using System.Linq;
using EpPathFinding.cs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Input;
using temp1.Components;

namespace temp1.Systems
{
    class PlayerControlSystem : EntityUpdateSystem
    {
        Mapper<IMovement> _movementMapper;
        Mapper<Dot> _boxMapper;
        OrthographicCamera _camera;
        JumpPointParam jpParam;
        GameContext _context;

        public PlayerControlSystem(OrthographicCamera camera, GameContext context) : base(Aspect.All(typeof(Player)))
        {
            _camera = camera;
            _context = context;
            jpParam = new JumpPointParam(_context.Grid, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Never);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _movementMapper = mapperService.Get<IMovement>();
            _boxMapper = mapperService.Get<Dot>();
        }

        public override void Update(GameTime gameTime)
        {
            var state = MouseExtended.GetState();
            if (state.LeftButton != ButtonState.Pressed)
                return;
            if(_context.PointedId == -1){
                CommitMovement(state);
                return;
            }
        }

        void CommitMovement(MouseStateExtended state){
            var pointOnMap = _camera.ScreenToWorld(state.X, state.Y);
            var to = (pointOnMap / 32).ToPoint();
            if (!_context.Grid.Contains(to) || !_context.Grid.IsWalkableAt(to.X, to.Y))
                return;
            var from = _boxMapper.Get(_context.PlayerId).MapPosition;
            jpParam.Reset(new GridPos(from.X, from.Y), new GridPos(to.X, to.Y));
            var result = JumpPointFinder.FindPath(jpParam);
            if (result.Count < 2)
                return;
            var movement = new PolylineMovement(
                    result.Select(e => new Vector2(e.x * 32 + 16, e.y * 32 + 16)).ToArray(),
                3f);

            CreateMarker(to);
            _movementMapper.Put(_context.PlayerId, movement);
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