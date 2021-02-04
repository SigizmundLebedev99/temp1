/* Generated by MyraPad at 04.02.2021 13:01:17 */
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using temp1.Screens;

namespace temp1.UI
{
	public partial class MainMenu
	{
		public MainMenu(ScreenManager screenManager, Game game)
		{
			BuildUI();
            _menuStartNewGame.Click += (s,e) => screenManager.LoadScreen(new PlayScreen(game));
            _menuQuit.Click += (s,e) => game.Exit();
		}
	}
}