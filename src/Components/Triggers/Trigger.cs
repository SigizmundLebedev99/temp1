using System;
using DefaultEcs;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace temp1.Components
{
    interface ITrigger
    {
        void Check(in Entity self, MoveAction action, in Entity actionEntity);
    }
}