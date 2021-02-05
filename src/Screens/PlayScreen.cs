using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled.Renderers;
using Myra.Graphics2D.UI;
using temp1.Systems;
using temp1.UI;

namespace temp1.Screens
{
    class PlayScreen : GameScreen
    {
        SpriteBatch _sb;
        TiledMapRenderer _tiledMapRenderer;
        World _world;
        GameContext _context;
        OrthographicCamera camera;

        Desktop inventory2;
        Desktop inventory1;

        public PlayScreen(Game game) : base(game)
        {
            _sb = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Initialize()
        {
            camera = new OrthographicCamera(GraphicsDevice);
            _world = new World();
            _context = new GameContext(Content, camera);
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice);
        }

        public override void LoadContent()
        {
            inventory2 = new Desktop();
            inventory2.Root = new Inventory2(_context);
            _context.Inventory2 = (Inventory2)inventory2.Root;

            inventory1 = new Desktop();
            inventory1.Root = new Inventory1(_context);
            _context.Inventory1 = (Inventory1)inventory1.Root;

            _context.LoadTypes();
            _context.LoadMap("tiled/map", _world);
            _tiledMapRenderer.LoadMap(_context.Map);
            ConfigureWorld();
        }

        private void ConfigureWorld()
        {
            _world.RegisterSystem(new PointerSystem(_sb, _context));
            _world.RegisterSystem(new AISystem(_context.CollisionGrid));
            _world.RegisterSystem(new MoveSystem());
            _world.RegisterSystem(new TransparensySystem(_context));
            _world.RegisterSystem(new ExpirationSystem());
            _world.RegisterSystem(new DirectionSystem());
            _world.RegisterSystem(new DirectionToAnimationSystem());
            _world.RegisterSystem(new AnimationRenderSystem(_sb));
            _world.RegisterSystem(new StaticSpriteRenderSystem(_sb));
            _world.RegisterSystem(new SpawnSystem(_context));
        }
        
        public override void Update(GameTime gameTime)
        {
            if(!Game.IsActive)
                return;
            _world.Update(gameTime);
            _tiledMapRenderer.Update(gameTime);
            if(_context.GameState != GameState.Default)
                return;
            var state = Mouse.GetState();
            if (!this.Game.IsActive)
                return;
            if (state.X <= 0)
                camera.Move(new Vector2(-5, 0));
            if (state.Y <= 0 && camera.Position.Y >= 0)
                camera.Move(new Vector2(0, -5));
            if (state.X > this.GraphicsDevice.Viewport.Width)
                camera.Move(new Vector2(5, 0));
            if (state.Y > this.GraphicsDevice.Viewport.Height)
                camera.Move(new Vector2(0, 5));
            if(state.RightButton == ButtonState.Pressed)
                _context.Inventory1.Open();
        }

        public override void Draw(GameTime gameTime)
        {
            var matrix = camera.GetViewMatrix();
            _sb.Begin(SpriteSortMode.BackToFront, transformMatrix: matrix);
            _tiledMapRenderer.Draw(matrix);
            _world.Draw(gameTime);
            _sb.End();

            if(_context.GameState == GameState.Inventry2Opened)
                inventory2.Render();
            if(_context.GameState == GameState.Inventry1Opened)
                inventory1.Render();
        }
    }
}