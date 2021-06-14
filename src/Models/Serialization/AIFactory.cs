using temp1.AI;

namespace temp1.Models.Serialization
{
    abstract class AIFactory
    {
        public abstract IGameAI Get();
    }

    class RandomMovementAIFactory : AIFactory
    {
        public override IGameAI Get()
        {
            return new RandomMovement();
        }
    }
}