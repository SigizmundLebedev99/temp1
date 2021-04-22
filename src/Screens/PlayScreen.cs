using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled.Renderers;
using temp1.UI;

namespace temp1.Screens
{
    class PlayScreen : GameScreen
    {

        public PlayScreen(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            
        }

        public override void LoadContent()
        {
            GameContext.Init(Game, Content);
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