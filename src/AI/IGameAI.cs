using DefaultEcs;
using Microsoft.Xna.Framework;
using temp1.Models.Serialization;

namespace temp1.AI
{
    interface IGameAI
    {
        AIFactory GetFactory();
        void Update(GameTime time, Entity entity);
    }
}