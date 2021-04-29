using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Tiled;
using temp1.AI;
using temp1.Components;
using temp1.Data;

namespace temp1
{
    class GameObjectsContext
    {
        private JsonContentLoader loader = new JsonContentLoader();
        private ContentManager _content;
        private WorldContext _world;

        private Dictionary<string, GameObjectTypeInfo> GameObjectTypes;
        private Dictionary<string, Action<Entity, GameObjectTypeInfo, TiledMapObject>> Handlers;

        public GameObjectsContext(ContentManager content)
        {
            _content = content;
            GameObjectTypes = new Dictionary<string, GameObjectTypeInfo>();
        }

        public void Initialize(WorldContext world, string gameObjectTypesPath = "game-object-types.json")
        {
            _world = world;
            var mapTypes = _content.Load<GameObjectTypeInfo[]>(gameObjectTypesPath, loader);
            GameObjectTypes = mapTypes.ToDictionary(e => e.TypeName);
            Handlers = new Dictionary<string, Action<Entity, GameObjectTypeInfo, TiledMapObject>>
            {
                {"ActorHandler", ActorHandler},
                {"ChestHandler", ChestHandler},
                {"HullHandler", HullHandler},
            };
        }

        public int CreateMapObject(string type, Vector2? position = null, TiledMapObject tiledMapObj = null)
        {
            if (!GameObjectTypes.TryGetValue(type, out var objType))
                throw new ArgumentException("Invalid type: " + type);

            var entity = _world.CreateEntity();

            if (position.HasValue)
                entity.Attach(new MapObject(position.Value, GameObjectType.None));

            if (objType.Path.EndsWith(".sf"))
            {
                var sprite = _content.GetAnimatedSprite(objType.Path);
                if (objType.Origin != null)
                    sprite.Origin = new Vector2(objType.Origin.X, objType.Origin.Y);
                entity.Attach(sprite);
            }
            else
            {
                var sprite = _content.GetSprite(objType);
                entity.Attach(sprite);
            }

            if (objType.Handler == null || !Handlers.TryGetValue(objType.Handler, out var handler))
                return entity.Id;

            handler(entity, objType, tiledMapObj);

            return entity.Id;
        }

        void ActorHandler(Entity e, GameObjectTypeInfo type, TiledMapObject tiledMapObj)
        {
            var mapObj = e.Get<MapObject>();
            if (type.TypeName == "player")
            {
                e.Attach(new Storage());
                GameContext.PlayerId = e.Id;
                e.Attach<BaseAI>(new PlayerControll(e.Id));
            }
            if (type.TypeName == "enemy")
            {
                mapObj.Type = GameObjectType.Enemy;
                e.Attach(new Cursor("sword"));
                e.Attach<BaseAI>(new RandomMovement(e.Id));
            }
            e.Attach(new ActionPoints
            {
                Max = 10,
                Remain = 10
            });
            e.Attach(new AllowedToAct());
        }

        void ChestHandler(Entity e, GameObjectTypeInfo obj, TiledMapObject mapObject)
        {
            var mapObj = e.Get<MapObject>();
            mapObj.Type = GameObjectType.Storage;
            var storage = new Storage();

            foreach (var prop in mapObject.Properties)
            {
                var type = GameObjectTypes[prop.Key];
                var countInStack = int.Parse(prop.Value);
                if (countInStack == 0)
                    continue;

                while (countInStack > 0)
                {
                    if (type.StackSize < countInStack)
                    {
                        countInStack -= type.StackSize;
                        storage.Content.Add(new ItemStack
                        {
                            ItemType = type,
                            Count = type.StackSize
                        });
                    }
                    else
                    {
                        storage.Content.Add(new ItemStack
                        {
                            ItemType = type,
                            Count = countInStack,
                        });
                        break;
                    }
                }
            }
            //context.MovementGrid.SetValueAt(mapObj.MapPosition.X, mapObj.MapPosition.Y, 3);
            e.Attach(storage);
            e.Attach(new Cursor("hand"));
        }

        void HullHandler(Entity e, GameObjectTypeInfo type, TiledMapObject mapObject)
        {
            e.Attach(new Hull());
        }
    }
}