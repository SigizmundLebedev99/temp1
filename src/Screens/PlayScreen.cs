using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
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
        AnimatedSprite sprite;
        World world;
        Entity player;
        private OrthographicCamera camera;

        public PlayScreen(Game game) : base(game)
        {
            _sb = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void LoadContent()
        {
            camera = new OrthographicCamera(GraphicsDevice);
            _map = Content.Load<TiledMap>("map");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _map);
            var spriteSheet = Content.Load<SpriteSheet>("motw.sf", new JsonContentLoader());
            sprite = new AnimatedSprite(spriteSheet);
            sprite.Play("idle");
            world = new WorldBuilder()
                .AddSystem(new PlayerControlSystem(camera))
                .AddSystem(new MoveSystem())
                .AddSystem(new AnimationRenderSystem(_sb))
                .Build();
            player = world.CreateEntity();
            player.Attach(sprite);
            player.Attach(new Player());
            player.Attach(new AllowedToAct());
            player.Attach<Box>(new Box
            {
                SelectionBounds = new Point(sprite.TextureRegion.Width, sprite.TextureRegion.Height),
                Position = new Vector2(50, 50)
            });
        }

        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);
            state = Mouse.GetState();
            if (state.X <= 0 && camera.Position.X >= -(_map.WidthInPixels / 2))
                camera.Move(new Vector2(-5, 0));
            if (state.Y <= 0 && camera.Position.Y >= 0)
                camera.Move(new Vector2(0, -5));
            if (state.X > this.GraphicsDevice.Viewport.Width && camera.Position.X < _map.WidthInPixels / 2 - this.GraphicsDevice.Viewport.Width)
                camera.Move(new Vector2(5, 0));
            if (state.Y > this.GraphicsDevice.Viewport.Height && camera.Position.Y < _map.HeightInPixels - this.GraphicsDevice.Viewport.Height)
                camera.Move(new Vector2(0, 5));
        }

        public override void Draw(GameTime gameTime)
        {
            var matrix = camera.GetViewMatrix();
            _sb.Begin(transformMatrix: matrix);
            _tiledMapRenderer.Draw(matrix);
            world.Draw(gameTime);
            _sb.End();
        }
    }
}