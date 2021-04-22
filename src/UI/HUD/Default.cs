using Microsoft.Xna.Framework;
using temp1.Factories;
using temp1.UI.Controls;

namespace temp1.UI
{
    public class Default : Desktop
    {
        public Default(Game game): base(game.GraphicsDevice){
            var factory = game.Services.GetService<ControlsFactory>();
            
            var root = new ContentControll();
            root.Size = Size;

            var inventory = factory.CreateButton(2);
            inventory.OffsetFrom = Anchors.BottomRight;
            inventory.Offset = new Vector2(-20,-20);
            root.AddChild(inventory);

            Root = root;
        }
    }
}