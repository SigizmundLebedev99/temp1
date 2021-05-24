using DefaultEcs;
using temp1.AI;
using temp1.Components;

namespace temp1
{
    class EntitySets
    {
        public EntitySet MapObjects { get; }
        public EntitySet Actors { get; set; }

        public EntitySets(World world)
        {
            MapObjects = world.GetEntities().With<MapObject>().AsSet();
            Actors = world.GetEntities().With<IBaseAI>().With<ActionPoints>().AsSet();
        }
    }
}