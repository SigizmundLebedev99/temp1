using System;
using temp1.Models;

namespace temp1.Data
{
    enum GameObjectType
    {
        None = 0,
        Storage = 1,
        Enemy = 2,
        Item = 4
    }

    class GameObjectTypeInfo
    {
        public string TypeName;
        public GameObjectType Type;
        public RenderObjectInfo Sprite;
        public string Handler;
        public int StackSize;
        public ItemTypeFlags Flags;
    }
}

