using temp1.AI;

namespace temp1.Models
{
    abstract class AIFactory
    {
        public abstract IGameAI Get();
    }

    class PlayerControlAIFactory : AIFactory
    {
        public override IGameAI Get()
        {
            return new PlayerControl();
        }
    }

    class RandomMovementAIFactory : AIFactory
    {
        public override IGameAI Get()
        {
            return new RandomMovement();
        }
    }
}