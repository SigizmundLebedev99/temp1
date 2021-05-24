using DefaultEcs;
using Microsoft.Xna.Framework;

namespace temp1.AI
{
    interface IBaseAI
    {
        void Update(GameTime time, Entity entity);
    }
}