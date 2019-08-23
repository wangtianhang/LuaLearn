using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class PrintStackData
{
    public static void printStack(LuaState ls)
    {
        int top = ls.getTop();
        for (int i = 1; i <= top; i++)
        {
            LuaType t = ls.type(i);
            switch (t)
            {
                case LuaType.LUA_TBOOLEAN:
                    Console.Write("[{0}]", ls.toBoolean(i));
                    break;
                case LuaType.LUA_TNUMBER:
                    if (ls.isInteger(i))
                    {
                        Console.Write("[{0}]", ls.toInteger(i));
                    }
                    else
                    {
                        Console.Write("[{0}]", ls.toNumber(i));
                    }
                    break;
                case LuaType.LUA_TSTRING:
                    Console.Write("[\"{0}\"]", ls.toString(i));
                    break;
                default: // other values
                    Console.Write("[{0}]", ls.typeName(t));
                    break;
            }
        }
        //System.out.println();
        Console.WriteLine();
    }
}

