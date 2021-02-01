using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class DamageSystem : EntityProcessingSystem
    {
        Mapper<Health> _stateMapper;
        Mapper<Attack> _attackMapper;

        public DamageSystem() : base(Aspect.All(typeof(Attack)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _stateMapper = mapperService.Get<Health>();
            _attackMapper = mapperService.Get<Attack>();
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