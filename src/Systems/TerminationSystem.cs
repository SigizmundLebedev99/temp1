using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class TerminationSystem : EntityProcessingSystem
    {
        public TerminationSystem() : base(Aspect.One(
        typeof(Attack),
        typeof(EndOfTurn),
        typeof(BattleStart),
        typeof(BattleEnd),
        typeof(NextTurn))) { }

        public override void Initialize(IComponentMapperService mapperService)
        { }

        public override void Process(GameTime gameTime, int entityId)
        {
            DestroyEntity(entityId);
        }
    }
}