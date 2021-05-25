using DefaultEcs;
using temp1.AI;
using temp1.Components;

namespace temp1
{
    class EntitySets
    {
        public EntitySet MapObjects { get; }
        public EntitySet Actors { get; }
        public EntitySet Triggers { get; }
        public EntitySet Cursors { get; }
        public EntitySet Serializable { get; }

        public EntitySets(World world)
        {
            MapObjects = world.GetEntities().With<Position>().AsSet();
            Actors = world.GetEntities().With<IGameAI>().With<ActionPoints>().AsSet();
            Triggers = world.GetEntities().With<Trigger>().AsSet();
            Cursors = world.GetEntities().With<Cursor>().AsSet();
            Serializable = world.GetEntities().With<Serializable>().AsSet();
        }
    }
}