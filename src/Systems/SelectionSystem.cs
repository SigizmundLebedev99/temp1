using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace temp1.Systems
{
    class SelectionSystem : EntityUpdateSystem
    {
        public SelectionSystem() : base(Aspect.All())
        {
            
        }

        public override void Initialize(IComponentMapperService mapperService)
        {

        }

        public override void Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }
    }
}