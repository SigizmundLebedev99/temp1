using System;
using Microsoft.Xna.Framework;
using MonoGame.Squid.Controls;
using temp1.Components;

namespace temp1.UI
{
    enum UIState
    {
        Default,
        Inventory1,
        Inventory2,
    }

    class UIService
    {
        private UIState _state;
        private Desktop _ui;

        internal void Default()
        {
            throw new NotImplementedException();
        }

        public UIState State => _state;
        public bool IsMouseOnHud => false;

        public UIService(Game game)
        {
            _ui = new Desktop();
            CreateSkin();
        }

        public void GoToMainMenu()
        {

        }

        public void OpenInventory1(Storage storage)
        {

        }

        public void OpenInventory2(Storage left, Storage right)
        {

        }

        public void Draw()
        {
            _ui.Draw();
        }

        private void CreateSkin()
        {
            _ui.Skin = Styling.Skin;

            _ui.CursorSet = Styling.Cursors;
        }
    }
}