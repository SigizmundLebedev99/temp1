using MonoGame.Extended.Collections;
using temp1.Data;

namespace temp1.Components
{
    class AllowedToAct{}
    class Player{}
    class Portal{}
    class Enemy{}
    
    class Storage{
        public Bag<FilledSlot> Content = new Bag<FilledSlot>();
    }

    class Hull{
        public bool IsPlayerIn = false;
    }
}