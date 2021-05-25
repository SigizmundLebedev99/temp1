using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using temp1.Components;
using temp1.Data;
using temp1.Factories;
using temp1.UI.Controls;

namespace temp1.UI
{
    class InventoryOpen : Desktop
    {
        InventoryArea inventory;

        public InventoryOpen(Game1 game) : base(game.Batch, SpriteSortMode.FrontToBack)
        {
            var factory = game.Services.GetService<ControlsFactory>();

            var root = new ContentControll();
            root.Size = Size;

            var frame = new ContentControll();
            frame.OffsetFrom = Anchors.Center;

            #region inventory
            var items = new WrapContent();
            items.OffsetFrom = Anchors.Center;

            var panel = factory.CreatePanel(4);
            panel.ComputeSize(Vector2.Zero, Autosize.Content);
            panel.OffsetFrom = Anchors.Center;

            items.Size = panel.Size - new Vector2(50);

            inventory = new InventoryArea(items, factory);
            inventory.OffsetFrom = Anchors.Center;
            inventory.Size = items.Size;

            #endregion

            #region equipment
            var helmetPanel = factory.CreatePanel(textureName: "ui/equipment/helmet");
            var helmetSlot = new ItemSlot(helmetPanel, ItemTypeFlags.Helmet);
            helmetSlot.OffsetFrom = Anchors.TopRight;
            helmetSlot.Offset = new Vector2(50, 0);

            var weaponPanel = factory.CreatePanel(textureName: "ui/equipment/sword");
            var weaponSlot = new ItemSlot(weaponPanel, ItemTypeFlags.Weapon);
            weaponSlot.OffsetFrom = Anchors.TopRight;
            weaponSlot.Offset = new Vector2(50, 55);
            #endregion

            frame.Size = panel.Size;
            frame.Children.Add(inventory);
            frame.Children.Add(panel);
            frame.Children.Add(items);
            frame.Children.Add(helmetSlot);
            frame.Children.Add(weaponSlot);
            
            root.Children.Add(frame);
            Root = root;
        }

        public void BuildItems(Storage storage)
        {
            inventory.BuildItems(storage);
        }

        public override void Update(GameTime time)
        {
            var keyState = KeyboardExtended.GetState();
            if(keyState.WasKeyJustDown(Keys.Escape)){
                GameContext.Hud.Default();
                return;
            }
            base.Update(time);
        }
    }
}