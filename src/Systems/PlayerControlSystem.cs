using System.Linq;
using EpPathFinding.cs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using temp1.Components;

namespace temp1.Systems
{
    class PlayerControlSystem : EntityProcessingSystem
    {
        ComponentMapper<IMovement> _movementMapper;
        ComponentMapper<Box> _boxMapper;
        OrthographicCamera _camera;
        JumpPointParam jpParam;
        BaseGrid _grid;

        public PlayerControlSystem(OrthographicCamera camera, BaseGrid searchGrid) : base(Aspect.All(typeof(Player), typeof(AnimatedSprite)))
        {
            _camera = camera;
            _grid = searchGrid;
            jpParam = new JumpPointParam(searchGrid, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Never);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _movementMapper = mapperService.GetMapper<IMovement>();
            _boxMapper = mapperService.GetMapper<Box>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var state = Mouse.GetState();
            if (state.LeftButton != ButtonState.Pressed)
                return;
            var pointOnMap = _camera.ScreenToWorld(state.X, state.Y);
            var to = (pointOnMap / 32).ToPoint();
            if(!_grid.IsWalkableAt(to.X, to.Y))
                return;

            var from = _boxMapper.Get(entityId).MapPosition;
            jpParam.Reset(new GridPos(from.X, from.Y), new GridPos(to.X, to.Y));
            var result = JumpPointFinder.FindPath(jpParam);
            if (result.Count < 2)
                return;
            
            var movement = new PolylineMovement(
                    result.Select(e => new Vector2(e.x * 32 + 16, e.y * 32 + 16)).ToArray(),
                3f);
            
            var marker = CreateEntity();
            marker.Attach<IExpired>(new TimeExpired(1.2f));
            marker.Attach(new Box
            {
                Position = to.ToVector2() * 32 + new Vector2(16)
            });
            var sprite = ContentStorage.Circles;
            sprite.Play("idle");
            marker.Attach(sprite);
            
            _movementMapper.Put(entityId,movement);
        }
    }
}