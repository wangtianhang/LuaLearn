using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class LuaStack
{
    private List<Object> slots = new List<Object>();

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

    public int absIndex(int idx)
    {
        return idx >= 0 ? idx : idx + slots.Count + 1;
    }

    public bool isValid(int idx)
    {
        int absIdx = absIndex(idx);
        return absIdx > 0 && absIdx <= slots.Count;
    }

    public Object get(int idx)
    {
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
        int absIdx = absIndex(idx);
        slots[absIdx - 1] = val;
    }

    public void reverse(int from, int to)
    {
        //Collections.reverse(slots.subList(from, to + 1));
        // 这块是猜的 有隐患
        slots.Reverse(from - 1, to - from + 1);
    }
}

