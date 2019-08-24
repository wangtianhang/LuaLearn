using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class LuaMath
{
    public static double floorDiv(double a, double b)
    {
        return Math.Floor(a / b);
    }

    // a % b == a - ((a // b) * b)
    public static double floorMod(double a, double b)
    {
        if (a > 0 && b == Double.MaxValue
                || a < 0 && b == Double.MinValue)
        {
            return a;
        }
        if (a > 0 && b == Double.MinValue
                || a < 0 && b == Double.MaxValue)
        {
            return b;
        }
        return a - Math.Floor(a / b) * b;
    }

    public static long shiftLeft(long a, int n)
    {
        return n >= 0 ? a << n : JavaHelper.ULongMoveRight(a, -n);
    }

    public static long shiftRight(long a, int n)
    {
        return n >= 0 ? JavaHelper.ULongMoveRight(a, n) : a << -n;
    }
}

