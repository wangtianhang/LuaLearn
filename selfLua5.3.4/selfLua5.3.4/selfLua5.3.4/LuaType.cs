using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* basic types */
public enum LuaType
{

    LUA_TNIL,
    LUA_TBOOLEAN,
    LUA_TLIGHTUSERDATA,
    LUA_TNUMBER,
    LUA_TSTRING,
    LUA_TTABLE,
    LUA_TFUNCTION,
    LUA_TUSERDATA,
    LUA_TTHREAD,
    LUA_TNONE // -1

}

class LuaValue
{

    public static LuaType typeOf(Object val)
    {
        if (val == null)
        {
            return LuaType.LUA_TNIL;
        }
        else if (val is Boolean) {
            return LuaType.LUA_TBOOLEAN;
        } else if (val is long || val is Double) {
            return LuaType.LUA_TNUMBER;
        } else if (val is String) {
            return LuaType.LUA_TSTRING;
        } else {
            throw new System.Exception("TODO");
        }
    }

    public static bool toBoolean(Object val)
    {
        if (val == null)
        {
            return false;
        }
        else if (val is Boolean) {
            return (Boolean)val;
        } else {
            return true;
        }
    }

}
