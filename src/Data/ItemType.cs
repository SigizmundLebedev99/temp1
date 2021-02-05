using System;

namespace temp1.Data
{
    [Flags]
    enum ItemTypeFlags
    {
        Comsumable = 1,
        Offensive = 2,
        Armor = 4,
        Weapon = 8
    }

    class ItemType
    {
        public string type;
        public int flags;
        public int stackSize;
        public string image;
    }

    class FilledSlot
    {
        public ItemType ItemType;
        public int Count;
    }
}

