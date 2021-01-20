using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using temp1.Components;

namespace temp1.Systems
{
    class EffectsSystem : EntityProcessingSystem
    {
        private ComponentMapper<NextTurn> _ntMapper;
        private ComponentMapper<Effects> _effectsMapper;
        
        public EffectsSystem() : base(Aspect.All(typeof(NextTurn)))
        {}

        public override void Initialize(IComponentMapperService mapperService)
        {
        }

        public override void Process(GameTime gameTime, int entityId)
        {
        }
    }
}