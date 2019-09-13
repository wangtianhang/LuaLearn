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
        string fullPath = Path.GetFullPath(args[0]);
        bool ret = LuaDLL.luaL_dofile(L, fullPath);
        if(!ret)
        {
            Console.WriteLine("执行luaL_dofile出错");
        }
        LuaDLL.lua_close(L);

        Console.WriteLine("测试结束");
        Console.ReadLine();
        return;
    }
}

