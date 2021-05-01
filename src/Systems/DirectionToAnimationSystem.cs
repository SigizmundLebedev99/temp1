using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using temp1.Components;

namespace temp1.Systems
{
    class DirectionToAnimationSystem : EntityProcessingSystem
    {
        Mapper<RenderingObject> _spriteMapper;
        Mapper<Direction> _directionMapper;
        Mapper<BaseAction> _actionMapper;

        public DirectionToAnimationSystem() : base(Aspect.All(typeof(RenderingObject), typeof(Direction)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _directionMapper = mapperService.Get<Direction>();
            _spriteMapper = mapperService.Get<RenderingObject>();
            _actionMapper = mapperService.Get<BaseAction>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var sprite = _spriteMapper.Get(entityId);

            var dir = _directionMapper.Get(entityId);
            var action = _actionMapper.Get(entityId);
            var angle = dir.angle;
            string direction;

            if(angle >= Math.PI * 0.25 && angle < 0.75 * Math.PI)
                direction = "East";
            else if(Math.Abs(angle) >= Math.PI * 0.75 && Math.Abs(angle) < 1.25 * Math.PI)
                direction = "North";
            else if(angle <= Math.PI * -0.25 && angle > -0.75 * Math.PI)
                direction = "West";
            else
                direction = "South";

            if(action == null || !(action is WalkAction)){
                _directionMapper.Delete(entityId);
                sprite.Play("idle" + direction);
                return;
            }

            sprite.Play("walk" + direction);
        }
    }
}