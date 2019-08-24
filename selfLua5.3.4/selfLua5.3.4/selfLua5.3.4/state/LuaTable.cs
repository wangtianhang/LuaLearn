using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LuaTable
{

    private List<Object> arr;
    private Dictionary<Object, Object> map;

    public LuaTable(int nArr, int nRec)
    {
        if (nArr > 0)
        {
            arr = new List<Object>(nArr);
        }
        if (nRec > 0)
        {
            map = new Dictionary<Object, Object>(nRec);
        }
    }

    public int length()
    {
        return arr == null ? 0 : arr.Count;
    }

    public Object get(Object key)
    {
        key = floatToInteger(key);

        if (arr != null && key is long) {
            int idx = (int)((long)key);
            if (idx >= 1 && idx <= arr.Count)
            {
                return arr[idx - 1];
            }
        }

        return map != null ? map[key] : null;
    }

    public void put(Object key, Object val)
    {
        if (key == null)
        {
            throw new System.Exception("table index is nil!");
        }
        if (key is Double && double.IsNaN((Double)key)) 
        {
            throw new System.Exception("table index is NaN!");
        }

        key = floatToInteger(key);
        if (key is long) {
            int idx = (int)((long)key);
            if (idx >= 1)
            {
                if (arr == null)
                {
                    arr = new List<Object>();
                }

                int arrLen = arr.Count;
                if (idx <= arrLen)
                {
                    arr[idx - 1] = val;
                    if (idx == arrLen && val == null)
                    {
                        shrinkArray();
                    }
                    return;
                }
                if (idx == arrLen + 1)
                {
                    if (map != null)
                    {
                        map.Remove(key);
                    }
                    if (val != null)
                    {
                        arr.Add(val);
                        expandArray();
                    }
                    return;
                }
            }
        }

        if (val != null)
        {
            if (map == null)
            {
                map = new Dictionary<object, object>();
            }
            map.Add(key, val);
        }
        else
        {
            if (map != null)
            {
                map.Remove(key);
            }
        }
    }

    private Object floatToInteger(Object key)
    {
        if (key is Double) {
            Double f = (Double)key;
            if (LuaNumber.isInteger(f))
            {
                return (long)f;
            }
        }
        return key;
    }

    private void shrinkArray()
    {
        for (int i = arr.Count - 1; i >= 0; i--)
        {
            if (arr[i] == null)
            {
                arr.RemoveAt(i);
            }
        }
    }

    private void expandArray()
    {
        if (map != null)
        {
            for (int idx = arr.Count + 1; ; idx++)
            {
                Object val = map.Remove((long)idx);
                if (val != null)
                {
                    arr.Add(val);
                }
                else
                {
                    break;
                }
            }
        }
    }

}