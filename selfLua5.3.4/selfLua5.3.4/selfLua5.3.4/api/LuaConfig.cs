using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class LuaConfig
{
    public static int LUA_MINSTACK = 20;
    public static int LUAI_MAXSTACK = 1000000;
    public static int LUA_REGISTRYINDEX = -LUAI_MAXSTACK - 1000;
    public static long LUA_RIDX_GLOBALS = 2;

    public const string NULL_ALIAS = "null";
}

