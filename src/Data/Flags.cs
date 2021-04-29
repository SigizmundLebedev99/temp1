using System;

namespace temp1.Data
{
    [Flags]
    enum ItemTypeFlags
    {
        Comsumable = 1,
        Armor = 2,
        Weapon = 4,
    }

    class ItemStack
    {
        public GameObjectTypeInfo ItemType;
        public int Count;
    }
}

