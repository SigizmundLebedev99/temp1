using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Extended.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Tiled;
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

    class GameContext
    {
        public World World;
        public StaticGrid MovementGrid;
        public OrthographicCamera Camera;
        public UIService UI;
        public Bag<int> Actors => actorsSubscription.ActiveEntities;
        public Bag<int> MapObjects => mapObjectsSubscription.ActiveEntities;
        public GameState GameState = GameState.Peace;
        public PathFinder PathFinder;

        public Dictionary<string, GameObjectTypeInfo> GameObjectTypes;
        public TiledMap Map;

        public int PlayerId;
        public int PointedId;

        public UIState UIState => UI.State;

        Game _game;
        ContentManager _content;
        JsonContentLoader loader = new JsonContentLoader();
        EntitySubscription actorsSubscription;
        EntitySubscription mapObjectsSubscription;
        Mapper<AllowedToAct> _allowedMapper;
        Mapper<TurnOccured> _combatantMapper;
        Mapper<BaseAction> _actionMapper;

        public GameContext(Game game, OrthographicCamera camera)
        {
            _game = game;
            _content = _game.Content;
            Camera = camera;
            GameObjectTypes = new Dictionary<string, GameObjectTypeInfo>();
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
            actorsSubscription = new EntitySubscription(world.EntityManager, Aspect.All(typeof(BaseAI)).Build(world.ComponentManager));
            mapObjectsSubscription = new EntitySubscription(world.EntityManager, Aspect.All(typeof(MapObject)).Build(world.ComponentManager));
            ConfigureObstacles();
            ConfigureMapObjects();
            PathFinder = new PathFinder(this);
            UI = new UIService(_game);
            _allowedMapper = world.ComponentManager.Get<AllowedToAct>();
            _actionMapper = world.ComponentManager.Get<BaseAction>();
            _combatantMapper = world.ComponentManager.Get<TurnOccured>();
        }

        public void StartBattle()
        {
            GameState = GameState.Combat;
            for (var i = 0; i < Actors.Count; i++)
            {
                _allowedMapper.Delete(Actors[i]);
                _combatantMapper.Delete(Actors[i]);
                _actionMapper.Delete(Actors[i]);
            }
            _allowedMapper.Put(PlayerId, new AllowedToAct());
        }

        public void DropItem(ItemStack item, Vector2? from = null)
        {
            var _from = from.HasValue ? from.Value : World.GetEntity(PlayerId).Get<MapObject>().Position;
            var entity = World.CreateEntity();
            entity.Attach(item);
            var random = new Random();
            var x = random.Next(-15, 15) * 4;
            var y = random.Next(-25, 25);
            var mo = new MapObject(_from, GameObjectType.Item);
            entity.Attach(mo);
            entity.Attach(new Cursor("hand"));
            entity.Attach<Expired>(new Movement(_from, _from + new Vector2(x, y), mo, 1f / 16f)
            {
                OnCompleted = () => entity.Detach<IOriginMove>()
            });
            var sprite = GetSprite(item.ItemType);
            entity.Attach<IOriginMove>(new JumpOriginMove(sprite));
            sprite.Depth = 0;
            entity.Attach(sprite);
        }

        public Sprite GetSprite(GameObjectTypeInfo type)
        {
            var texture = _content.Load<Texture2D>(type.path);
            var sprite =
                type.region == null ?
                    new Sprite(texture)
                    : new Sprite(new TextureRegion2D(texture, type.region.Rectangle));
            if (type.origin != null)
                sprite.Origin = new Vector2(type.origin.x, type.origin.y);
            return sprite;
        }

        public AnimatedSprite GetAnimatedSprite(string name)
        {
            var ss = _content.Load<SpriteSheet>(name, loader);
            var sprite = new AnimatedSprite(ss);
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
            if (position != null)
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
                    var tile = obstacles.GetTile(x, y);
                    searchGrid.SetValueAt(x, y, tile.IsBlank ? (sbyte)0 : (sbyte)-1);
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