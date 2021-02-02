using System;
using System.Collections.Generic;
using System.Linq;
using temp1.AI;
using temp1.Components;

static class ComponentsMap{
    static List<(string, Type)> _map = new List<(string, Type)>{
        ("player", typeof(Player)),
        ("actor", typeof(AllowedToAct)),
        ("actor", typeof(Direction)),
        ("enemy", typeof(Enemy)),
        ("chest", typeof(Storage))
    };

    static List<(string, Type)> _aiMap = new List<(string, Type)>{
        ("randomMovement", typeof(RandomMovement))
    };
    
    static ComponentsMap()
    {
        Component_Map = _map.ToLookup(e => e.Item1, e => e.Item2);
        AI_Map = _aiMap.ToDictionary(e => e.Item1, e => e.Item2);    
    }

    public static ILookup<string, Type> Component_Map;
    public static Dictionary<string, Type> AI_Map;
}