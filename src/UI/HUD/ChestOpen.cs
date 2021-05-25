using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using temp1.Components;
using temp1.Factories;
using temp1.UI.Controls;

namespace temp1.UI
{
    class ChestOpen : Desktop
    {
        InventoryArea leftInventory;
        InventoryArea rightInventory;

        public ChestOpen(Game1 game) : base(game.Batch, SpriteSortMode.FrontToBack)
        {
            var factory = game.Services.GetService<ControlsFactory>();

            var root = new ContentControll();
            root.Size = Size;

            var frame = new ContentControll();
            frame.OffsetFrom = Anchors.Center;

            #region left_inventory

            var leftFrame = new ContentControll();

            var leftItems = new WrapContent();
            leftItems.OffsetFrom = Anchors.Center;

            var leftPanel = factory.CreatePanel(4);
            leftPanel.OffsetFrom = Anchors.Center;

            leftItems.Size = leftPanel.Size - new Vector2(50);

            leftInventory = new InventoryArea(leftItems, factory);
            leftInventory.Size = leftItems.Size;
            leftInventory.OffsetFrom = Anchors.Center;

            leftFrame.Children.Add(leftPanel);
            leftFrame.Children.Add(leftItems);
            leftFrame.Children.Add(leftInventory);

            leftFrame.Size = leftPanel.Size;
            leftFrame.OffsetFrom = Anchors.CenterRight;

            #endregion

            #region right_inventory

            var rightFrame = new ContentControll();

            var rightItems = new WrapContent();
            rightItems.OffsetFrom = Anchors.Center;

            var rightPanel = factory.CreatePanel(4);
            rightPanel.OffsetFrom = Anchors.Center;

            rightItems.Size = rightPanel.Size - new Vector2(50);

            rightInventory = new InventoryArea(rightItems, factory);
            rightInventory.Size = rightItems.Size;
            rightInventory.OffsetFrom = Anchors.Center;

            rightFrame.Children.Add(rightPanel);
            rightFrame.Children.Add(rightItems);
            rightFrame.Children.Add(rightInventory);
            rightFrame.OffsetFrom = Anchors.CenterLeft;

            rightFrame.Size = rightPanel.Size;

            #endregion

            frame.Children.Add(leftFrame);
            frame.Children.Add(rightFrame);

            root.Children.Add(frame);
            Root = root;
        }

        public void BuildItems(Storage left, Storage right)
        {
            leftInventory.BuildItems(left);
            rightInventory.BuildItems(right);
        }

        public override void Update(GameTime time)
        {
            var keyState = KeyboardExtended.GetState();
            if (keyState.WasKeyJustDown(Keys.Escape))
            {
                GameContext.Hud.Default();
                return;
            }
            base.Update(time);
        }
    }
}