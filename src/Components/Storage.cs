using System.Collections.Generic;
using System.Linq;
using temp1.Models;

namespace temp1.Components
{
    class Storage
    {
        public List<ItemStack> Items = new List<ItemStack>();

        public void Add(ItemStack slot)
        {
            var existing = Items.FirstOrDefault(e => e.ItemType == slot.ItemType && e.Count < e.ItemType.StackSize);

            if (existing == null)
            {
                Items.Add(slot);
                return;
            }

            var total = existing.Count + slot.Count;
            existing.Count = total > slot.ItemType.StackSize ? slot.ItemType.StackSize : total;
            slot.Count = total - existing.Count;
            if (slot.Count > 0)
                Items.Add(slot);
        }
    }
}