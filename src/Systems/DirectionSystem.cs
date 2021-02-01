using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class DirectionSystem : EntityProcessingSystem
    {
        ComponentMapper<Dot> _boxMapper;
        ComponentMapper<Direction> _directionMapper;

        public DirectionSystem() : base(Aspect.All(typeof(AllowedToAct), typeof(Dot), typeof(Direction)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _directionMapper = mapperService.GetMapper<Direction>();
            _boxMapper = mapperService.GetMapper<Dot>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var dir = _directionMapper.Get(entityId);
            var box = _boxMapper.Get(entityId);

            if(box.Position == dir.CurrentPosition){
                dir.Changed = false;
                return;
            }
            dir.PreviousPosition = dir.CurrentPosition;
            dir.CurrentPosition = box.Position;
            dir.Changed = true;
        }
    }
}