using System;
using Microsoft.Xna.Framework;
using temp1.Components;
using temp1.UI.Controls;

namespace temp1.UI
{
    enum UIState
    {
        Default,
        Inventory1,
        Inventory2,
    }

    class HUDService
    {
        private UIState _state;
        private Desktop _ui;
        private Game _game;

        public UIState State => _state;
        public bool IsMouseOnHud => false;

        public HUDService(Game game)
        {
            _game = game;
        }

        
        public void Default()
        {
            _ui = new Default(_game);
        }


        public void OpenInventory1(Storage storage)
        {

        }

        public void OpenInventory2(Storage left, Storage right)
        {

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