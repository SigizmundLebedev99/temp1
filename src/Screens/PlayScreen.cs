using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled.Renderers;
using temp1.Systems;

namespace temp1.Screens
{
    class PlayScreen : GameScreen
    {
        private SpriteBatch _sb;
        TiledMapRenderer _tiledMapRenderer;
        World _world;
        GameContext _context;
        private OrthographicCamera camera;

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
            _context.LoadTypes();
            _context.LoadMap("tiled/map", _world);
            _tiledMapRenderer.LoadMap(_context.Map);
            ConfigureWorld();
        }

        private void ConfigureWorld()
        {
            _world.RegisterSystem(new PointerSystem(_sb, _context, camera));
            _world.RegisterSystem(new AISystem(_context.Grid));
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
        }

        public override void Draw(GameTime gameTime)
        {
            var matrix = camera.GetViewMatrix();
            _sb.Begin(SpriteSortMode.BackToFront, transformMatrix: matrix);
            _tiledMapRenderer.Draw(matrix);
            _world.Draw(gameTime);
            _sb.End();
        }
    }
}