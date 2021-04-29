using System;
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
    }

    class HudContext
    {
        private HUDState _state;
        private Desktop _ui;
        private Game1 _game;

        public HUDState State => _state;
        public bool IsMouseOnHud => false;

        public HudContext(Game1 game)
        {
            _game = game;
        }
        
        public void Default()
        {
            _ui = new Default(_game);
        }


        public void OpenInventory1(Storage storage)
        {
            var inventory = new InventoryOpen(_game);
            inventory.BuildItems(storage);
            _ui = inventory;
            _state = HUDState.Inventory1;
        }

        public void OpenInventory2(Storage left, Storage right)
        {
            var inventory = new InventoryOpen(_game);
            inventory.BuildItems(left);
            _ui = inventory;
            _state = HUDState.Inventory2;
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