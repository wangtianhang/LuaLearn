using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class BaseLib
{
    public static int print(LuaState ls)
    {
        int nArgs = ls.getTop();
        for (int i = 1; i <= nArgs; i++)
        {
            if (ls.isBoolean(i))
            {
                //System.out.print(ls.toBoolean(i));
                Console.Write(ls.toBoolean(i));
            }
            else if (ls.isString(i))
            {
                //System.out.print(ls.toString(i));
                Console.Write(ls.toString(i));
            }
            else
            {
                //System.out.print(ls.typeName(ls.type(i)));
                Console.Write(ls.typeName(ls.type(i)));
            }
            if (i < nArgs)
            {
                //System.out.print("\t");
                Console.Write("\t");
            }
        }
        //System.out.println();
        Console.WriteLine();
        return 0;
    }

    public static int getMetatable(LuaState ls)
    {
        if (!ls.getMetatable(1))
        {
            ls.pushNil();
        }
        return 1;
    }

    public static int setMetatable(LuaState ls)
    {
        ls.setMetatable(1);
        return 1;
    }

    public static int next(LuaState ls)
    {
        ls.setTop(2); /* create a 2nd argument if there isn't one */
        if (ls.next(1))
        {
            return 2;
        }
        else
        {
            ls.pushNil();
            return 1;
        }
    }

    public static int pairs(LuaState ls)
    {
        ls.pushCSharpFunction(next); /* will return generator, */
        ls.pushValue(1);                 /* state, */
        ls.pushNil();
        return 3;
    }

    public static int iPairs(LuaState ls)
    {
        ls.pushCSharpFunction(iPairsAux); /* iteration function */
        ls.pushValue(1);                      /* state */
        ls.pushInteger(0);                    /* initial value */
        return 3;
    }

    public static int iPairsAux(LuaState ls)
    {
        long i = ls.toInteger(2) + 1;
        ls.pushInteger(i);
        return ls.getI(1, i) == LuaType.LUA_TNIL ? 1 : 2;
    }

    public static int error(LuaState ls)
    {
        return ls.error();
    }

    public static int pCall(LuaState ls)
    {
        int nArgs = ls.getTop() - 1;
        ThreadStatus status = ls.pCall(nArgs, -1, 0);
        ls.pushBoolean(status == ThreadStatus.LUA_OK);
        ls.insert(1);
        return ls.getTop();
    }
}

