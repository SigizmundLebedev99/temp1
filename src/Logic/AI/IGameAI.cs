using DefaultEcs;
using Microsoft.Xna.Framework;
using temp1.Models.Serialization;
using temp1.PathFinding;

namespace temp1.AI
{
    interface IGameAI
    {
        AIFactory GetFactory();

        void Update(GameTime time, in Entity entity);
    }
}