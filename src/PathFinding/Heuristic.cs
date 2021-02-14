using System;
using System.Collections.Generic;
using System.Collections;

namespace temp1.PathFinding
{
    public class Heuristic
    {
        public static float Manhattan(int iDx, int iDy)
        {
            return (float)iDx + iDy;
        }

        public static float Euclidean(int iDx, int iDy)
        {
            float tFdx = (float)iDx;
            float tFdy = (float)iDy;
            return (float)Math.Sqrt((double)(tFdx * tFdx + tFdy * tFdy));
        }

        public static float Chebyshev(int iDx, int iDy)
        {
            return (float)Math.Max(iDx, iDy);
        }
    }
}
