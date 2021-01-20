using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Linq;
using temp1.Components;

namespace temp1.Systems
{
    class ProtectionSystem : EntityProcessingSystem
    {
        ComponentMapper<Effects> _effectsMapper;
        ComponentMapper<Attack> _attackMapper;
        ComponentMapper<Stats> _statsMapper;

        public ProtectionSystem() : base(Aspect.All(typeof(Attack)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _effectsMapper = mapperService.GetMapper<Effects>();
            _attackMapper = mapperService.GetMapper<Attack>();
            _statsMapper = mapperService.GetMapper<Stats>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var attack = _attackMapper.Get(entityId);
            var stats = _statsMapper.Get(attack.TargetEntityId);
            var effects = _effectsMapper.Get(attack.TargetEntityId);
            var changedStats = effects.Apply(stats);
            attack.Damage = (int)(attack.Damage * changedStats.Protection);
        }
    }
}