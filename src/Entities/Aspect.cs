using System;
using System.Collections.Specialized;

namespace MonoGame.Extended.Entities
{
    public class Aspect
    {
        internal Aspect()
        {
            AllSet = 0;
            ExclusionSet = 0;
            OneSet = 0;
        }

        public long AllSet;
        public long ExclusionSet;
        public long OneSet;

        public static AspectBuilder All(params Type[] types)
        {
            return new AspectBuilder().All(types);
        }

        public static AspectBuilder One(params Type[] types)
        {
            return new AspectBuilder().One(types);
        }

        public static AspectBuilder Exclude(params Type[] types)
        {
            return new AspectBuilder().Exclude(types);
        }

        public bool IsInterested(long componentBits)
        {
            if (AllSet != 0 && (componentBits & AllSet) != AllSet)
                return false;

            if (ExclusionSet != 0 && (componentBits & ExclusionSet) != 0)
                return false;

            if (OneSet != 0 && (componentBits & OneSet) == 0)
                return false;

            return true;
        }
    }
}