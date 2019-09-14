using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class LuaFunction
{
    protected int reference = -1;
    protected LuaState luaState;

    public LuaFunction(int reference, LuaState state)
    {
        this.reference = reference;
        this.luaState = state;
    }

    public void Call()
    {
        //BeginPCall();
        //PCall();
        //EndPCall();
        LuaDLL.lua_rawgeti(luaState.L, LuaIndexes.LUA_REGISTRYINDEX, reference);
        LuaDLL.lua_pcall(luaState.L, 0, 0, 0);
    }
}

