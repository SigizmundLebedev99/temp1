using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Extended.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using temp1.AI;
using temp1.Components;
using temp1.Data;
using temp1.PathFinding;
using temp1.UI;
using temp1.Util;

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
        public static int PlayerId;
        public static int PointedId;

        public static WorldContext World;
        public static GameObjectsContext GameObjectsContext;
        public static HudContext Hud;
        public static MapContext Map;
        public static Game Game;
        
        static SpriteBatch _sb;
        static TiledMapRenderer _tiledMapRenderer;

        public static void Init(Game game, ContentManager Content)
        {
            Game = game;
            _sb = new SpriteBatch(game.GraphicsDevice);

            World = new WorldContext();
            GameObjectsContext = new GameObjectsContext(Content);
            Hud = new HudContext(game);
            Map = new MapContext(Content, GameObjectsContext);

            World.ConfigureWorld(Map, Hud, _sb, GameObjectsContext, Content);
            GameObjectsContext.Initialize(World);
            Hud.Default();
            Map.LoadMap("tiled/map");

            _tiledMapRenderer = Map.GetRenderer(game.GraphicsDevice);

            Camera = new OrthographicCamera(game.GraphicsDevice);
        }

        public static void Update(GameTime gameTime)
        {
            if(!Game.IsActive)
                return;
            World.World.Update(gameTime);
            _tiledMapRenderer.Update(gameTime);
            Hud.Update(gameTime);
            if(Hud.State != HUDState.Default)
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
            if (state.Y > v.Height&& camera.Position.Y < map.HeightInPixels - v.Height)
                camera.Move(new Vector2(0, 5));
        }

        public static void Draw(GameTime gameTime)
        {
            var matrix = GameContext.Camera.GetViewMatrix();
            _sb.Begin(SpriteSortMode.BackToFront, transformMatrix: matrix);
            _tiledMapRenderer.Draw(matrix);
            World.World.Draw(gameTime);
            _sb.End();

            Hud.Draw(gameTime);
        }
    }
}