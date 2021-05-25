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
using temp1.Models;

namespace temp1
{
    class GameObjectsFactory
    {
        private JsonContentLoader loader = new JsonContentLoader();
        private ContentManager _content;
        private World _world;

        private Dictionary<string, GameObjectTypeInfo> GameObjectTypes;
        private Dictionary<string, Action<Entity, GameObjectTypeInfo, TiledMapObject>> Handlers;

        public World World { set { if (value != null) _world = value; } }

        public GameObjectsFactory(ContentManager content)
        {
            _content = content;
            GameObjectTypes = new Dictionary<string, GameObjectTypeInfo>();
        }

        public void Initialize(string gameObjectTypesPath = "game-object-types.json")
        {
            var mapTypes = _content.Load<GameObjectTypeInfo[]>(gameObjectTypesPath, loader);
            GameObjectTypes = mapTypes.ToDictionary(e => e.TypeName);
            Handlers = new Dictionary<string, Action<Entity, GameObjectTypeInfo, TiledMapObject>>
            {
                {"ActorHandler", ActorHandler},
                {"ChestHandler", ChestHandler},
                {"PortalHandler",PortalHandler}
            };
        }

        public Entity CreateMapObject(string type, Vector2? position = null, TiledMapObject tiledMapObj = null)
        {
            if (!GameObjectTypes.TryGetValue(type, out var objType))
                throw new ArgumentException("Invalid type: " + type);

            var entity = _world.CreateEntity();

            if (position.HasValue)
            {
                var x = position.Value.X - (position.Value.X % 32) + 16;
                var y = position.Value.Y - (position.Value.Y % 32) + 16;
                entity.Set(new Position(new Vector2(x, y)));
            }

            if (objType.Sprite != null)
            {
                entity.Set(new RenderingObject(_content.GetSprite(objType.Sprite), objType.Sprite.Path));
            }

            if (objType.Handler == null || !Handlers.TryGetValue(objType.Handler, out var handler))
                return entity;

            handler(entity, objType, tiledMapObj);

            return entity;
        }

        void ActorHandler(Entity e, GameObjectTypeInfo type, TiledMapObject tiledMapObj)
        {
            if (type.TypeName == "player")
            {
                e.Set(new Storage());
                GameContext.Player = e;
                e.Set<IGameAI>(new PlayerControl());
            }
            if (type.TypeName == "enemy")
            {
                var sprite = e.Get<RenderingObject>();
                e.Set(new Cursor("sword", new Rectangle((int)sprite.Origin.X, (int)sprite.Origin.Y, sprite.Bounds.Width, sprite.Bounds.Height)));
                e.Set(GameObjectType.Enemy);
                e.Set<IGameAI>(new RandomMovement());
            }

            e.Set(new ActionPoints
            {
                Max = 10,
                Remain = 10
            });
            e.Set(new Serializable());
            e.Set(new AllowedToAct());
        }

        void ChestHandler(Entity e, GameObjectTypeInfo obj, TiledMapObject tiled)
        {
            e.Set(GameObjectType.Storage);
            e.Set(new Blocking());
            var storage = new Storage();

            foreach (var prop in tiled.Properties.Where(e => e.Key.StartsWith('~')))
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
            e.Set(new Serializable());
            e.Set(new Cursor("hand", new Rectangle(-16, -16, 32, 32)));
        }

        void PortalHandler(Entity e, GameObjectTypeInfo obj, TiledMapObject tiled)
        {
            e.Set(new Trigger
            {
                Invoke = (action, entity) =>
                {
                    var destination = tiled.Properties["destination"];
                    GameContext.LoadMap(destination);
                }
            });
            e.Set(new Cursor("hand", new Rectangle(-16, -16, 32, 32)));
        }
    }
}