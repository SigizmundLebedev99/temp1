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

    class ItemStack
    {
        public GameObjectTypeInfo ItemType;
        public int Count;
    }
}
