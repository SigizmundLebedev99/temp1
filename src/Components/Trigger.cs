namespace temp1.Components
{

    abstract class Trigger
    {
        public bool ForPlayerOnly;

        public abstract void OnTrigger(WalkAction action);
    }
}