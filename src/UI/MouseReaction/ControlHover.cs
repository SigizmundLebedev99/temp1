using Microsoft.Xna.Framework.Input;
using temp1.UI.Controls;
using temp1.UI.DrawingPieces;

namespace temp1.UI.MouseReactions
{
    public class ControlHover : MouseReaction
    {
        IDrawingPiece hover;
        IDrawingPiece original;
        public ControlHover(IDrawingPiece hover)
        {
            this.hover = hover;
        }

        public override void OnEnter(Control control, MouseState state)
        {
            original = control.DrawingPiece;
            control.DrawingPiece = hover;
            base.OnEnter(control, state);
        }

        public override void OnLeave(Control control, MouseState state)
        {
            control.DrawingPiece = original;
            base.OnLeave(control, state);
        }
    }
}