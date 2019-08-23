using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class LuaValue
{

    public static LuaType typeOf(Object val)
    {
        if (val == null)
        {
            return LuaType.LUA_TNIL;
        }
        else if (val is Boolean)
        {
            return LuaType.LUA_TBOOLEAN;
        }
        else if (val is long || val is Double)
        {
            return LuaType.LUA_TNUMBER;
        }
        else if (val is String)
        {
            return LuaType.LUA_TSTRING;
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
        if(val is long)
        {
            ret = (long)val;
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool toFloat(Object val, ref double ret)
    {
        if (val is double)
        {
            ret = (double)val;
            return true;
        }
        else
        {
            return false;
        }
    }
}
