using System.Linq;
using MonoGame.Extended.Collections;
using temp1.Data;

namespace temp1.Components
{
    class Storage{
        public Bag<FilledSlot> Content = new Bag<FilledSlot>();

        public void Add(FilledSlot slot){
            var existing = Content.FirstOrDefault(e => e.ItemType == slot.ItemType && e.Count < e.ItemType.stackSize);
            
            if(existing == null){
                Content.Add(slot);
                return;
            }

            var total = existing.Count + slot.Count;
            existing.Count = total > slot.ItemType.stackSize ? slot.ItemType.stackSize : total;
            slot.Count = total - existing.Count;
            if(slot.Count > 0)
                Content.Add(slot);
        }
    }
}