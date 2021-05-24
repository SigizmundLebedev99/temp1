using DefaultEcs;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;
using temp1.AI;
using temp1.Components;
using temp1.Systems;

namespace temp1
{
    class WorldContext
    {
        private World _world;

        public World World => _world;

        public void ConfigureWorld(MapContext map, SpriteBatch batch, GameObjectsContext go, ContentManager content)
        {
            _world = new World(32);
            .AddSystem(new TurnBasedCombatSystem(this))
            .AddSystem(new WalkActionSystem(this))
            .AddSystem(new OpenStorageActionSystem())
            .AddSystem(new PeakItemActionSystem())
            .AddSystem(new BaseActionSystem())
            .AddSystem(new PossibleMovementBuildSystem(map))
            .AddSystem(new CursorSystem(batch, map, content))
            .AddSystem(new AISystem())
            .AddSystem(new CanopySystem())
            .AddSystem(new ExpirationSystem())
            .AddSystem(new DirectionToAnimationSystem())
            .AddSystem(new SpriteRenderSystem(batch))
            .AddSystem(new PossibleMovementDrawSystem(batch, content))
            .AddSystem(new SpawnSystem(map, go, content)).Build();
        }
    }
}