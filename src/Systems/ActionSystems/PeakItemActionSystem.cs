using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using temp1.Components;

namespace temp1.Systems
{
    class PeakItemActionSystem : EntityProcessingSystem
    {
        public PeakItemActionSystem() : base(Aspect.All(typeof(PeakItemAction)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            mapperService.Get<PeakItemAction>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var entity = GetEntity(entityId);
            var action = entity.Get<PeakItemAction>();
            entity.Detach<PeakItemAction>();
            entity.Attach<BaseAction>(action);
        }
    }
}