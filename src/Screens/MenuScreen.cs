using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;

namespace temp1.Screens
{
    class MenuScreen : GameScreen
    {
        private Desktop _desktop;
        public MenuScreen(Game game) : base(game)
        {
            MyraEnvironment.Game = game;
            var grid = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 8
            };

            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            var startButton = new TextButton()
            {
                Text = "Start",
                PressedBackground = new SolidBrush(Color.Red),
                Padding = new Thickness(5),
                GridColumn = 1,
                GridRow = 0
            };
            var quitButton = new TextButton()
            {
                Text = "Выйти",
                Padding = new Thickness(5),
                PressedBackground = new SolidBrush(Color.Red),
                GridColumn = 1,
                GridRow = 1
            };
            var dialog = ExitBox();

            startButton.Click += (e, s) =>
            {
                this.ScreenManager.LoadScreen(new PlayScreen(this.Game));
            };
            quitButton.Click += (e, s) => dialog.ShowModal(_desktop);
            grid.Widgets.Add(startButton);
            grid.Widgets.Add(quitButton);
            _desktop = new Desktop();
            _desktop.Root = grid;
        }

        Dialog ExitBox()
        {
            var panel = new HorizontalStackPanel();
            var yesButton = new TextButton
            {
                Text = "да",
                Margin = new Thickness(10,0),
                Padding = new Thickness(5)
            };
            
            var noButton = new TextButton
            {
                Text = "нет",
                Padding = new Thickness(5)
            };
            panel.Widgets.Add(yesButton);
            panel.Widgets.Add(noButton);
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            var dialog = Dialog.CreateMessageBox("Вы уверены?", panel);
            dialog.CloseButton.Visible = false;
            dialog.ButtonCancel.Visible = false;
            dialog.ButtonOk.Visible = false;
            yesButton.Click += (s,e) => Game.Exit();
            noButton.Click += (s,e) => dialog.Close();
            return dialog;
        }

        public override void Draw(GameTime gameTime)
        {
            _desktop.Render();
        }

        public override void Update(GameTime gameTime)
        { }
    }
}