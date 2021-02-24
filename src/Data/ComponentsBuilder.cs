using System;
using System.Collections.Generic;
using System.Reflection;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Tiled;
using temp1;
using temp1.AI;
using temp1.Components;
using temp1.Data;

static class ComponentsBuilder
{
    static Dictionary<string, Type> _aiMap = new Dictionary<string, Type>{
        {"randomMovement", typeof(RandomMovement)},
        {"player", typeof(PlayerControll)}
    };

    public static void BuildComponents(Entity e, GameObjectTypeInfo obj, TiledMapObject mapObj, GameContext context)
    {
        if (obj.ai != null)
        {
            var _type = _aiMap[obj.ai.type];
            var ai = (BaseAI)Activator.CreateInstance(_type, new object[] { e.Id, context });
            e.Attach(ai);
        }

        if (obj.handler == null)
            return;
        var method = typeof(ComponentsBuilder).GetMethod(obj.handler, BindingFlags.Static | BindingFlags.NonPublic);
        method.Invoke(null, new object[] { e, obj, mapObj, context });
    }

    static void ActorHandler(Entity e, GameObjectTypeInfo type, TiledMapObject tiledMapObj, GameContext context)
    {
        var mapObj = e.Get<MapObject>();
        if (type.typeName == "player")
        {
            e.Attach(new Storage());
            context.PlayerId = e.Id;
        }
        if (type.typeName == "enemy")
        {
            mapObj.Type = GameObjectType.Enemy;
        }
        e.Attach(new ActionPoints{
            Max = 10,
            Remain = 10
        });
        e.Attach(new AllowedToAct());
    }

    static void ChestHandler(Entity e, GameObjectTypeInfo obj, TiledMapObject mapObject, GameContext context)
    {
        var mapObj = e.Get<MapObject>();
        mapObj.Type = GameObjectType.Storage;
        var storage = new Storage();
        foreach (var prop in mapObject.Properties)
        {
            var type = context.GameObjectTypes[prop.Key];
            var count = int.Parse(prop.Value);
            if (count == 0)
                continue;
            var _count = count;
            while(_count > 0){
                if(type.stackSize < _count){
                    _count -= type.stackSize;
                    storage.Content.Add(new ItemStack{
                        ItemType = type,
                        Count = type.stackSize
                    });
                }
                else{
                    storage.Content.Add(new ItemStack{
                        ItemType = type,
                        Count = _count
                    });
                    break;
                }
            }
        }
        context.MovementGrid.SetValueAt(mapObj.MapPosition.X, mapObj.MapPosition.Y, 3);
        e.Attach(storage);
    }

    static void HullHandler(Entity e, GameObjectTypeInfo type, TiledMapObject mapObject, GameContext context)
    {
        e.Attach(new Hull());
    }
}