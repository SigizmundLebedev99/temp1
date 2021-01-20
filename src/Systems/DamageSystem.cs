using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class DamageSystem : EntityProcessingSystem
    {
        ComponentMapper<Health> _stateMapper;
        ComponentMapper<Attack> _attackMapper;

        public DamageSystem() : base(Aspect.All(typeof(Attack)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _stateMapper = mapperService.GetMapper<Health>();
            _attackMapper = mapperService.GetMapper<Attack>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var attack = _attackMapper.Get(entityId);
            var state = _stateMapper.Get(attack.TargetEntityId);
            state.HP -= attack.Damage;
            if (state.HP <= 0)
                DestroyEntity(attack.TargetEntityId);
        }
    }
}