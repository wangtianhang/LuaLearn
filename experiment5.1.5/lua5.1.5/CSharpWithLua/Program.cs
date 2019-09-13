using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

class Program
{
    static int luaPrint(IntPtr L)
    {
        Console.WriteLine("luaPrint");
        try
        {
            int n = LuaDLL.lua_gettop(L);

            return 0;
        }
        catch(System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine(Directory.GetCurrentDirectory());

        IntPtr L = LuaDLL.lua_open();  /* create state */
        if (L == IntPtr.Zero)
        {
            Console.WriteLine("cannot create state: not enough memory");
            return;
        }
        LuaDLL.luaL_openlibs(L);  /* open libraries */
        //RegisterHelperLib(L);
        LuaDLL.lua_register(L, "print2", luaPrint);
        LuaDLL.luaL_dofile(L, args[0]);
        LuaDLL.lua_close(L);

        Console.ReadLine();
        return;
    }
}

