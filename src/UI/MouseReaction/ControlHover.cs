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
            if(hover == null)
                return;
            original = control.Background;
            control.Background = hover;
            base.OnEnter(control, state);
        }

        public override void OnLeave(Control control, MouseState state)
        {
            if(hover == null)
                return;
            control.Background = original;
            base.OnLeave(control, state);
        }
    }
}