using System;
using MonoGame.Extended.Sprites;

namespace temp1.Models
{
    [Flags]
    enum EffectFlags
    {
        Offensive = 1,
        Deffensive = 2,
        Healing = 4,
        Buff = 32,
        Debuff = 64
    }

    [Flags]
    enum TargetFlags
    {
        Self = 1,
        Target = 2,
        Area = 4,
        Freandly = 5
    }

    class Ability
    {
        public string Name;
        public string Description;
    }

    class Effect
    {
        public EffectFlags Type;
        public int TargetId;
        public Stats StatsChange;
    }
}