using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using temp1.UI;
using temp1.UI.Controls;
using temp1.UI.DrawingPieces;
using temp1.UI.MouseReactions;

namespace temp1.Screens
{
    class SavesScreen : GameScreen
    {
        Desktop _desktop;

        public SavesScreen(Game game) : base(game)
        {
            _desktop = new Desktop(((Game1)Game).Batch);

            var root = _desktop.Root;

            var savesScroll = new Scroll();
            savesScroll.OffsetFrom = Anchors.Center;
            savesScroll.Background = new TexturePiece(Content.Load<Texture2D>("ui/panel1"), new DrawOptions(new Vector2(200, 400), new Margin(25, 25)));

            var saves = SaveContext.GetSaves();
            var scrollContent = new ContentControll();
            var offset = new Vector2();
            foreach (var save in saves)
            {
                var saveControl = new MouseControl();
                saveControl.Offset = offset;
                saveControl.Background = new BackgroundColorPiece(Color.Blue, saveControl);
                saveControl.MouseReaction.AddReaction(new ControlHover(new BackgroundColorPiece(Color.BlueViolet, saveControl)));
                saveControl.Text.Value = save.Name;
                saveControl.Size = new Vector2(savesScroll.Size.X, 30);
                offset += new Vector2(0, saveControl.Size.Y + 5);
                scrollContent.Children.Add(saveControl);

                saveControl.MouseUp += (s, e) =>
                {
                    SaveContext.LoadGame(save);
                };
            }
            scrollContent.ComputeSize();
            savesScroll.Content = scrollContent;
            root.Children.Add(savesScroll);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);
            _desktop.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            _desktop.Update(gameTime);
        }
    }
}