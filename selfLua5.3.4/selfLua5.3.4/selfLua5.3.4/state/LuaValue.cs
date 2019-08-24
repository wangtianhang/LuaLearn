using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class LuaValue
{

    public static LuaType typeOf(Object val)
    {
        Type type = null;
        if (val != null)
        {
            type = val.GetType();
        }

        if (val == null)
        {
            return LuaType.LUA_TNIL;
        }
        else if (val is Boolean)
        {
            return LuaType.LUA_TBOOLEAN;
        }
        else if (val is long || val is UInt64 || val is Double)
        {
            return LuaType.LUA_TNUMBER;
        }
        else if (val is String)
        {
            return LuaType.LUA_TSTRING;
        }
        else if(val is LuaTable)
        {
            return LuaType.LUA_TTABLE;
        }
        else
        {
            throw new System.Exception("TODO");
        }
    }

    public static bool toBoolean(Object val)
    {
        if (val == null)
        {
            return false;
        }
        else if (val is Boolean)
        {
            return (Boolean)val;
        }
        else
        {
            return true;
        }
    }

    public static bool toInteger(Object val, ref long ret)
    {
        if (val is long)
        {
            ret = (long)val;
            return true;
        }
        else if (val is Double)
        {
            double n = (Double)val;
            if(LuaNumber.isInteger(n))
            {
                ret = (long)n;
                return true;
            }
            return false;
        }
        else if (val is String)
        {
            return toInteger((String)val, ref ret);
        }
        else
        {
            return false;
        }
    }

    public static bool toFloat(Object val, ref double ret)
    {
        if (val is Double)
        {
            ret = (Double)val;
            return true;
        }
        else if (val is long)
        {
            ret = (double)((long)val);
            return true;
        }
        else if (val is String)
        {
            return LuaNumber.parseFloat((String)val, ref ret);
        }
        else
        {
            return false;
        }
    }

    private static bool toInteger(String s, ref long ret)
    {
        //long i = 0;
        if(LuaNumber.parseInteger(s, ref ret))
        {
            return true;
        }
        //         if (i != null)
        //         {
        //             return i;
        //         }
        //Double f = LuaNumber.parseFloat(s);
        double f = 0;
        if(LuaNumber.parseFloat(s, ref f)
            && LuaNumber.isInteger(f))
        {
            ret = (long)f;
            return true;
        }
//         if (f != null && LuaNumber.isInteger(f))
//         {
//             return f.longValue();
//         }
        return false;
    }
}
