using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LuaStateImpl : LuaState
{

    private LuaStack stack = new LuaStack();

    /* basic stack manipulation */


    public int getTop()
    {
        return stack.top();
    }


    public int absIndex(int idx)
    {
        return stack.absIndex(idx);
    }


    public bool checkStack(int n)
    {
        return true; // TODO
    }


    public void pop(int n)
    {
        for (int i = 0; i < n; i++)
        {
            stack.pop();
        }
    }


    public void copy(int fromIdx, int toIdx)
    {
        stack.set(toIdx, stack.get(fromIdx));
    }


    public void pushValue(int idx)
    {
        stack.push(stack.get(idx));
    }


    public void replace(int idx)
    {
        stack.set(idx, stack.pop());
    }


    public void insert(int idx)
    {
        rotate(idx, 1);
    }


    public void remove(int idx)
    {
        rotate(idx, -1);
        pop(1);
    }


    public void rotate(int idx, int n)
    {
        int t = stack.top() - 1;            /* end of stack segment being rotated */
        int p = stack.absIndex(idx) - 1;    /* start of segment */
        int m = n >= 0 ? t - n : p - n - 1; /* end of prefix */

        stack.reverse(p, m);     /* reverse the prefix with length 'n' */
        stack.reverse(m + 1, t); /* reverse the suffix */
        stack.reverse(p, t);     /* reverse the entire segment */
    }


    public void setTop(int idx)
    {
        int newTop = stack.absIndex(idx);
        if (newTop < 0)
        {
            throw new System.Exception("stack underflow!");
        }

        int n = stack.top() - newTop;
        if (n > 0)
        {
            for (int i = 0; i < n; i++)
            {
                stack.pop();
            }
        }
        else if (n < 0)
        {
            for (int i = 0; i > n; i--)
            {
                stack.push(null);
            }
        }
    }

    /* access functions (stack -> Go); */


    public String typeName(LuaType tp)
    {
        switch (tp)
        {
            case LuaType.LUA_TNONE: return "no value";
            case LuaType.LUA_TNIL: return "nil";
            case LuaType.LUA_TBOOLEAN: return "bool";
            case LuaType.LUA_TNUMBER: return "number";
            case LuaType.LUA_TSTRING: return "string";
            case LuaType.LUA_TTABLE: return "table";
            case LuaType.LUA_TFUNCTION: return "function";
            case LuaType.LUA_TTHREAD: return "thread";
            default: return "userdata";
        }
    }


    public LuaType type(int idx)
    {
        return stack.isValid(idx)
                ? LuaValue.typeOf(stack.get(idx))
                : LuaType.LUA_TNONE;
    }


    public bool isNone(int idx)
    {
        return type(idx) == LuaType.LUA_TNONE;
    }


    public bool isNil(int idx)
    {
        return type(idx) == LuaType.LUA_TNIL;
    }


    public bool isNoneOrNil(int idx)
    {
        LuaType t = type(idx);
        return t == LuaType.LUA_TNONE || t == LuaType.LUA_TNIL;
    }


    public bool isBoolean(int idx)
    {
        return type(idx) == LuaType.LUA_TBOOLEAN;
    }


    public bool isInteger(int idx)
    {
        return stack.get(idx) is long;
    }


    public bool isNumber(int idx)
    {
        double number = 0;
        bool ret = toNumberX(idx, ref number);
        return ret;
    }


    public bool isString(int idx)
    {
        LuaType t = type(idx);
        return t == LuaType.LUA_TSTRING || t == LuaType.LUA_TNUMBER;
    }


    public bool isTable(int idx)
    {
        return type(idx) == LuaType.LUA_TTABLE;
    }


    public bool isThread(int idx)
    {
        return type(idx) == LuaType.LUA_TTHREAD;
    }


    public bool isFunction(int idx)
    {
        return type(idx) == LuaType.LUA_TFUNCTION;
    }


    public bool toBoolean(int idx)
    {
        return LuaValue.toBoolean(stack.get(idx));
    }


    public long toInteger(int idx)
    {
        long ret = 0;
        if(toIntegerX(idx, ref ret))
        {
            return ret;
        }
        else
        {
            return 0;
        }
    }


    public bool toIntegerX(int idx, ref long ret)
    {
        Object val = stack.get(idx);
        if(val is long)
        {
            ret = (long)val;
            return true;
        }
        else
        {
            return false;
        }
    }


    public double toNumber(int idx)
    {
        Double n = 0;
        if(toNumberX(idx, ref n))
        {
            return n;
        }
        else
        {
            return 0;
        }
    }


    public bool toNumberX(int idx, ref double ret)
    {
        Object val = stack.get(idx);
        if (val is Double)
        {
            ret = (double)val;
            return true;
        }
        else if (val is long)
        {
            ret = (double)((long)val);
            return true;
        }
        else
        {
            return false;
        }
    }


    public String toString(int idx)
    {
        Object val = stack.get(idx);
        if (val is String) {
            return (String)val;
        } else if (val is long || val is Double) {
            return val.ToString();
        } else {
            return null;
        }
    }

    /* push functions (Go -> stack); */


    public void pushNil()
    {
        stack.push(null);
    }


    public void pushBoolean(bool b)
    {
        stack.push(b);
    }


    public void pushInteger(long n)
    {
        stack.push(n);
    }


    public void pushNumber(double n)
    {
        stack.push(n);
    }


    public void pushString(String s)
    {
        stack.push(s);
    }

}