using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Program
{
    static void Main(string[] args)
    {
        IntPtr L = LuaDLL.lua_open();  /* create state */
        if (L == IntPtr.Zero)
        {
            Console.WriteLine("cannot create state: not enough memory");
            return;
        }
        LuaDLL.luaL_openlibs(L);  /* open libraries */
        RegisterHelperLib(L);
        LuaDLL.luaL_dofile(L, args[1]);
        LuaDLL.lua_close(L);
        return;
    }
}

