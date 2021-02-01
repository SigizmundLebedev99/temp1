using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class MoveSystem : EntityProcessingSystem
    {
        Mapper<IMovement> _moveMapper;
        Mapper<Dot> _boxMapper;

        public MoveSystem() : base(Aspect.All(typeof(AllowedToAct), typeof(IMovement), typeof(Dot)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _moveMapper = mapperService.Get<IMovement>();
            _boxMapper = mapperService.Get<Dot>();
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