using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class DirectionSystem : EntityProcessingSystem
    {
        Mapper<MapObject> _boxMapper;
        Mapper<Direction> _directionMapper;

        public DirectionSystem() : base(Aspect.All(typeof(Direction)))
        {}

        public override void Initialize(IComponentMapperService mapperService)
        {
            _directionMapper = mapperService.Get<Direction>();
            _boxMapper = mapperService.Get<MapObject>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var dir = _directionMapper.Get(entityId);
            var box = _boxMapper.Get(entityId);
            dir.PreviousPosition = dir.CurrentPosition;
            dir.CurrentPosition = box.Position;
        }
    }
}