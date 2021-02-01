using System;
using System.Collections.Generic;
using System.Linq;
using EpPathFinding.cs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using temp1.Components;
using temp1.Data;

namespace temp1
{
    class GameContext
    {
        public World _world;
        public TiledMap Map;
        public BaseGrid Grid;

        public int PlayerId;

        ContentManager _content;
        Dictionary<string, SpriteSheet> SpriteSheets;
        Dictionary<string, Sprite> Sprites;
        Dictionary<string, MapObjectType> MapObjectTypes;
        JsonContentLoader loader = new JsonContentLoader();

        public GameContext(ContentManager content)
        {
            _content = content;
            MapObjectTypes = new Dictionary<string, MapObjectType>();
            Sprites = new Dictionary<string, Sprite>();
            SpriteSheets = new Dictionary<string, SpriteSheet>();
        }

        public void LoadTypes()
        {
            var types = _content.Load<MapObjectType[]>("typeMap.json", loader);
            MapObjectTypes = types.ToDictionary(e => e.type);
        }

        public void LoadMap(string map, World world)
        {
            _world = world;
            Map = _content.Load<TiledMap>(map);
            ConfigureMapObjects();
            ConfigureObstacles();
        }

        public Sprite GetSprite(string name)
        {
            if (!Sprites.ContainsKey(name))
                Sprites[name] = new Sprite(_content.Load<Texture2D>(name));
            return Sprites[name];
        }

        public AnimatedSprite GetAnimatedSprite(string name)
        {
            if (!SpriteSheets.ContainsKey(name))
                SpriteSheets[name] = _content.Load<SpriteSheet>(name, loader);
            var sprite = new AnimatedSprite(SpriteSheets[name]);
            sprite.Play("idle");
            return sprite;
        }

        void ConfigureObstacles()
        {
            var searchGrid = new StaticGrid(Map.Width, Map.Height);
            var obstacles = Map.GetLayer<TiledMapObjectLayer>("obstacles").Objects.Select(e =>
                new Polygon((e as TiledMapPolygonObject).Points.Select(p => new Vector2(p.X, p.Y) + e.Position)))
                .ToArray();
            for (var x = 0; x < Map.Width; x++)
            {
                for (var y = 0; y < Map.Height; y++)
                {
                    var isIn = false;
                    for (var i = 0; i < obstacles.Length; i++)
                    {
                        if (obstacles[i].Contains(x * Map.TileWidth + Map.TileWidth / 2, y * Map.TileHeight + Map.TileHeight / 2))
                        {
                            isIn = true;
                            break;
                        }
                    }
                    searchGrid.SetWalkableAt(x, y, !isIn);
                }
            }

            Grid = searchGrid;
        }

        void ConfigureMapObjects()
        {
            var mapObjects = Map.GetLayer<TiledMapObjectLayer>("markers").Objects;
            foreach (var obj in mapObjects)
            {
                CreateEntity(obj.Type, obj.Position);
            }
        }

        public int CreateEntity(string type, Vector2? position = null)
        {
            var obj = MapObjectTypes[type];
            var entity = _world.CreateEntity();
            if (obj.animated)
            {
                var sprite = GetAnimatedSprite(obj.path);
                if (obj.origin != null)
                    sprite.Origin = new Vector2(obj.origin.x, obj.origin.y);
                entity.Attach(sprite);
            }
            else
            {
                var sprite = GetSprite(obj.path);
                if (obj.origin != null)
                    sprite.Origin = new Vector2(obj.origin.x, obj.origin.y);
                entity.Attach(sprite);
            }
            if (position.HasValue)
                entity.Attach(new Dot(position.Value));
            if (obj.components != null && obj.components.Length > 0)
            {
                var attachMethod = typeof(Entity).GetMethod(nameof(entity.Attach));
                foreach (var flag in obj.components)
                {
                    foreach (var _type in ComponentsMap.Map[flag])
                    {
                        var comp = Activator.CreateInstance(_type);
                        attachMethod.MakeGenericMethod(_type).Invoke(entity, new object[] { comp });
                    }
                }
            }
            if(type == "player")
                PlayerId = entity.Id;
            return entity.Id;
        }
    }
}