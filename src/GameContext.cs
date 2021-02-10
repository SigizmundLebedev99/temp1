using System;
using System.Collections.Generic;
using System.Linq;
using EpPathFinding.cs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Tiled;
using temp1.Components;
using temp1.Data;
using temp1.UI;

namespace temp1
{
    class GameContext
    {
        public World World;
        public TiledMap Map;
        public BaseGrid MovementGrid;
        public OrthographicCamera Camera;
        public HudService Hud;
        public Dictionary<string, GameObjectTypeInfo> GameObjectTypes;
        
        public Inventory2 Inventory2;
        public Inventory1 Inventory1;

        public int PlayerId;
        public int PointedId;

        public HudState HudState => Hud.HudState;

        ContentManager _content;
        Dictionary<string, SpriteSheet> SpriteSheets;
        Dictionary<string, Sprite> Sprites;
        JsonContentLoader loader = new JsonContentLoader();
        

        public GameContext(ContentManager content, OrthographicCamera camera)
        {
            _content = content;
            Camera = camera;
            GameObjectTypes = new Dictionary<string, GameObjectTypeInfo>();
            Sprites = new Dictionary<string, Sprite>();
            SpriteSheets = new Dictionary<string, SpriteSheet>();
        }

        public void LoadTypes()
        {
            var mapTypes = _content.Load<GameObjectTypeInfo[]>("game-object-types.json", loader);
            GameObjectTypes = mapTypes.ToDictionary(e => e.typeName);
        }

        public void LoadMap(string map, World world)
        {
            World = world;
            Map = _content.Load<TiledMap>(map);
            ConfigureObstacles();
            ConfigureMapObjects();
            Hud = new HudService(_content, this);
        }

        public void DropItem(ItemStack item, Vector2? from = null){
            var _from = from.HasValue?from.Value:World.GetEntity(PlayerId).Get<MapObject>().Position;
            var entity = World.CreateEntity();
            entity.Attach(item);
            var random = new Random();
            var x = random.Next(-50, 50) * 2;
            var y = random.Next(-25, 0);
            entity.Attach(new MapObject(_from, GameObjectType.Item));
            entity.Attach<IMovement>(new FallMovement(_from, _from + new Vector2(x,y)));
            var sprite = GetSprite(item.ItemType);
            sprite.Depth = 0;
            entity.Attach(sprite);
        }

        public Sprite GetSprite(GameObjectTypeInfo type){
            if(!Sprites.ContainsKey(type.typeName)){
                var texture = _content.Load<Texture2D>(type.path);
                var sprite = 
                    type.region == null ? 
                        new Sprite(texture) 
                        : new Sprite(new TextureRegion2D(texture, type.region.Rectangle));
                if (type.origin != null)
                    sprite.Origin = new Vector2(type.origin.x, type.origin.y);
                Sprites[type.typeName] = sprite;
            }

            return Sprites[type.typeName];
        }

        public AnimatedSprite GetAnimatedSprite(string name)
        {
            if (!SpriteSheets.ContainsKey(name))
                SpriteSheets[name] = _content.Load<SpriteSheet>(name, loader);
            var sprite = new AnimatedSprite(SpriteSheets[name]);
            sprite.Play("idle");
            return sprite;
        }

        public int CreateEntity(string type, Vector2? position = null, TiledMapObject tiledMapObj = null)
        {
            var objType = GameObjectTypes[type];
            var entity = World.CreateEntity();
            if (objType.path.EndsWith(".sf"))
            {
                var sprite = GetAnimatedSprite(objType.path);
                if (objType.origin != null)
                    sprite.Origin = new Vector2(objType.origin.x, objType.origin.y);
                entity.Attach(sprite);
            }
            else
            {
                var sprite = GetSprite(objType);
                entity.Attach(sprite);
            }
            if (position!= null)
                entity.Attach(new MapObject(position.Value, GameObjectType.None));

            ComponentsBuilder.BuildComponents(entity, objType, tiledMapObj, this);

            return entity.Id;
        }

        void ConfigureObstacles()
        {
            var searchGrid = new StaticGrid(Map.Width, Map.Height);
           var obstacles = Map.GetLayer<TiledMapTileLayer>("obstacles");
            for (ushort x = 0; x < Map.Width; x++)
            {
                for (ushort y = 0; y < Map.Height; y++)
                {
                    var tile = obstacles.GetTile(x,y);
                    searchGrid.SetWalkableAt(x, y, tile.IsBlank);
                }
            }

            MovementGrid = searchGrid;
        }

        void ConfigureMapObjects()
        {
            var mapObjects = Map.GetLayer<TiledMapObjectLayer>("markers").Objects;
            foreach (var mapObj in mapObjects)
            {
                CreateEntity(mapObj.Type, mapObj.Position, mapObj);
            }
        }
    }
}