using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using temp1.UI.MouseReactions;

namespace temp1.UI.Controls
{
    public enum ControlMS
    {
        None,
        Hot,
        Pressed
    }

    public class MouseControl : Control
    {
        private MouseReaction mouseReaction;
        public MouseReaction MouseReaction { get => mouseReaction ?? (mouseReaction = new NullObjectReaction(this)); set { mouseReaction = value; } }

        public event Action<MouseControl, MouseState> MouseDown;
        public event Action<MouseControl, MouseState> MouseUp;
        public event Action<MouseControl, MouseState> MouseEnter;
        public event Action<MouseControl, MouseState> MouseLeave;

        protected ControlMS previousState = ControlMS.None;
        public ControlMS State { get; private set; }

        static MouseControl _selectedControl;
        public bool IsSelected => _selectedControl == this;

        protected bool IsIn(Vector2 position, MouseState state)
        {
            return new RectangleF(position, Size).Contains(state.Position);
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            previousState = State;
            base.Update(time, mouse, position);
            var buttonPressed = (mouse.LeftButton == ButtonState.Pressed || mouse.RightButton == ButtonState.Pressed);
            if (IsIn(position, mouse))
            {
                if (previousState == ControlMS.None)
                {
                    MouseReaction.OnEnter(this, mouse);
                    MouseEnter?.Invoke(this, mouse);
                    State = ControlMS.Hot;
                }
                else if (buttonPressed && previousState != ControlMS.Pressed)
                {
                    MouseReaction.OnMouseDown(this, mouse);
                    MouseDown?.Invoke(this, mouse);
                    State = ControlMS.Pressed;
                    if(_selectedControl == null)
                        _selectedControl = this;
                }
                else if (!buttonPressed && previousState == ControlMS.Pressed)
                {
                    MouseReaction.OnMouseUp(this, mouse);
                    MouseUp?.Invoke(this, mouse);
                    State = ControlMS.Hot;
                    if(IsSelected)
                        _selectedControl = null;
                }
            }
            else
            {
                if(IsSelected && !buttonPressed)
                    _selectedControl = null;
                if (previousState != ControlMS.None)
                {
                    MouseReaction.OnLeave(this, mouse);
                    MouseLeave?.Invoke(this, mouse);
                    State = ControlMS.None;
                }
            }
        }

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth = 0)
        {
            base.Draw(time, batch, position, depth);
        }
    }
}