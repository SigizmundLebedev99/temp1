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
            label.Text.Value = "Pause";

            var yOffset = -80;
            MouseControl createButton(string text)
            {
                var button = _factory.CreateTextButton(0);
                button.OffsetFrom = Anchors.Center;
                button.Offset = new Vector2(0, yOffset);
                //button.Text = text;
                yOffset += 80;
                return button;
            }

            var content = new ContentControll();
            content.OffsetFrom = Anchors.Center;

            var start = createButton("Continue");
            start.MouseUp += (s, e) => GameContext.Hud.Default();

            var save = createButton("Save");
            save.MouseUp += (s, e) =>
            {
                //SaveContext.SaveMapState();
                GameContext.Hud.Default();
            };

            var exit = createButton("Exit");

            var panel = _factory.CreatePanel(4);
            content.Children.Add(panel);
            content.Children.Add(start);
            content.Children.Add(save);
            content.Children.Add(exit);
            content.Children.Add(label);

            content.ComputeSize();

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