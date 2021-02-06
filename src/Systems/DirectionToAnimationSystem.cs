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
        Mapper<AnimatedSprite> _spriteMapper;
        Mapper<Direction> _directionMapper;
        Mapper<IMovement> _moveMapper;

        public DirectionToAnimationSystem() : base(Aspect.All(typeof(AnimatedSprite), typeof(Direction)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _directionMapper = mapperService.Get<Direction>();
            _spriteMapper = mapperService.Get<AnimatedSprite>();
            _moveMapper = mapperService.Get<IMovement>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var move = _moveMapper.Get(entityId);
            var animation = _spriteMapper.Get(entityId);
            var dir = _directionMapper.Get(entityId);
            var angle = dir.Angle;
            string direction;

            if(angle >= Math.PI * 0.25 && angle < 0.75 * Math.PI)
                direction = "East";
            else if(Math.Abs(angle) >= Math.PI * 0.75 && Math.Abs(angle) < 1.25 * Math.PI)
                direction = "North";
            else if(angle <= Math.PI * -0.25 && angle > -0.75 * Math.PI)
                direction = "West";
            else
                direction = "South";

            if(move == null || move.IsCompleted){
                _directionMapper.Delete(entityId);
                animation.Play("idle" + direction);
                return;
            }

            animation.Play("walk" + direction);
        }
    }
}