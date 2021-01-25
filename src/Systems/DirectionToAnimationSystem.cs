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
        ComponentMapper<AnimatedSprite> _spriteMapper;
        ComponentMapper<Direction> _directionMapper;

        public DirectionToAnimationSystem() : base(Aspect.All(typeof(AnimatedSprite), typeof(Direction)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _directionMapper = mapperService.GetMapper<Direction>();
            _spriteMapper = mapperService.GetMapper<AnimatedSprite>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var dir = _directionMapper.Get(entityId);
            var animation = _spriteMapper.Get(entityId);
            var angle = dir.Angle;
            if(dir.Changed){
                if(angle >= Math.PI * 0.25 && angle < 0.75 * Math.PI)
                    animation.Play("walkEast");
                else if(Math.Abs(angle) >= Math.PI * 0.75 && Math.Abs(angle) < 1.25 * Math.PI)
                    animation.Play("walkNorth");
                else if(angle <= Math.PI * -0.25 && angle > -0.75 * Math.PI)
                    animation.Play("walkWest");
                else
                    animation.Play("walkSouth");
            }
            else
                animation.Play("idle");
        }
    }
}