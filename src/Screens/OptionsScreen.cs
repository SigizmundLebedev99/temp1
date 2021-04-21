using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace temp1.Screens
{
    class OptionsScreen : GameScreen
    {
     
        public OptionsScreen(Game game) : base(game)
        {
            CreateOptions(game);
        }

        public override void Draw(GameTime gameTime)
        {
           
        }

        public override void Update(GameTime gameTime)
        { 
            
        }

        public void CreateOptions(Game game)
        {
            
        }

        private (int, int)[] Resolutions(){
            return new (int, int)[]{
                (800,600),
                (1024,768),
                (1280, 768)
            };
        }
    }
}