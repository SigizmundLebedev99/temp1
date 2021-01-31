using System.Linq;
using EpPathFinding.cs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using temp1.Components;
using temp1.Systems;

namespace temp1.Screens
{
    class PlayScreen : GameScreen
    {
        private MouseState state;
        private SpriteBatch _sb;
        TiledMap _map;
        TiledMapRenderer _tiledMapRenderer;
        World world;
        Entity player;
        BaseGrid _searchGrid;
        Polygon[] _obstacles;
        private OrthographicCamera camera;

        public PlayScreen(Game game) : base(game)
        {
            _sb = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void LoadContent()
        {
            camera = new OrthographicCamera(GraphicsDevice);
            _map = Content.Load<TiledMap>("tiled/map");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _map);
            ContentStorage.Load(Content);
            ConfigureObstacles();
            CreateWorld();
            CreatePlayer();
        }

        private void CreateWorld()
        {
            world = new WorldBuilder()
                .AddSystem(new PlayerControlSystem(camera, _searchGrid))
                .AddSystem(new AISystem(_searchGrid))
                .AddSystem(new MoveSystem())
                .AddSystem(new ExpirationSystem())
                .AddSystem(new DirectionSystem())
                .AddSystem(new DirectionToAnimationSystem())
                .AddSystem(new AnimationRenderSystem(_sb))
                .AddSystem(new SpawnSystem(_searchGrid, null))
                .Build();
        }

        private void CreatePlayer()
        {
            var pos = _map.GetLayer<TiledMapObjectLayer>("markers").Objects.First(e => e.Type == "player");
            player = world.CreateEntity();
            var sprite = ContentStorage.Player;
            player.Attach(sprite);
            player.Attach(new Player());
            player.Attach(new AllowedToAct());
            player.Attach(new Direction());
            player.Attach<Box>(new Box
            {
                SelectionBounds = new Point(sprite.TextureRegion.Width, sprite.TextureRegion.Height),
                Position = pos.Position
            });
        }

        void ConfigureObstacles()
        {
            _searchGrid = new StaticGrid(_map.Width, _map.Height);
            var obstacles = _map.GetLayer<TiledMapObjectLayer>("obstacles").Objects.Select(e =>
                new Polygon((e as TiledMapPolygonObject).Points.Select(p => new Vector2(p.X, p.Y) + e.Position)))
                .ToArray();
            _obstacles = obstacles;
            for (var x = 0; x < _map.Width; x++)
            {
                for (var y = 0; y < _map.Height; y++)
                {
                    var isIn = false;
                    for (var i = 0; i < obstacles.Length; i++){
                        if(obstacles[i].Contains(x * _map.TileWidth + _map.TileWidth / 2, y * _map.TileHeight + _map.TileHeight / 2)){
                            isIn = true;
                            break;
                        }
                    }
                    _searchGrid.SetWalkableAt(x,y,!isIn);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);
            state = Mouse.GetState();
            if (!this.Game.IsActive)
                return;
            if (state.X <= 0 && camera.Position.X >= -(_map.WidthInPixels / 2))
                camera.Move(new Vector2(-5, 0));
            if (state.Y <= 0 && camera.Position.Y >= 0)
                camera.Move(new Vector2(0, -5));
            if (state.X > this.GraphicsDevice.Viewport.Width && camera.Position.X < this.GraphicsDevice.Viewport.Width - _map.WidthInPixels)
                camera.Move(new Vector2(5, 0));
            if (state.Y > this.GraphicsDevice.Viewport.Height && camera.Position.Y < _map.HeightInPixels - this.GraphicsDevice.Viewport.Height)
                camera.Move(new Vector2(0, 5));
        }

        public override void Draw(GameTime gameTime)
        {
            var matrix = camera.GetViewMatrix();
            _sb.Begin(SpriteSortMode.BackToFront, transformMatrix: matrix);
            _tiledMapRenderer.Draw(matrix);
            world.Draw(gameTime);
            _sb.End();
        }
    }
}