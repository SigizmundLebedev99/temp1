using System;
using Microsoft.Xna.Framework.Content;
using Myra.Graphics2D.UI;
using temp1.Components;
using temp1.UI;

namespace temp1
{
    enum HudState{
        Default,
        Inventry1Opened,
        Inventry2Opened
    }

    class HudService
    {
        Inventory1 inventory1;
        Inventory2 inventory2;
        Hud hud;
        Desktop desktop;
        
        public HudState HudState = HudState.Default;
        public bool IsMouseOnHud => isMouseOnHud;

        private bool isMouseOnHud;

        public HudService(ContentManager contentManager, GameContext context)
        {
            desktop = new Desktop();
            inventory1 = new Inventory1(context, this);
            inventory2 = new Inventory2(context, this);
            hud = new Hud(contentManager, this, context);
            Default();
        }

        public void SetCallbacks(Widget widget){
            widget.MouseEntered += (s,e) => isMouseOnHud = true;
            widget.MouseLeft += (s,e) => isMouseOnHud = false;
        }

        public void OpenInventory2(Storage left, Storage right)
        {
            inventory2.Build(left, right);
            desktop.Root = inventory2;
            HudState = HudState.Inventry2Opened;
        }

        public void OpenInventory1(){
            inventory1.Open();
            desktop.Root = inventory1;
            HudState = HudState.Inventry1Opened;
        }

        public void Default(){
            desktop.Root = hud;
            HudState = HudState.Default;
            isMouseOnHud = false;
        }

        internal void Draw()
        {
            desktop.Render();
        }
    }
}