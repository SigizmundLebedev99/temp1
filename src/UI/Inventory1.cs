/* Generated by MyraPad at 05.02.2021 15:07:51 */
namespace temp1.UI
{
	partial class Inventory1
	{
		public Inventory1(GameContext context)
		{
			BuildUI();
            closeButton.Click += (s,e) => {
                context.GameState = GameState.Default;
            };
		}
	}
}