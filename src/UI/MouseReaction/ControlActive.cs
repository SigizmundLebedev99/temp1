using Microsoft.Xna.Framework.Input;
using temp1.UI.Controls;
using temp1.UI.DrawingPieces;

namespace temp1.UI.MouseReactions
{
    public class ControlActive : MouseReaction
    {
        IDrawingPiece active;
        IDrawingPiece original;

        public ControlActive(IDrawingPiece active)
        {
            this.active = active;
        }

        public override void OnMouseDown(Control control, MouseState state)
        {
            original = control.DrawingPiece;
            control.DrawingPiece = active;
            base.OnEnter(control, state);
        }

        public override void OnMouseUp(Control control, MouseState state)
        {
            control.DrawingPiece = original;
            base.OnMouseDown(control, state);
        }
    }
}