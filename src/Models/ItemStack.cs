using System;

namespace temp1.Data
{
    [Flags]
    enum ItemTypeFlags
    {
        Consumable = 1,
        Armor = 2,
        Weapon = 4,
        Helmet = 8,
        OneHanded = 16,
    }

    class ItemStack
    {
        public GameObjectTypeInfo ItemType;
        public int Count; 
    }
}

