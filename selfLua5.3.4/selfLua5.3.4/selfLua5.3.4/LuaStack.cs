using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class LuaStack
{
    private List<Object> slots = new List<Object>();

    int top()
    {
        return slots.Count();
    }

    void push(Object val)
    {
        if (slots.Count > 10000)
        { // TODO
            throw new System.Exception("lua 堆栈 过大");
        }
        slots.Add(val);
    }

    Object pop()
    {
        Object last = slots[slots.Count - 1];
        slots.RemoveAt(slots.Count - 1);
        return last;
    }

    int absIndex(int idx)
    {
        return idx >= 0 ? idx : idx + slots.Count + 1;
    }

    bool isValid(int idx)
    {
        int absIdx = absIndex(idx);
        return absIdx > 0 && absIdx <= slots.Count;
    }

    Object get(int idx)
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

    void set(int idx, Object val)
    {
        int absIdx = absIndex(idx);
        slots[absIdx - 1] = val;
    }

    void reverse(int from, int to)
    {
        //Collections.reverse(slots.subList(from, to + 1));
        // 这块是猜的 有隐患
        slots.Reverse(from, to - from + 1);
    }
}

