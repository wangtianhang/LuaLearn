using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LuaTable
{
    public LuaTable metatable;
    private List<Object> arr;
    private Dictionary<Object, Object> map;

    // used by next()
    private Dictionary<Object, Object> keys;
    private Object lastKey;
    private bool changed;

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

    public bool hasMetafield(String fieldName)
    {
        return metatable != null && metatable.get(fieldName) != null;
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

        //return map != null ? map[key] : null;
        if (map != null && map.ContainsKey(key))
        {
            return map[key];
        }
        else
        {
            return null;
        }
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

        changed = true;
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
            map.Remove(key);
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

    public Object nextKey(Object key)
    {
        if (keys == null || (key == null && changed))
        {
            initKeys();
            changed = false;
        }

        Object nextKey = null;
        // 当key为null时返回LuaConfig.NULL_ALIAS
        keys.TryGetValue(key ?? LuaConfig.NULL_ALIAS, out nextKey);
        if (nextKey == null && key != null && key != lastKey)
        {
            throw new System.Exception("invalid key to 'next'");
        }

        return nextKey;
    }

    private void initKeys()
    {
        if (keys == null)
        {
            keys = new Dictionary<object, object>();
        }
        else
        {
            keys.Clear();
        }
        Object key = LuaConfig.NULL_ALIAS;
        if (arr != null)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i] != null)
                {
                    long nextKey = i + 1;
                    keys.Add(key, nextKey);
                    key = nextKey;
                }
            }
        }
        if (map != null)
        {
//             for (Object k : map.keySet())
//             {
//                 Object v = map.get(k);
//                 if (v != null)
//                 {
            foreach(var iter in map)
            { 
                if(iter.Value != null)
                { 
                    keys.Add(key, iter.Key);
                    key = iter.Key;
                }
            }
        }
        lastKey = key;
    }
}