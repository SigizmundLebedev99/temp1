using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using temp1.Factories;
using temp1.Screens;

namespace temp1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;

        public SpriteBatch Batch => _spriteBatch;
        public GraphicsDeviceManager GDManager => _graphics;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "assets";
            IsMouseVisible = true;
            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
            
            var controlsFactory = new ControlsFactory(Content);
            Services.AddService(controlsFactory);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _screenManager.LoadScreen(new MenuScreen(this));
            base.Initialize();
        }

        protected override void LoadContent()
        {
        }
    }
}
