using FluentBehaviourTree;
using Microsoft.Xna.Framework;

namespace temp1.AI
{
    abstract class BaseAI
    {
        protected GameContext Context;
        public readonly int EntityId;

        public BaseAI(int entityId, GameContext context)
        {
            EntityId = entityId;
            Context = context;
        }

        public abstract void Update(GameTime time);
    }
}