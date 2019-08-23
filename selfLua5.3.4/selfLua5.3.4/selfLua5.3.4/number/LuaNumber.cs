using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class LuaNumber
{
    public static bool isInteger(double f)
    {
        return f == (long)f;
    }

    // TODO
    public static bool parseInteger(String str, ref long ret)
    {
        //         try
        //         {
        //             return long.Parse(str);
        //         }
        //         catch (System.Exception e)
        //         {
        //             return 0;
        //         }
        return long.TryParse(str, out ret);
    }

    // TODO
    public static bool parseFloat(String str, ref double ret)
    {
        //         try
        //         {
        //             return Double.Parse(str);
        //         }
        //         catch (System.Exception e)
        //         {
        //             return 0;
        //         }
        return double.TryParse(str, out ret);
    }
}

