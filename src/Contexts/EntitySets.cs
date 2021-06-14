using DefaultEcs;
using temp1.AI;
using temp1.Components;

namespace temp1
{
    class EntitySets
    {
        private EntitySet playerSet;

        public EntitySet MapObjects { get; }
        public EntitySet Actors { get; }
        public EntitySet Triggers { get; }
        public EntitySet Cursors { get; }
        public EntitySet Serializable { get; }
        public Entity Player => playerSet.GetEntities()[0];

        public EntitySets(World world)
        {
            MapObjects = world.GetEntities().With<Position>().AsSet();
            Actors = world.GetEntities().With<ActionPoints>().AsSet();
            Triggers = world.GetEntities().With<ITrigger>().AsSet();
            Cursors = world.GetEntities().With<Cursor>().AsSet();
            Serializable = world.GetEntities().With<Serializable>().AsSet();
            playerSet = world.GetEntities().With<Player>().AsSet();
        }
    }
}