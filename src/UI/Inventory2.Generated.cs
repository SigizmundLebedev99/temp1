/* Generated by MyraPad at 05.02.2021 14:36:53 */
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace temp1.UI
{
	partial class Inventory2: Grid
	{
		private void BuildUI()
		{
			var textBox1 = new TextBox();
			textBox1.Text = "Chest";
			textBox1.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
			textBox1.GridColumn = 1;
			textBox1.GridRow = 1;
			textBox1.Background = new SolidBrush("#00000000");

			firstPanel = new VerticalStackPanel();
			firstPanel.Background = new SolidBrush("#80562962");
			firstPanel.Id = "firstPanel";

			var scrollViewer1 = new ScrollViewer();
			scrollViewer1.GridColumn = 1;
			scrollViewer1.GridRow = 2;
			scrollViewer1.Content = firstPanel;

			var textBox2 = new TextBox();
			textBox2.Text = "Player";
			textBox2.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
			textBox2.GridColumn = 3;
			textBox2.GridRow = 1;
			textBox2.Background = new SolidBrush("#00000000");

			secondPanel = new VerticalStackPanel();
			secondPanel.Background = new SolidBrush("#80562962");
			secondPanel.Id = "secondPanel";

			var scrollViewer2 = new ScrollViewer();
			scrollViewer2.GridColumn = 3;
			scrollViewer2.GridRow = 2;
			scrollViewer2.Content = secondPanel;

			closeButton = new TextButton();
			closeButton.Text = "X";
			closeButton.PressedBackground = new SolidBrush("#FF9503FF");
			closeButton.Margin = new Thickness(12, 0, 0, 0);
			closeButton.Padding = new Thickness(10, 2);
			closeButton.GridColumn = 4;
			closeButton.GridRow = 1;
			closeButton.Background = new SolidBrush("#8C201BFF");
			closeButton.OverBackground = new SolidBrush("#FE3930FF");
			closeButton.Id = "closeButton";

			
			ColumnsProportions.Add(new Proportion
			{
				Type = Myra.Graphics2D.UI.ProportionType.Part,
			});
			ColumnsProportions.Add(new Proportion
			{
				Type = Myra.Graphics2D.UI.ProportionType.Pixels,
				Value = 225,
			});
			ColumnsProportions.Add(new Proportion
			{
				Type = Myra.Graphics2D.UI.ProportionType.Pixels,
				Value = 34,
			});
			ColumnsProportions.Add(new Proportion
			{
				Type = Myra.Graphics2D.UI.ProportionType.Pixels,
				Value = 225,
			});
			ColumnsProportions.Add(new Proportion
			{
				Type = Myra.Graphics2D.UI.ProportionType.Part,
			});
			RowsProportions.Add(new Proportion
			{
				Type = Myra.Graphics2D.UI.ProportionType.Part,
			});
			RowsProportions.Add(new Proportion
			{
				Type = Myra.Graphics2D.UI.ProportionType.Pixels,
				Value = 30,
			});
			RowsProportions.Add(new Proportion
			{
				Type = Myra.Graphics2D.UI.ProportionType.Pixels,
				Value = 350,
			});
			RowsProportions.Add(new Proportion
			{
				Type = Myra.Graphics2D.UI.ProportionType.Part,
			});
			Widgets.Add(textBox1);
			Widgets.Add(scrollViewer1);
			Widgets.Add(textBox2);
			Widgets.Add(scrollViewer2);
			Widgets.Add(closeButton);
		}

		
		public VerticalStackPanel firstPanel;
		public VerticalStackPanel secondPanel;
		public TextButton closeButton;
	}
}
