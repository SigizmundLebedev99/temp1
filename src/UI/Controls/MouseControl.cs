using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace temp1.UI.Controls
{
    public enum ControlMS
    {
        None,
        Hot,
        Pressed
    }

    public abstract class MouseControl : Control
    {
        public event Action<MouseControl, MouseState> MouseDown;
        public event Action<MouseControl, MouseState> MouseUp;
        public event Action<MouseControl, MouseState> MouseEnter;
        public event Action<MouseControl, MouseState> MouseLeave;

        protected ControlMS previousState = ControlMS.None;
        public ControlMS State;

        protected bool IsIn(Vector2 position, MouseState state)
        {
            return new RectangleF(position, Size).Contains(state.Position);
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            if (IsIn(position, mouse))
            {
                if (previousState == ControlMS.None)
                {
                    MouseEnter?.Invoke(this, mouse);
                    State = ControlMS.Hot;
                }
                else if ((mouse.LeftButton == ButtonState.Pressed || mouse.RightButton == ButtonState.Pressed) && previousState != ControlMS.Pressed)
                {
                    MouseDown?.Invoke(this, mouse);
                    State = ControlMS.Pressed;
                }
                else if (mouse.LeftButton == ButtonState.Released && mouse.RightButton == ButtonState.Released && previousState == ControlMS.Pressed)
                {
                    MouseUp?.Invoke(this, mouse);
                    State = ControlMS.Hot;
                }
            }
            else
            {
                if (previousState != ControlMS.None)
                {
                    MouseLeave?.Invoke(this, mouse);
                    State = ControlMS.None;
                }
            }
            previousState = State;
        }
    }
}