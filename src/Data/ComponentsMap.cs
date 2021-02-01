using System;
using System.Collections.Generic;
using System.Linq;
using temp1.Components;

static class ComponentsMap{
    static List<(string, Type)> _map = new List<(string, Type)>{
        ("player", typeof(Player)),
        ("actor", typeof(AllowedToAct)),
        ("actor", typeof(Direction)),
        ("enemy", typeof(Enemy)),
        ("chest", typeof(Storage))
    };
    
    static ComponentsMap()
    {
        Map = _map.ToLookup(e => e.Item1, e => e.Item2);    
    }

    public static ILookup<string, Type> Map;
}