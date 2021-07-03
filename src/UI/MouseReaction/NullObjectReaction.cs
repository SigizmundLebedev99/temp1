using Microsoft.Xna.Framework.Input;
using temp1.UI.Controls;
using temp1.UI.DrawingPieces;

namespace temp1.UI.MouseReactions
{
    public class NullObjectReaction : MouseReaction
    {
        MouseControl _control;
        public NullObjectReaction(MouseControl control)
        {
            _control = control;
        }

        public override void AddReaction(MouseReaction reaction)
        {
            _control.MouseReaction = reaction;
        }
    }
}