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
        public PlayerControlSystem(OrthographicCamera camera) : base(Aspect.All(typeof(Player), typeof(AnimatedSprite)))
        {
            _camera = camera;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _movementMapper = mapperService.GetMapper<IMovement>();
            _boxMapper = mapperService.GetMapper<Box>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var state = Mouse.GetState();
            if(state.LeftButton == ButtonState.Pressed){
                var to = _camera.ScreenToWorld(state.X, state.Y);
                _movementMapper.Put(entityId,
                    new PolylineMovement(
                        new []{_boxMapper.Get(entityId).Position, to}, 
                    4f));
            }
        }
    }
}