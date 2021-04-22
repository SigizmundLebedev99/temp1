using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using temp1.PathFinding;
using temp1.Util;

namespace temp1
{
    class MapContext
    {
        private TiledMap map;
        private ContentManager _content;
        private GameObjectsContext _goContext;

        public PathFinder PathFinder { get; private set; }
        public StaticGrid MovementGrid { get; private set; }

        public TiledMap Map { get => map; }

        public MapContext(ContentManager content, GameObjectsContext goContext)
        {
            _content = content;
            _goContext = goContext;
        }

        public void LoadMap(string mapName)
        {
            map = _content.Load<TiledMap>(mapName);
            ConfigureObstacles();
            ConfigureMapObjects();
            PathFinder = new PathFinder(this);
        }

        public TiledMapRenderer GetRenderer(GraphicsDevice device)
        {
            if (map == null)
                throw new InvalidOperationException("Map not loaded");
            var renderer = new TiledMapRenderer(device);
            renderer.LoadMap(map);
            return renderer;
        }

        void ConfigureObstacles()
        {
            var searchGrid = new StaticGrid(map.Width, map.Height);
            var obstacles = map.GetLayer<TiledMapTileLayer>("obstacles");
            for (ushort x = 0; x < map.Width; x++)
            {
                for (ushort y = 0; y < map.Height; y++)
                {
                    var tile = obstacles.GetTile(x, y);
                    searchGrid.SetValueAt(x, y, tile.IsBlank ? (sbyte)0 : (sbyte)-1);
                }
            }

            MovementGrid = searchGrid;
        }

        void ConfigureMapObjects()
        {
            var mapObjects = map.GetLayer<TiledMapObjectLayer>("markers").Objects;
            foreach (var mapObj in mapObjects)
            {
                _goContext.CreateMapObject(mapObj.Type, mapObj.Position, mapObj);
            }
        }
    }
}