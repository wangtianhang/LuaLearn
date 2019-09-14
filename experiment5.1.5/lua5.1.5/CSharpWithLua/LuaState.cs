using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class LuaState
{
    public IntPtr L;

    public LuaState(IntPtr L)
    {
        this.L = L;
    }

    public LuaFunction GetFunction(string name)
    {
        int oldTop = LuaDLL.lua_gettop(L);
        int pos = name.LastIndexOf('.');

        if (pos > 0)
        {
            throw new System.Exception("还没支持table中的function");
        }
        else
        {
            LuaDLL.lua_getglobal(L, name);
            LuaTypes type = LuaDLL.lua_type(L, -1);
            if (type != LuaTypes.LUA_TFUNCTION)
            {
                // 还原堆栈
                LuaDLL.lua_settop(L, oldTop);
                return null;
            }

            // 可以找到的情况
            int reference = LuaDLL.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
            return new LuaFunction(reference, this);
        }
        return null;
    }
}

