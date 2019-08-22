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
}

