using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class LuaConfig
{
    static int LUA_MINSTACK = 20;
    static int LUAI_MAXSTACK = 1000000;
    static int LUA_REGISTRYINDEX = -LUAI_MAXSTACK - 1000;
    static long LUA_RIDX_GLOBALS = 2;
}

