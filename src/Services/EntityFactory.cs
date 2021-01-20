using MonoGame.Extended.Entities;
using temp1.Components;

namespace temp1.Services
{
    class EntityFactory
    {
        private readonly World _world;
        public EntityFactory(World world)
        {
            _world = world;
        }

        public Entity CreateActor()
        {
            var entity = _world.CreateEntity();
            entity.Attach(new Stats { MaxHp = 100, Protection = 0.0f });
            entity.Attach(new TurnPartitioner(1));
            entity.Attach(new Health { HP = 100 });
            entity.Attach(new Effects());
            entity.Attach(new Equipment());
            return entity;
        }

        public Entity CreateAttack(Attack attack)
        {
            var entity = _world.CreateEntity();
            entity.Attach(attack);
            return entity;
        }
    }
}