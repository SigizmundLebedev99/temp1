using System;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using temp1.Components;
using temp1.Systems;
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
        public static Entity? PointedEntity;

        public static World World;
        public static GameObjectsFactory GameObjects;
        public static HudContext Hud;
        public static MapContext Map;
        public static CombatContext Combat;
        public static Game1 Game;
        public static EntitySets EntitySets;

        public static ContentManager Content => Game.Content;

        private static ISystem<GameTime> SystemsSet;
        private static bool LoadNewMap = true;
        private static Action LoadMapAction = null;

        public static void Init(Game1 game, ContentManager Content)
        {
            Game = game;
            Camera = new OrthographicCamera(game.GraphicsDevice);
            GameObjects = new GameObjectsFactory(Content);
            Combat = new CombatContext();

            GameObjects.Initialize();
            Hud = new HudContext(game);
            Hud.Default();
        }

        public static void LoadMap(string mapName, World world = null)
        {
            LoadNewMap = true;
            LoadMapAction = () => ConfigureNewMap(mapName, world);
        }

        private static void ConfigureNewMap(string mapName, World world = null)
        {

            var newWorld = world ?? new World(64);

            if (Player.IsAlive)
                Player = Player.CopyTo(newWorld);

            if (World != null && World != newWorld)
                World.Dispose();

            World = newWorld;

            EntitySets = new EntitySets(World);
            Map?.Dispose();
            Map = new MapContext(GameObjects, Game.GraphicsDevice);
            SystemsSet?.Dispose();
            SystemsSet = ConfigureSystems();
            GameObjects.World = World;
            Map.LoadMap(mapName);

            World.Subscribe<(WalkAction, Entity)>((in (WalkAction, Entity) payload) =>
            {
                var (action, entity) = payload;
                var triggers = EntitySets.Triggers.GetEntities();
                for (var i = 0; i < triggers.Length; i++)
                {
                    triggers[i].Get<Trigger>().Check(triggers[i], action, entity);
                }
            });

            if (GameContext.Player.IsAlive)
            {
                Camera.LookAt(GameContext.Player.Get<Position>().Value);
            }

            LoadNewMap = false;
        }

        public static void Update(GameTime gameTime)
        {
            if (!Game.IsActive)
                return;
            if (LoadNewMap)
                LoadMapAction?.Invoke();

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
            if (state.Y > v.Height && camera.Position.Y < map.HeightInPixels - v.Height + 100)
                camera.Move(new Vector2(0, 5));
        }

        public static void Draw(GameTime gameTime)
        {
            if (!Game.IsActive)
                return;
            if (LoadNewMap)
                LoadMapAction?.Invoke();

            Game.GraphicsDevice.Clear(Color.Black);
            var matrix = GameContext.Camera.GetViewMatrix();
            Map?.Draw(matrix);

            Game.Batch.Begin(SpriteSortMode.BackToFront, transformMatrix: matrix);
            SystemsSet.Update(gameTime);
            Game.Batch.End();

            Hud.Draw(gameTime);
        }

        private static ISystem<GameTime> ConfigureSystems()
        {
            return new SequentialSystem<GameTime>(
                new TurnBasedCombatSystem(World),
                new ActionSystem(World),
                new PossibleMovementBuildSystem(World),
                new CursorSystem(Game.Batch, World),
                new AISystem(World),
                new CanopySystem(World),
                new ExpirationSystem(World),
                new DirectionToAnimationSystem(World),
                new SpriteRenderSystem(World, Game.Batch),
                new PossibleMovementDrawSystem(World, Game.Batch)
            );
        }
    }
}