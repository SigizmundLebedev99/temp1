using Microsoft.Xna.Framework;
using temp1.Factories;
using temp1.UI.Controls;

namespace temp1.UI
{
    public class Default : Desktop
    {
        public Default(Game1 game) : base(game.Batch)
        {
            var factory = game.Services.GetService<ControlsFactory>();

            var root = new ContentControll();
            root.Size = Size;

            var inventory = factory.CreateButton<Button>(2);
            inventory.OffsetFrom = Anchors.BottomRight;
            inventory.Offset = new Vector2(-20, -30);
            
            inventory.MouseUp += (s,e) => GameContext.Hud.OpenInventory(GameContext.Player);

            var toBattle = factory.CreateButton<Button>(8);
            toBattle.OffsetFrom = Anchors.BottomRight;
            toBattle.Offset = new Vector2(-80, -30);

            toBattle.MouseUp += (s,e) => GameContext.Combat.StartBattle();

            var panel = factory.CreatePanel(5);
            panel.ComputeSize(Vector2.Zero, Autosize.Content);
            panel.OffsetFrom = Anchors.BottomRight;
            var mousePanel = new MouseControl();
            mousePanel.Size = panel.Size;
            mousePanel.OffsetFrom = Anchors.BottomRight;

            mousePanel.MouseEnter += (s,e) => GameContext.Hud.IsMouseOnHud = true;
            mousePanel.MouseLeave += (s,e) => GameContext.Hud.IsMouseOnHud = false;

            root.Children.Add(panel);
            root.Children.Add(mousePanel);
            root.Children.Add(inventory);
            root.Children.Add(toBattle);

            Root = root;
        }
    }
}