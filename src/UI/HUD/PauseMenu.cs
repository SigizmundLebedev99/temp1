using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using temp1.Factories;
using temp1.UI.Controls;

namespace temp1.UI
{
    class PauseMenu : Desktop
    {
        public PauseMenu(Game1 game) : base(game.Batch)
        {
            var _factory = game.Services.GetService<ControlsFactory>();

            var label = _factory.CreateLabel(fontName: "fonts/commodore64");
            label.OffsetFrom = Anchors.TopCenter;
            label.Offset = new Vector2(0, 40);
            label.Text = "Pause";
            label.ComputeSize(Vector2.Zero, Autosize.Content);

            var content = new ContentControll();
            content.OffsetFrom = Anchors.Center;

            var start = _factory.CreateTextButton(0);
            start.OffsetFrom = Anchors.Center;
            start.Offset = new Vector2(0, -80);
            start.Text = "Continue";
            start.MouseUp += (s, e) => GameContext.Hud.Default();

            var save = _factory.CreateTextButton(0);
            save.OffsetFrom = Anchors.Center;
            save.Text = "Save";
            save.MouseUp += (s, e) => {
                SaveContext.SaveWorld();
                GameContext.Hud.Default();
            };

            var exit = _factory.CreateTextButton(0);
            exit.OffsetFrom = Anchors.Center;
            exit.Offset = new Vector2(0, 80);
            exit.Text = "Exit";

            var panel = _factory.CreatePanel(4);
            panel.ComputeSize(Size, Autosize.Content);

            content.Children.Add(panel);
            content.Children.Add(start);
            content.Children.Add(save);
            content.Children.Add(exit);
            content.Children.Add(label);

            content.ComputeSize(Size, Autosize.Content);

            Root.Children.Add(content);
            this.Update(new GameTime());
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