using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class LuaStack
{
    private List<Object> slots = new List<Object>();
    /* call info */
    public Closure closure;
    public List<Object> varargs;
    public int pc;
    /* linked list */
    public LuaStack prev;
    public LuaStateImpl state;
    public Dictionary<int, UpvalueHolder> openuvs;

    public int top()
    {
        return slots.Count();
    }

    public void push(Object val)
    {
        if(val is UInt64)
        {
            int test = 0;
        }
        if (slots.Count > 10000)
        { // TODO
            throw new System.Exception("lua 堆栈 过大");
        }
        slots.Add(val);
    }

    public Object pop()
    {
        Object last = slots[slots.Count - 1];
        slots.RemoveAt(slots.Count - 1);
        return last;
    }

    public void pushN(List<Object> vals, int n)
    {
        int nVals = vals == null ? 0 : vals.Count;
        if (n < 0)
        {
            n = nVals;
        }
        for (int i = 0; i < n; i++)
        {
            push(i < nVals ? vals[i] : null);
        }
    }

    public List<Object> popN(int n)
    {
        List<Object> vals = new List<Object>(n);
        for (int i = 0; i < n; i++)
        {
            vals.Add(pop());
        }
        //Collections.reverse(vals);
        vals.Reverse();
        return vals;
    }

    public int absIndex(int idx)
    {
        return idx >= 0 || idx <= LuaConfig.LUA_REGISTRYINDEX
        ? idx : idx + slots.Count + 1;
    }

    public bool isValid(int idx)
    {
        if (idx < LuaConfig.LUA_REGISTRYINDEX)
        { /* upvalues */
            int uvIdx = LuaConfig.LUA_REGISTRYINDEX - idx - 1;
            return closure != null && uvIdx < closure.upvals.Length;
        }
        if (idx == LuaConfig.LUA_REGISTRYINDEX)
        {
            return true;
        }
        int absIdx = absIndex(idx);
        return absIdx > 0 && absIdx <= slots.Count;
    }

    public Object get(int idx)
    {
        if (idx < LuaConfig.LUA_REGISTRYINDEX)
        { /* upvalues */
            int uvIdx = LuaConfig.LUA_REGISTRYINDEX - idx - 1;
            if (closure != null
                    && closure.upvals.Length > uvIdx
                    && closure.upvals[uvIdx] != null)
            {
                return closure.upvals[uvIdx].get();
            }
            else
            {
                return null;
            }
        }
        if (idx == LuaConfig.LUA_REGISTRYINDEX)
        {
            return state.registry;
        }
        int absIdx = absIndex(idx);
        if (absIdx > 0 && absIdx <= slots.Count)
        {
            return slots[absIdx - 1];
        }
        else
        {
            return null;
        }
    }

    public void set(int idx, Object val)
    {
        if (idx < LuaConfig.LUA_REGISTRYINDEX)
        { /* upvalues */
            int uvIdx = LuaConfig.LUA_REGISTRYINDEX - idx - 1;
            if (closure != null
                    && closure.upvals.Length > uvIdx
                    && closure.upvals[uvIdx] != null)
            {
                closure.upvals[uvIdx].set(val);
            }
            return;
        }
        if (idx == LuaConfig.LUA_REGISTRYINDEX)
        {
            state.registry = (LuaTable)val;
            return;
        }
        int absIdx = absIndex(idx);
        slots[absIdx - 1] = val;
    }

    public void reverse(int from, int to)
    {
        //Collections.reverse(slots.subList(from, to + 1));
        // 这块是猜的 有隐患
        //slots.Reverse(from - 1, to - from + 1);
        //         if (to > from)
        //         {
        //             slots.Reverse(from, to - from + 1);
        //         }
        //         else if (to < from)
        //         {
        //             slots.Reverse(to, from - to + 1);
        //         }
        slots.Reverse(from, to - from + 1);
    }
}

