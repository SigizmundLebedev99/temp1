using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Entities;
using temp1;
using temp1.AI;
using temp1.Components;
using temp1.Data;

static class ComponentsMap{
    static ILookup<string, Type> _map = new []{
        ("player", typeof(Player)),
        ("actor", typeof(AllowedToAct)),
        ("actor", typeof(Direction)),
        ("enemy", typeof(Enemy)),
        ("chest", typeof(Storage)),
        ("hull", typeof(Hull))
    }.ToLookup(e => e.Item1, e => e.Item2);

    static Dictionary<string, Type> _aiMap = new Dictionary<string, Type>{
        {"randomMovement", typeof(RandomMovement)}
    };

    public static void BuildComponents(Entity e, MapObjectType obj, GameContext context){
        if (obj.components != null && obj.components.Length > 0)
            {
                var attachMethod = typeof(Entity).GetMethod(nameof(e.Attach));
                foreach (var flag in obj.components)
                {
                    foreach (var _type in _map[flag])
                    {
                        var comp = Activator.CreateInstance(_type);
                        attachMethod.MakeGenericMethod(_type).Invoke(e, new object[] { comp });
                    }
                }
            }
            if (obj.ai != null)
            {
                var _type = _aiMap[obj.ai.type];
                var ai = (BaseAI)Activator.CreateInstance(_type, new object[] { e.Id, context });
                e.Attach(ai);
            }
    }
}