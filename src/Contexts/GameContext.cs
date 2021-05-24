using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled.Renderers;
using temp1.UI;

namespace temp1
{
    enum GameState
    {
        Peace,
        Combat,
    }

    static class GameContext
    {
        public static GameState GameState = GameState.Peace;
        public static OrthographicCamera Camera;
        public static Entity Player;
        public static Entity PointedEntity;

        public static World World;
        public static GameObjectsContext GameObjects;
        public static HudContext Hud;
        public static MapContext Map;
        public static CombatContext Combat;
        public static Game1 Game;

        public static ContentManager Content => Game.Content;

        static SpriteBatch _sb;

        public static void Init(Game1 game, ContentManager Content)
        {
            Game = game;

            Camera = new OrthographicCamera(game.GraphicsDevice);
            
            _sb = game.Batch;

            World = new World();

            GameObjects = new GameObjectsContext(Content);

            Map = new MapContext(Content, GameObjects, game.GraphicsDevice);

            //World.ConfigureWorld(Map, _sb, GameObjects, Content);

            GameObjects.Initialize(World);

            Hud = new HudContext(game);
            Hud.Default();

            Combat = new CombatContext(World);

            Map.LoadMap("tiled/map");
        }

        public static void Update(GameTime gameTime)
        {
            if (!Game.IsActive)
                return;
            World.Update(gameTime);
            Map.Update(gameTime);
            Hud.Update(gameTime);
            if (Hud.State != HUDState.Default)
                return;
            var state = Mouse.GetState();
            var v = Game.GraphicsDevice.Viewport;
            var map = Map.Map;

            var camera = GameContext.Camera;

            if (state.X <= 0 && camera.Position.X > 0)
                camera.Move(new Vector2(-5, 0));
            if (state.Y <= 0 && camera.Position.Y > 0)
                camera.Move(new Vector2(0, -5));
            if (state.X > v.Width && camera.Position.X < map.WidthInPixels - v.Width)
                camera.Move(new Vector2(5, 0));
            if (state.Y > v.Height && camera.Position.Y < map.HeightInPixels - v.Height)
                camera.Move(new Vector2(0, 5));
        }

        public static void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            var matrix = GameContext.Camera.GetViewMatrix();
            Map.Draw(matrix);

            _sb.Begin(SpriteSortMode.BackToFront, transformMatrix: matrix);
            World.World.Draw(gameTime);
            _sb.End();
            
            Hud.Draw(gameTime);
        }
    }
}