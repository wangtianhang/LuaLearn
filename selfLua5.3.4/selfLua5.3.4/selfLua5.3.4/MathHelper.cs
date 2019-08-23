using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class MathHelper
{
    /// <summary>
    /// 无符号右移，与JS中的>>>等价
    /// </summary>
    /// <param name="x">要移位的数</param>
    /// <param name="y">移位数</param>
    /// <returns></returns>
    public static int UIntMoveRight(int x, int y)
    {
        int mask = 0x7fffffff; //Integer.MAX_VALUE
        for (int i = 0; i < y; i++)
        {
            x >>= 1;
            x &= mask;
        }
        return x;
    }

    public static long ULongMoveRight(long x, int y)
    {
        long mask = long.MaxValue; //Integer.MAX_VALUE
        for (int i = 0; i < y; i++)
        {
            x >>= 1;
            x &= mask;
        }
        return x;
    }

    public static long floorDiv(long x, long y)
    {
        long r = x / y;
        // if the signs are different and modulo not zero, round down
        if ((x ^ y) < 0 && (r * y != x))
        {
            r--;
        }
        return r;
    }

    public static long floorMod(long x, long y)
    {
        long r = x - floorDiv(x, y) * y;
        return r;
    }
}

