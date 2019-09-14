using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


public class LuaIndexes
{
    public static int LUA_REGISTRYINDEX = -10000;
    public static int LUA_ENVIRONINDEX = -10001;
    public static int LUA_GLOBALSINDEX = -10002;
}

public enum LuaTypes
{
    LUA_TNONE = -1,
    LUA_TNIL = 0,
    LUA_TBOOLEAN = 1,
    LUA_TLIGHTUSERDATA = 2,
    LUA_TNUMBER = 3,
    LUA_TSTRING = 4,
    LUA_TTABLE = 5,
    LUA_TFUNCTION = 6,
    LUA_TUSERDATA = 7,
    LUA_TTHREAD = 8,

}

class LuaDLL
{
    public const string LUADLL = "lua5.1.5.dll";
    public static int LUA_MULTRET = -1;
    public static string[] LuaTypeName = { "none", "nil", "boolean", "lightuserdata", "number", "string", "table", "function", "userdata", "thread" };

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr luaL_newstate();

    public static IntPtr lua_open()
    {
        return luaL_newstate();
    }

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void lua_close(IntPtr luaState);

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void luaL_openlibs(IntPtr luaState);

    public static bool luaL_dofile(IntPtr luaState, string fileName)                                              //[-0, +1, e]
    {
        int result = luaL_loadfile(luaState, fileName);

        if (result != 0)
        {
            return false;
        }

        return LuaDLL.lua_pcall(luaState, 0, LUA_MULTRET, 0) == 0;
    }

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int luaL_loadfile(IntPtr luaState, string filename);

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int lua_pcall(IntPtr luaState, int nArgs, int nResults, int errfunc);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int LuaCSFunction(IntPtr luaState);

    public static void lua_register(IntPtr luaState, string name, LuaCSFunction func)
    {
        lua_pushcfunction(luaState, func);
        lua_setglobal(luaState, name);
    }

    public static void lua_setglobal(IntPtr luaState, string name)
    {
        lua_setfield(luaState, LuaIndexes.LUA_GLOBALSINDEX, name);
    }

    //     public static void lua_setfield(IntPtr L, int idx, string key)
    //     {
    //         if (tolua_setfield(L, idx, key) != 0)
    //         {
    //             string error = LuaDLL.lua_tostring(L, -1);
    //             throw new System.Exception(error);
    //         }
    //     }
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void lua_setfield(IntPtr L, int idx, string key);

    public static void lua_pushcfunction(IntPtr luaState, LuaCSFunction func)
    {
        IntPtr fn = Marshal.GetFunctionPointerForDelegate(func);
        lua_pushcclosure(luaState, fn, 0);
    }

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void lua_pushcclosure(IntPtr luaState, IntPtr fn, int n);

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int lua_gettop(IntPtr luaState);

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int lua_isstring(IntPtr luaState, int index);

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr lua_tolstring(IntPtr luaState, int index, out int strLen);

    public static string lua_ptrtostring(IntPtr str, int len)
    {
        string ss = Marshal.PtrToStringAnsi(str, len);

        if (ss == null)
        {
            byte[] buffer = new byte[len];
            Marshal.Copy(str, buffer, 0, len);
            return Encoding.UTF8.GetString(buffer);
        }

        return ss;
    }

    public static string lua_tostring(IntPtr luaState, int index)
    {
        int len = 0;
        IntPtr str = lua_tolstring(luaState, index, out len);
        if(str != IntPtr.Zero)
        {
            return lua_ptrtostring(str, len);
        }
        return null;
    }

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern LuaTypes lua_type(IntPtr luaState, int index);

    public static bool lua_isnil(IntPtr luaState, int n)
    {
        return (lua_type(luaState, n) == LuaTypes.LUA_TNIL);
    }

    public static bool lua_isboolean(IntPtr luaState, int n)
    {
        LuaTypes type = lua_type(luaState, n);
        return type == LuaTypes.LUA_TBOOLEAN || type == LuaTypes.LUA_TNIL;
    }

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int lua_toboolean(IntPtr luaState, int index);

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr lua_topointer(IntPtr L, int idx);

    public static string luaL_typename(IntPtr luaState, int stackPos)
    {
        LuaTypes type = LuaDLL.lua_type(luaState, stackPos);
        return lua_typename(luaState, type);
    }

    public static string lua_typename(IntPtr luaState, LuaTypes type)
    {
        int t = (int)type;
        return LuaTypeName[t + 1];
    }

    public static bool lua_istable(IntPtr luaState, int n)
    {
        return lua_type(luaState, n) == LuaTypes.LUA_TTABLE;
    }

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void lua_pushnil(IntPtr luaState);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void lua_settop(IntPtr luaState, int top);

    public static void lua_pop(IntPtr luaState, int amount)
    {
        LuaDLL.lua_settop(luaState, -(amount) - 1);
    }

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int lua_next(IntPtr luaState, int index);

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void lua_getfield(IntPtr L, int idx, string key);

    public static void lua_getglobal(IntPtr luaState, string name)
    {
        lua_getfield(luaState, LuaIndexes.LUA_GLOBALSINDEX, name);
    }

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int luaL_ref(IntPtr L, int t);

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void lua_rawgeti(IntPtr luaState, int idx, int n);
}

