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

        public PlayerControlSystem(OrthographicCamera camera, BaseGrid searchGrid) : base(Aspect.All(typeof(Player), typeof(AnimatedSprite)))
        {
            _camera = camera;
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
            if(state.LeftButton != ButtonState.Pressed)
                return;
            var to = (_camera.ScreenToWorld(state.X, state.Y) / 32).ToPoint();
            var from = (_boxMapper.Get(entityId).Position / 32).ToPoint();
            jpParam.Reset(new GridPos(from.X, from.Y),new GridPos(to.X, to.Y)); 
            var result = JumpPointFinder.FindPath(jpParam); 
            if(result.Count < 2)
                return;                                   
            _movementMapper.Put(entityId,
                new PolylineMovement(
                    result.Select(e => new Vector2(e.x * 32 + 16, e.y * 32 + 16)).ToArray(),
                4f));            
        }
    }
}