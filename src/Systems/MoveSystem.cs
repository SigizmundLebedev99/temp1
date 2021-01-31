using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class MoveSystem : EntityProcessingSystem
    {
        ComponentMapper<IMovement> _moveMapper;
        ComponentMapper<Dot> _boxMapper;

        public MoveSystem() : base(Aspect.All(typeof(AllowedToAct), typeof(IMovement), typeof(Dot)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _moveMapper = mapperService.GetMapper<IMovement>();
            _boxMapper = mapperService.GetMapper<Dot>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var move = _moveMapper.Get(entityId);
            var box = _boxMapper.Get(entityId);
            box.Position = move.Move();
            if(move.IsCompleted){
                _moveMapper.Delete(entityId);
            }
        }
    }
}