using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Tiled;
using temp1;
using temp1.AI;
using temp1.Components;
using temp1.Data;

static class ComponentsMap
{
    static Dictionary<string, Type> _aiMap = new Dictionary<string, Type>{
        {"randomMovement", typeof(RandomMovement)},
        {"player", typeof(PlayerControll)}
    };

    public static void BuildComponents(Entity e, MapObjectType obj, TiledMapObject mapObj, GameContext context)
    {
        if (obj.ai != null)
        {
            var _type = _aiMap[obj.ai.type];
            var ai = (BaseAI)Activator.CreateInstance(_type, new object[] { e.Id, context });
            e.Attach(ai);
        }

        if (obj.handler == null)
            return;
        var method = typeof(ComponentsMap).GetMethod(obj.handler, BindingFlags.Static | BindingFlags.NonPublic);
        method.Invoke(null, new object[] { e, obj, mapObj, context });
    }

    static void ActorHandler(Entity e, MapObjectType type, TiledMapObject mapObject, GameContext context)
    {
        if (type.type == "player")
        {
            e.Attach(new Player());
            context.PlayerId = e.Id;
        }
        if (type.type == "enemy")
        {
            e.Attach(new Enemy());
        }
        e.Attach(new AllowedToAct());
        e.Attach(new Direction());
    }

    static void ChestHandler(Entity e, MapObjectType obj, TiledMapObject mapObject, GameContext context)
    {
        var storage = new Storage();
        foreach (var prop in mapObject.Properties)
        {
            if (int.Parse(prop.Value) == 0)
                continue;
            storage.Content.Add(new FilledSlot{
                ItemType = context.ItemTypes[prop.Key],
                count = int.Parse(prop.Value)
            });
        }
    }

    static void HullHandler(Entity e, MapObjectType type, TiledMapObject mapObject, GameContext context)
    {
        e.Attach(new Hull());
    }
}