using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled.Renderers;
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
            _world.RegisterSystem(new TurnBasedCombatSystem(_context));
            _world.RegisterSystem(new WalkActionSystem(_context));
            _world.RegisterSystem(new OpenStorageActionSystem(_context));
            _world.RegisterSystem(new PeakItemActionSystem());
            _world.RegisterSystem(new BaseActionSystem(_context));
            _world.RegisterSystem(new PossibleMovementBuildSystem(_context));
            _world.RegisterSystem(new CursorSystem(_sb, _context));
            _world.RegisterSystem(new AISystem(_context.MovementGrid));
            _world.RegisterSystem(new TransparensySystem(_context));
            _world.RegisterSystem(new MoveOriginSystem());
            _world.RegisterSystem(new ExpirationSystem());
            _world.RegisterSystem(new DirectionToAnimationSystem());
            _world.RegisterSystem(new AnimationRenderSystem(_sb));
            _world.RegisterSystem(new StaticSpriteRenderSystem(_sb));
            _world.RegisterSystem(new PossibleMovementDrawSystem(_sb, Content));
            _world.RegisterSystem(new SpawnSystem(_context));
        }
        
        public override void Update(GameTime gameTime)
        {
            if(!Game.IsActive)
                return;
            _world.Update(gameTime);
            _tiledMapRenderer.Update(gameTime);
            if(_context.HudState != HudState.Default)
                return;
            var state = Mouse.GetState();
            var v = GraphicsDevice.Viewport;
            var map = _context.Map;
            if (!this.Game.IsActive)
                return;
            if (state.X <= 0 && camera.Position.X > 0)
                camera.Move(new Vector2(-5, 0));
            if (state.Y <= 0 && camera.Position.Y > 0)
                camera.Move(new Vector2(0, -5));
            if (state.X > v.Width && camera.Position.X < map.WidthInPixels - v.Width)
                camera.Move(new Vector2(5, 0));
            if (state.Y > v.Height&& camera.Position.Y < map.HeightInPixels - v.Height)
                camera.Move(new Vector2(0, 5));
        }

        public override void Draw(GameTime gameTime)
        {
            var matrix = camera.GetViewMatrix();
            _sb.Begin(SpriteSortMode.BackToFront, transformMatrix: matrix);
            _tiledMapRenderer.Draw(matrix);
            _world.Draw(gameTime);
            _sb.End();

            _context.Hud.Draw();
        }
    }
}