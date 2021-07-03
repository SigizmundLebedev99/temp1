using System.Diagnostics;
using DefaultEcs;
using Microsoft.Xna.Framework;
using temp1.Components;
using temp1.UI.Controls;

namespace temp1.UI
{
    enum HUDState
    {
        Default,
        Inventory1,
        Inventory2,
        Pause,
    }

    class HudContext
    {
        public HUDState State => _state;
        public bool IsMouseOnHud;

        private HUDState _state;
        private Desktop _ui;
        private Game1 _game;

        public HudContext(Game1 game)
        {
            _game = game;
        }
        
        public void Default()
        {
            _ui = new Default(_game);
            _state = HUDState.Default;
        }

        public void OpenInventory(Entity entity)
        {
            IsMouseOnHud = false;
            var inventory = new InventoryOpen(_game);
            var storage = entity.Get<Storage>();
            
            if(storage == null){
                Debug.WriteLine("_warn_ HUDContext.cs 47");
                return;
            }

            inventory.BuildItems(storage);
            _ui = inventory;
            _state = HUDState.Inventory1;
        }

        public void OpenChest(Storage left, Storage right)
        {
            var inventory = new ChestOpen(_game);
            inventory.BuildItems(left, right);
            _ui = inventory;
            _state = HUDState.Inventory2;
        }

        public void Pause()
        {
            _ui = new PauseMenu(_game);
            _state = HUDState.Pause;
        }

        public void Draw(GameTime time)
        {
            if (_ui != null)
                _ui.Draw(time);
        }

        public void Update(GameTime time)
        {
            if (_ui != null)
                _ui.Update(time);
        }
    }
}