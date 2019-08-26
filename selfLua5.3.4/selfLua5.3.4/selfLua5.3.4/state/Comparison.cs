using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Comparison
{

    public static bool eq(Object a, Object b, LuaStateImpl ls)
    {
        if (a == null)
        {
            return b == null;
        }
        else if (a is Boolean || a is String) {
            return a.Equals(b);
        } else if (a is long) {
            return a.Equals(b) ||
                    (b is Double && b.Equals((double)((long)a)));
        } else if (a is Double) {
            return a.Equals(b) ||
                    (b is long && a.Equals((double)((long)b)));
        }
        else if (a instanceof LuaTable) {
            if (b instanceof LuaTable && a != b && ls != null) {
                Object mm = ls.getMetamethod(a, b, "__eq");
                if (mm != null)
                {
                    return LuaValue.toBoolean(ls.callMetamethod(a, b, mm));
                }
            }
            return a == b;
        }
        else {
            return a == b;
        }
    }

    public static bool lt(Object a, Object b, LuaStateImpl ls)
    {
        if (a is String && b is String) {
            return ((String)a).CompareTo((String)b) < 0;
        }
        if (a is long) {
            if (b is long) {
                return ((long)a) < ((long)b);
            } else if (b is Double) {
                return (double)((long)a) < ((Double)b);
            }
        }
        if (a is Double) {
            if (b is Double) {
                return ((Double)a) < ((Double)b);
            } else if (b is long) {
                return ((Double)a) < (double)((long)b);
            }
        }
        Object mm = ls.getMetamethod(a, b, "__lt");
        if (mm != null)
        {
            return LuaValue.toBoolean(ls.callMetamethod(a, b, mm));
        }
        throw new System.Exception("comparison error!");
    }

    public static bool le(Object a, Object b, LuaStateImpl ls)
    {
        if (a is String && b is String) {
            return ((String)a).CompareTo((String)b) <= 0;
        }
        if (a is long) {
            if (b is long) {
                return ((long)a) <= ((long)b);
            } else if (b is Double) {
                return (double)((long)a) <= ((Double)b);
            }
        }
        if (a is Double) {
            if (b is Double) {
                return ((Double)a) <= ((Double)b);
            } else if (b is long) {
                return ((Double)a) <= (double)((long)b);
            }
        }
        Object mm = ls.getMetamethod(a, b, "__le");
        if (mm != null)
        {
            return LuaValue.toBoolean(ls.callMetamethod(a, b, mm));
        }
        mm = ls.getMetamethod(b, a, "__lt");
        if (mm != null)
        {
            return LuaValue.toBoolean(ls.callMetamethod(b, a, mm));
        }
        throw new System.Exception("comparison error!");
    }

}
