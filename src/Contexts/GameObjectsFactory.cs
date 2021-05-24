using System;
using System.Collections.Generic;
using System.Linq;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using temp1.AI;
using temp1.Components;
using temp1.Data;

namespace temp1
{
    class GameObjectsFactory
    {
        private JsonContentLoader loader = new JsonContentLoader();
        private ContentManager _content;
        private World _world;

        private Dictionary<string, GameObjectTypeInfo> GameObjectTypes;
        private Dictionary<string, Action<Entity, GameObjectTypeInfo, TiledMapObject>> Handlers;

        public GameObjectsFactory(ContentManager content)
        {
            _content = content;
            GameObjectTypes = new Dictionary<string, GameObjectTypeInfo>();
        }

        public void Initialize(World world, string gameObjectTypesPath = "game-object-types.json")
        {
            _world = world;
            var mapTypes = _content.Load<GameObjectTypeInfo[]>(gameObjectTypesPath, loader);
            GameObjectTypes = mapTypes.ToDictionary(e => e.TypeName);
            Handlers = new Dictionary<string, Action<Entity, GameObjectTypeInfo, TiledMapObject>>
            {
                {"ActorHandler", ActorHandler},
                {"ChestHandler", ChestHandler}
            };
        }

        public Entity CreateMapObject(string type, Vector2? position = null, TiledMapObject tiledMapObj = null)
        {
            if (!GameObjectTypes.TryGetValue(type, out var objType))
                throw new ArgumentException("Invalid type: " + type);

            var entity = _world.CreateEntity();

            if (position.HasValue)
                entity.Set(new MapObject(position.Value, GameObjectType.None));

            Sprite sprite;

            if (objType.Sprite != null)
            {
                var spriteInfo = objType.Sprite;
                if (spriteInfo.Path.EndsWith(".sf"))
                {
                    sprite = _content.GetAnimatedSprite(spriteInfo.Path);
                    if (spriteInfo.Origin != null)
                        sprite.Origin = new Vector2(spriteInfo.Origin.X, spriteInfo.Origin.Y);
                }
                else
                    sprite = _content.GetSprite(objType);


                entity.Set(new RenderingObject(sprite));
            }

            if (objType.Handler == null || !Handlers.TryGetValue(objType.Handler, out var handler))
                return entity;

            handler(entity, objType, tiledMapObj);

            return entity;
        }

        void ActorHandler(Entity e, GameObjectTypeInfo type, TiledMapObject tiledMapObj)
        {
            var mapObj = e.Get<MapObject>();
            if (type.TypeName == "player")
            {
                e.Set(new Storage());
                GameContext.Player = e;
                mapObj.Type = GameObjectType.Blocking;
                e.Set<IBaseAI>(new PlayerControll());
            }
            if (type.TypeName == "enemy")
            {
                mapObj.Type = GameObjectType.Enemy | GameObjectType.Blocking;
                e.Set(new Cursor("sword"));
                e.Set<IBaseAI>(new RandomMovement());
            }

            e.Set(new ActionPoints
            {
                Max = 10,
                Remain = 10
            });
            
            e.Set(new AllowedToAct());
        }

        void ChestHandler(Entity e, GameObjectTypeInfo obj, TiledMapObject mapObject)
        {
            var mapObj = e.Get<MapObject>();
            mapObj.Type = GameObjectType.Storage | GameObjectType.Blocking;
            var storage = new Storage();

            foreach (var prop in mapObject.Properties.Where(e => e.Key.StartsWith('~')))
            {
                var type = GameObjectTypes[prop.Key.Substring(1)];
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

            e.Set(storage);
            e.Set(new Cursor("hand"));
        }
    }
}