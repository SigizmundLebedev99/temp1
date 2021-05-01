using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace temp1.Screens
{
    class PlayScreen : GameScreen
    {

        public PlayScreen(Game game) : base(game)
        {
        }
        
        public override void Update(GameTime gameTime)
        {
            GameContext.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameContext.Draw(gameTime);
        }
    }
}