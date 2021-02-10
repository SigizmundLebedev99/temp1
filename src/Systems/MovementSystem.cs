using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class MovementSystem : EntityProcessingSystem
    {
        Mapper<IMovement> _moveMapper;
        Mapper<PossibleMoves> _possibleMoveMap;
        Mapper<MapObject> _boxMapper;

        public MovementSystem() : base(Aspect.All(typeof(IMovement), typeof(MapObject)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _moveMapper = mapperService.Get<IMovement>();
            _boxMapper = mapperService.Get<MapObject>();
            _possibleMoveMap= mapperService.Get<PossibleMoves>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var move = _moveMapper.Get(entityId);
            var box = _boxMapper.Get(entityId);
            box.Position = move.Move();
            if(move.IsCompleted){
                move.OnComplete?.Invoke();
                _moveMapper.Delete(entityId);
                _possibleMoveMap.Delete(entityId);
            }
        }
    }
}