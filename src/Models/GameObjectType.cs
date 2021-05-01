using System;
using Microsoft.Xna.Framework;
using temp1.Components;

namespace temp1.Data
{
    [Flags]
    enum GameObjectType
    {
        None = 0,
        Storage = 1,
        Enemy = 2,
        Item = 4,
        Blocking = 8
    }

    class Origin
    {
        public int X;
        public int Y;
    }

    class GameObjectTypeInfo
    {
        public string TypeName;
        public GameObjectType Type;
        public string Path;
        public Origin Origin;
        public Region Region;
        public string Handler;
        public int StackSize;
        public ItemTypeFlags Flags;
    }
}

