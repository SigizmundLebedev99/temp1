using MonoGame.Extended.Collections;

namespace temp1.Components
{
    class BattleStart
    {
        public int First;
        public Bag<int> Combatants;

        public BattleStart(int first, Bag<int> combatants)
        {
            First = first;
            Combatants = combatants;
        }
    }
}