using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;
using MonoGame.Extended.Entities;
using temp1.AI;
using temp1.Components;
using temp1.Systems;

namespace temp1
{
    class WorldContext
    {
        private EntitySubscription actorsSubscription;
        private EntitySubscription mapObjectsSubscription;
        private World _world;

        public Bag<int> Actors => actorsSubscription.ActiveEntities;
        public Bag<int> MapObjects => mapObjectsSubscription.ActiveEntities;

        public World World => _world;

        public void ConfigureWorld(MapContext map, SpriteBatch batch, GameObjectsContext go, ContentManager content)
        {
            _world = new WorldBuilder().AddSystem(new TurnBasedCombatSystem(this))
            .AddSystem(new WalkActionSystem(this))
            .AddSystem(new OpenStorageActionSystem())
            .AddSystem(new PeakItemActionSystem())
            .AddSystem(new BaseActionSystem())
            .AddSystem(new PossibleMovementBuildSystem(map))
            .AddSystem(new CursorSystem(batch, map, content))
            .AddSystem(new AISystem())
            .AddSystem(new TransparensySystem())
            .AddSystem(new ExpirationSystem())
            .AddSystem(new DirectionToAnimationSystem())
            .AddSystem(new SpriteRenderSystem(batch))
            .AddSystem(new PossibleMovementDrawSystem(batch, content))
            .AddSystem(new SpawnSystem(map, go, content)).Build();

            actorsSubscription = new EntitySubscription(_world.EntityManager, Aspect.All(typeof(BaseAI)).Build(_world.ComponentManager));
            mapObjectsSubscription = new EntitySubscription(_world.EntityManager, Aspect.All(typeof(MapObject)).Build(_world.ComponentManager));
        }

        public Mapper<T> GetMapper<T>() where T : class
        {
            return _world.ComponentManager.Get<T>();
        }

        public Entity CreateEntity()
        {
            return _world.CreateEntity();
        }

        public Entity GetEntity(int id)
        {
            return _world.GetEntity(id);
        }
    }
}