namespace temp1.Components
{
    enum Slot
    {
        Head,
        Chest,
        Hands,
        Legs,
        Spine
    }

    class Item
    {
        public Slot Slot;
        public string Name;
        public int MaxHP;
        public float Protection;
    }

    class Equipment
    {
        public Slot[] Slots = new[] { Slot.Head, Slot.Chest, Slot.Hands, Slot.Legs, Slot.Spine };
        public Item[] List = new Item[5];
        public Stats Apply(Stats stats)
        {

            for (var i = 0; i < List.Length; i++)
            {
                stats = new Stats()
                {
                    MaxHp = stats.MaxHp + List[i].MaxHP,
                    Protection = stats.Protection + List[i].Protection
                };
            }
            return stats;
        }
    }
}