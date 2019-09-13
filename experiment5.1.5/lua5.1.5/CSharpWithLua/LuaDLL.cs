using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


class LuaDLL
{
    public const string LUADLL = "lua5.1.5";
    public static int LUA_MULTRET = -1;

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

//     public static void luaL_register(IntPtr luaState, string name, LuaCSFunction func)
//     {
//         lua_pushcfunction(luaState, func);
//         lua_setglobal(luaState, name);
//     }
}

