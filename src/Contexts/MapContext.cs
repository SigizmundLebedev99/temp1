using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using temp1.Components;
using temp1.PathFinding;

namespace temp1
{
    class MapContext
    {
        private TiledMap _map;
        private GameObjectsFactory _goContext;
        private GraphicsDevice _device;
        private TiledMapRenderer _mapRenderer;

        public string MapName { get; private set; }
        public PathFinder PathFinder { get; private set; }
        public StaticGrid MovementGrid { get; private set; }

        public TiledMap Map { get => _map; }

        private List<Texture2D> Images { get; set; }

        public MapContext(GameObjectsFactory goContext, GraphicsDevice device)
        {
            _goContext = goContext;
            _device = device;
        }

        public void LoadMap(string mapName)
        {
            MapName = mapName;
            _map = GameContext.Content.Load<TiledMap>(mapName);
            _mapRenderer = ConfigureRenderer();
            ConfigureObstacles();
            ConfigureMapObjects();
            ProcessLayers(_map);

            PathFinder = new PathFinder(this);
        }

        public TiledMapRenderer ConfigureRenderer()
        {
            if (_map == null)
                throw new InvalidOperationException("Map not loaded");
            var renderer = new TiledMapRenderer(_device);
            renderer.LoadMap(_map);
            return renderer;
        }

        public void Update(GameTime gameTime)
        {
            _mapRenderer.Update(gameTime);
        }

        public void Draw(Matrix matrix)
        {
            _mapRenderer.Draw(matrix);
        }

        void ConfigureObstacles()
        {
            var searchGrid = new StaticGrid(_map.Width, _map.Height);
            var obstacles = _map.GetLayer<TiledMapTileLayer>("obstacles");
            obstacles.IsVisible = false;
            for (ushort x = 0; x < _map.Width; x++)
            {
                for (ushort y = 0; y < _map.Height; y++)
                {
                    var tile = obstacles.GetTile(x, y);
                    searchGrid.SetValueAt(x, y, tile.IsBlank ? (sbyte)0 : (sbyte)-1);
                }
            }

            MovementGrid = searchGrid;
        }

        void ProcessLayers(TiledMap map)
        {
            Images = new List<Texture2D>(32);

            void ParseLayers(IEnumerable<TiledMapLayer> layers)
            {
                foreach (var layer in layers)
                {
                    if (layer is TiledMapGroupLayer group)
                    {
                        group.IsVisible = false;
                        ParseLayers(group.Layers);
                        continue;
                    }

                    if (!(layer is TiledMapTileLayer) || !layer.Name.StartsWith("_b_"))
                        continue;

                    var tiled = (TiledMapTileLayer)layer;
                    tiled.IsVisible = true;
                    var rect = TrimBlankTiles(tiled);
                    var cover = GameContext.World.CreateEntity();

                    var renderTarget = new RenderTarget2D(_device, rect.Width, rect.Height);
                    _device.SetRenderTarget(renderTarget);
                    _device.Clear(Color.Transparent);
                    _mapRenderer.Draw(tiled, Matrix.CreateTranslation(-rect.X, -rect.Y, 0));

                    Images.Add(renderTarget);

                    var sprite = new Sprite(renderTarget);
                    sprite.Origin = new Vector2(-rect.X, -rect.Y);

                    sprite.Depth = 0.1f / ((rect.Y / 32) + (rect.Height / 32) - 1);

                    cover.Set(new RenderingObject(sprite, ""));
                    tiled.IsVisible = false;

                    if (tiled.Properties.TryGetValue("Persistent", out var persistent) && bool.Parse(persistent))
                        continue;

                    var canopy = new Canopy(tiled);
                    if (tiled.Properties.TryGetValue("Interier", out var interier))
                        canopy.IsInterier = bool.Parse(interier);
                    cover.Set(canopy);
                }
            }
            ParseLayers(map.Layers);
            _device.SetRenderTarget(null);
        }

        void ConfigureMapObjects()
        {
            var mapObjects = _map.GetLayer<TiledMapObjectLayer>("markers").Objects;
            foreach (var mapObj in mapObjects)
            {
                _goContext.CreateMapObject(mapObj.Type, mapObj.Position, mapObj);
            }
        }

        Rectangle TrimBlankTiles(TiledMapTileLayer layer)
        {
            int maxX = 0, maxY = 0;
            int minX = int.MaxValue, minY = int.MaxValue;

            for (ushort i = 0; i < _map.Width; i++)
            {
                for (ushort j = 0; j < _map.Height; j++)
                {
                    var tile = layer.GetTile(i, j);
                    if (tile.IsBlank)
                        continue;
                    maxX = i > maxX ? i : maxX;
                    maxY = j > maxY ? j : maxY;
                    minX = i < minX ? i : minX;
                    minY = j < minY ? j : minY;
                }
            }

            return new Rectangle(minX * _map.TileWidth, minY * _map.TileHeight, (maxX - minX + 1) * _map.TileWidth, (maxY - minY + 1) * _map.TileHeight);
        }

        public void Dispose()
        {
            _mapRenderer.Dispose();
            foreach (var image in Images)
            {
                image.Dispose();
            }
        }
    }
}