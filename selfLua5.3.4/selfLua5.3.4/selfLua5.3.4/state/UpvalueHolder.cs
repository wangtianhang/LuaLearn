using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class UpvalueHolder
{
    public int index;
    private LuaStack stack;
    private Object value;

    public UpvalueHolder(Object value)
    {
        this.value = value;
        this.index = 0;
    }

    public UpvalueHolder(LuaStack stack, int index)
    {
        this.stack = stack;
        this.index = index;
    }

    public Object get()
    {
        return stack != null ? stack.get(index + 1) : value;
    }

    public void set(Object value)
    {
        if (stack != null)
        {
            stack.set(index + 1, value);
        }
        else
        {
            this.value = value;
        }
    }

    public void migrate()
    {
        if (stack != null)
        {
            value = stack.get(index + 1);
            stack = null;
        }
    }
}

