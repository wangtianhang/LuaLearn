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
        //Console.WriteLine("luaPrint");
        try
        {
            int n = LuaDLL.lua_gettop(L);
            //Console.WriteLine("n " + n);

            StringBuilder sb = new StringBuilder();
            for(int i = 1; i <= n; ++i)
            {
                if (i > 1) sb.Append("    ");

                if(LuaDLL.lua_isstring(L, i) == 1)
                {
                    sb.Append(LuaDLL.lua_tostring(L, i));
                }
                else if(LuaDLL.lua_isnil(L, i))
                {
                    sb.Append("nil");
                }
                else if (LuaDLL.lua_isboolean(L, i))
                {
                    sb.Append(LuaDLL.lua_toboolean(L, i) != 0 ? "true" : "false");
                }
                else
                {
                    //Console.WriteLine("暂不支持的Print类型");
                    IntPtr p = LuaDLL.lua_topointer(L, i);
                    if (p == IntPtr.Zero)
                    {
                        sb.Append("nil");
                    }
                    else
                    {
                        sb.Append(LuaDLL.luaL_typename(L, i)).Append(":0x").Append(p.ToString("X"));
                    }
                }
            }

            Console.WriteLine(sb.ToString());

            return 0;
        }
        catch(System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }

    static void RecurrenceLogTable(IntPtr L)
    {
        if (LuaDLL.lua_istable(L, -1))
        {
            //int tableIndex = lua_gettop(L);
            //printf("tableIndex %d \n", tableIndex);
            // push the first key
            //lua_pushnil(L);
            LuaDLL.lua_pushnil(L);
            while (LuaDLL.lua_next(L, -2) != 0)
            {
                /* 此时栈上 -1 处为 value, -2 处为 key */
                if (LuaDLL.lua_istable(L, -1))
                {
                    string keyStr = LuaDLL.lua_tostring(L, -2);
                    Console.WriteLine("子table {0} 开始", keyStr);
                    RecurrenceLogTable(L);
                    Console.WriteLine("子table {0} 结束", keyStr);
                    LuaDLL.lua_pop(L, 1);
                }
                else
                {
                    string keyStr = LuaDLL.lua_tostring(L, -2);
                    string valueStr = LuaDLL.lua_tostring(L, -1);
                    Console.WriteLine("{0} {1}", keyStr, valueStr);
                    LuaDLL.lua_pop(L, 1);
                }
            }
        }
    }

    static int LogTable(IntPtr L)
    {
        RecurrenceLogTable(L);
        return 0;
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
        LuaDLL.lua_register(L, "print", luaPrint);
        LuaDLL.lua_register(L, "LogTable2", LogTable);
        string fullPath = Path.GetFullPath(args[0]);
        bool ret = LuaDLL.luaL_dofile(L, fullPath);
        if(!ret)
        {
            Console.WriteLine("执行luaL_dofile出错");
        }
        else
        {
            Console.WriteLine("执行luaL_dofile成功");
        }
        LuaDLL.lua_close(L);

        Console.WriteLine("测试结束");
        Console.ReadLine();
        return;
    }
}

