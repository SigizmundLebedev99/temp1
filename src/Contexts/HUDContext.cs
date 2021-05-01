using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
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
        
        public HUDState State => _state;
        public bool IsMouseOnHud;

        private HUDState _state;
        private Desktop _ui;
        private Game1 _game;

        private Mapper<Storage> _storageMap;

        public HudContext(Game1 game, WorldContext context)
        {
            _game = game;
            _storageMap = context.GetMapper<Storage>();
        }
        
        public void Default()
        {
            _ui = new Default(_game);
            _state = HUDState.Default;
        }

        public void OpenInventory(int id = -1)
        {
            IsMouseOnHud = false;
            if(id == -1)
                id = GameContext.PlayerId;
            var inventory = new InventoryOpen(_game);
            var storage = _storageMap.Get(id);
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