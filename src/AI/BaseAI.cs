using Microsoft.Xna.Framework;

namespace temp1.AI
{
    abstract class BaseAI
    {
        public readonly int EntityId;

        public BaseAI(int entityId)
        {
            EntityId = entityId;
        }

        public abstract void Update(GameTime time);
    }
}