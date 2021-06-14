using System;
using DefaultEcs;

namespace temp1.Components
{
    class SingleAction : BaseAction
    {
        Action<Entity> _action;
        
        public SingleAction(Action<Entity> action)
        {
            _action = action;
        }

        public override void Start(in Entity entity){
            _action?.Invoke(entity);
        }
        
        public override int PointsTaken => 0;
    }
}