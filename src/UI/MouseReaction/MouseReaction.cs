using Microsoft.Xna.Framework.Input;
using temp1.UI.Controls;

namespace temp1.UI.MouseReactions
{
    public abstract class MouseReaction
    {
        protected MouseReaction next;

        public virtual void OnEnter(Control control, MouseState state)
        {
            next?.OnEnter(control, state);
        }
        public virtual void OnLeave(Control control, MouseState state)
        {
            next?.OnLeave(control, state);
        }
        public virtual void OnMouseDown(Control control, MouseState state)
        {
            next?.OnMouseDown(control, state);
        }
        public virtual void OnMouseUp(Control control, MouseState state)
        {
            next?.OnMouseUp(control, state);
        }

        public virtual void AddReaction(MouseReaction reaction)
        {
            if (next == null)
                next = reaction;
            else if (next.GetType() == reaction.GetType())
            {
                reaction.next = next.next;
                next = reaction;
            }
            else
            {
                next.AddReaction(reaction);
            }
        }
    }
}