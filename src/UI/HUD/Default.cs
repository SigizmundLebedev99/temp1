using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            

            var panel = factory.CreatePanel(5);
            panel.ComputeSize(Vector2.Zero, Autosize.Content);
            panel.OffsetFrom = Anchors.BottomRight;

            root.Children.Add(panel);
            root.Children.Add(inventory);

            Root = root;
        }
    }
}