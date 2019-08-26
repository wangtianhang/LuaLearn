using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LuaStateImpl : LuaState, LuaVM
{
    public LuaTable registry = new LuaTable(0, 0);
    private LuaStack stack = new LuaStack();
    //Prototype proto;
    //private int pc;
    /* basic stack manipulation */

    //     public LuaStateImpl(Prototype proto)
    //     {
    //         this.proto = proto;
    //     }
    // 
    //     public LuaStateImpl()
    //     {
    //         proto = null;
    //     }
    public LuaStateImpl()
    {
        registry.put(LuaConfig.LUA_RIDX_GLOBALS, new LuaTable(0, 0));
        LuaStack stack = new LuaStack();
        stack.state = this;
        pushLuaStack(stack);
    }

    private void pushLuaStack(LuaStack newTop)
    {
        newTop.prev = this.stack;
        this.stack = newTop;
    }

    private void popLuaStack()
    {
        LuaStack top = this.stack;
        this.stack = top.prev;
        top.prev = null;
    }

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

    /* comparison and arithmetic functions */


    public void arith(ArithOp op)
    {
        Object b = stack.pop();
        Object a = op != ArithOp.LUA_OPUNM && op != ArithOp.LUA_OPBNOT ? stack.pop() : b;
        Object result = Arithmetic.arith(a, b, op, this);
        stack.push(result);
    }


    public bool compare(int idx1, int idx2, CmpOp op)
    {
        if (!stack.isValid(idx1) || !stack.isValid(idx2))
        {
            return false;
        }

        Object a = stack.get(idx1);
        Object b = stack.get(idx2);
        switch (op)
        {
            case CmpOp.LUA_OPEQ: return Comparison.eq(a, b, this);
            case CmpOp.LUA_OPLT: return Comparison.lt(a, b, this);
            case CmpOp.LUA_OPLE: return Comparison.le(a, b, this);
            default: throw new System.Exception("invalid compare op!");
        }
    }

    /* miscellaneous functions */


    public void len(int idx)
    {
        Object val = stack.get(idx);
        if (val is String) {
            pushInteger(((String)val).Length);
            return;
        }
        Object mm = getMetamethod(val, val, "__len");
        if (mm != null)
        {
            stack.push(callMetamethod(val, val, mm));
            return;
        }
        if (val is LuaTable) {
            pushInteger(((LuaTable)val).length());
            return;
        }
        throw new System.Exception("length error!"); throw new System.Exception("length error!");
        
    }


    public void concat(int n)
    {
        if (n == 0)
        {
            stack.push("");
        }
        else if (n >= 2)
        {
            for (int i = 1; i < n; i++)
            {
                if (isString(-1) && isString(-2))
                {
                    String s2 = toString(-1);
                    String s1 = toString(-2);
                    pop(2);
                    pushString(s1 + s2);
                    continue;
                }

                Object b = stack.pop();
                Object a = stack.pop();
                Object mm = getMetamethod(a, b, "__concat");
                if (mm != null)
                {
                    stack.push(callMetamethod(a, b, mm));
                    continue;
                }

                throw new System.Exception("concatenation error!");
            }
        }
        // n == 1, do nothing
    }

    /* LuaVM */

     public int getPC()
     {
         return stack.pc;
     }

    public void addPC(int n)
    {
        stack.pc += n;
    }

    public int fetch()
    {
        return (int)stack.closure.proto.Code[stack.pc++];
    }

    public void getConst(int idx)
    {
        stack.push(stack.closure.proto.Constants[idx]);
    }

    public void getRK(int rk)
    {
        if (rk > 0xFF)
        { // constant
            getConst(rk & 0xFF);
        }
        else
        { // register
            pushValue(rk + 1);
        }
    }

    /* get functions (Lua -> stack) */

    public void newTable()
    {
        createTable(0, 0);
    }

    public void createTable(int nArr, int nRec)
    {
        stack.push(new LuaTable(nArr, nRec));
    }

    public LuaType getTable(int idx)
    {
        Object t = stack.get(idx);
        Object k = stack.pop();
        return getTable(t, k, false);
    }

    public LuaType getField(int idx, String k)
    {
        Object t = stack.get(idx);
        return getTable(t, k, false);
    }

    public LuaType getI(int idx, long i)
    {
        Object t = stack.get(idx);
        return getTable(t, i, false);
    }

    private LuaType getTable(Object t, Object k, bool raw)
    {
        if (t is LuaTable) {
            LuaTable tbl = (LuaTable)t;
            Object v = tbl.get(k);
            if (raw || v != null || !tbl.hasMetafield("__index"))
            {
                stack.push(v);
                return LuaValue.typeOf(v);
            }
        }
        if (!raw)
        {
            Object mf = getMetafield(t, "__index");
            if (mf != null)
            {
                if (mf is LuaTable) {
                    return getTable(mf, k, false);
                } else if (mf is Closure) {
                    Object v = callMetamethod(t, k, mf);
                    stack.push(v);
                    return LuaValue.typeOf(v);
                }
            }
        }
        throw new System.Exception("not a table!"); // todo
    }

    /* set functions (stack -> Lua) */

    public void setTable(int idx)
    {
        Object t = stack.get(idx);
        Object v = stack.pop();
        Object k = stack.pop();
        setTable(t, k, v, false);
    }

    public void setField(int idx, String k)
    {
        Object t = stack.get(idx);
        Object v = stack.pop();
        setTable(t, k, v, false);
    }

    public void setI(int idx, long i)
    {
        Object t = stack.get(idx);
        Object v = stack.pop();
        setTable(t, i, v, false);
    }

    private void setTable(Object t, Object k, Object v, bool raw)
    {
        if (t is LuaTable) {
            LuaTable tbl = (LuaTable)t;
            if (raw || tbl.get(k) != null || !tbl.hasMetafield("__newindex"))
            {
                tbl.put(k, v);
                return;
            }
        }
        if (!raw)
        {
            Object mf = getMetafield(t, "__newindex");
            if (mf != null)
            {
                if (mf is LuaTable) {
                    setTable(mf, k, v, false);
                    return;
                }
                if (mf is Closure) {
                    stack.push(mf);
                    stack.push(t);
                    stack.push(k);
                    stack.push(v);
                    call(3, 0);
                    return;
                }
            }
        }
        throw new System.Exception("not a table!");
    }

    /* 'load' and 'call' functions */
    public ThreadStatus load(byte[] chunk, String chunkName, String mode)
    {
        Prototype proto = ProcessLuaData.Undump(chunk); // todo
        //stack.push(new Closure(proto));
        Closure closure = new Closure(proto);
        stack.push(closure);
        if (proto.upvalues.Length > 0)
        {
            Object env = registry.get(LuaConfig.LUA_RIDX_GLOBALS);
            closure.upvals[0] = new UpvalueHolder(env); // todo
        }
        return ThreadStatus.LUA_OK;
    }

    public void call(int nArgs, int nResults)
    {
        Object val = stack.get(-(nArgs + 1));
        Object f = val is Closure ? val: null;

        if (f == null)
        {
            Object mf = getMetafield(val, "__call");
            if (mf != null && mf is Closure) {
                stack.push(f);
                insert(-(nArgs + 2));
                nArgs += 1;
                f = mf;
            }
        }

        if (f != null)
        {
            Closure c = (Closure)f;
            if (c.proto != null)
            {
                callLuaClosure(nArgs, nResults, c);
            }
            else
            {
                callCSharpClosure(nArgs, nResults, c);
            }
        }
        else
        {
            throw new System.Exception("not function!");
        }
    }

    private void callCSharpClosure(int nArgs, int nResults, Closure c)
    {
        // create new lua stack
        LuaStack newStack = new LuaStack(/*nRegs+LUA_MINSTACK*/);
        newStack.state = this;
        newStack.closure = c;

        // pass args, pop func
        if (nArgs > 0)
        {
            newStack.pushN(stack.popN(nArgs), nArgs);
        }
        stack.pop();

        // run closure
        pushLuaStack(newStack);
        int r = c.csharpFunc(this);
        popLuaStack();

        // return results
        if (nResults != 0)
        {
            List<Object> results = newStack.popN(r);
            //stack.check(results.size())
            stack.pushN(results, nResults);
        }
    }

    private void callLuaClosure(int nArgs, int nResults, Closure c)
    {
        int nRegs = c.proto.MaxStackSize;
        int nParams = c.proto.NumParams;
        bool isVararg = c.proto.IsVararg == 1;

        // create new lua stack
        LuaStack newStack = new LuaStack(/*nRegs + LUA_MINSTACK*/);
        newStack.closure = c;

        // pass args, pop func
        List<Object> funcAndArgs = stack.popN(nArgs + 1);
        newStack.pushN(JavaHelper.SubList(funcAndArgs, 1, funcAndArgs.Count), nParams);
        if (nArgs > nParams && isVararg)
        {
            newStack.varargs = JavaHelper.SubList(funcAndArgs, nParams + 1, funcAndArgs.Count);
        }

        // run closure
        pushLuaStack(newStack);
        setTop(nRegs);
        runLuaClosure();
        popLuaStack();

        // return results
        if (nResults != 0)
        {
            List<Object> results = newStack.popN(newStack.top() - nRegs);
            //stack.check(results.size())
            stack.pushN(results, nResults);
        }
    }

    private void runLuaClosure()
    {
        for (; ; )
        {
            int i = fetch();
            OpCode opCode = Instruction.getOpCode(i);
            opCode.action(i, this);
            if (opCode == OpCode.RETURN)
            {
                break;
            }
        }
    }

    public int registerCount()
    {
        return stack.closure.proto.MaxStackSize;
    }

    public void loadVararg(int n)
    {
        List<Object> varargs = stack.varargs != null
                ? stack.varargs : new List<object>();
        if (n < 0)
        {
            n = varargs.Count;
        }

        //stack.check(n)
        stack.pushN(varargs, n);
    }

    public void loadProto(int idx)
    {
        Prototype proto = stack.closure.proto.Protos[idx];
        //stack.push(new Closure(proto));
        Closure closure = new Closure(proto);
        stack.push(closure);

        for (int i = 0; i < proto.upvalues.Length; i++)
        {
            Upvalue uvInfo = proto.upvalues[i];
            int uvIdx = uvInfo.idx;
            if (uvInfo.instack == 1)
            {
                if (stack.openuvs == null)
                {
                    stack.openuvs = new Dictionary<int, UpvalueHolder>();
                }
                if (stack.openuvs.ContainsKey(uvIdx))
                {
                    closure.upvals[i] = stack.openuvs[uvIdx];
                }
                else
                {
                    closure.upvals[i] = new UpvalueHolder(stack, uvIdx);
                    stack.openuvs.Add(uvIdx, closure.upvals[i]);
                }
            }
            else
            {
                closure.upvals[i] = stack.closure.upvals[uvIdx];
            }
        }
    }

    public void closeUpvalues(int a)
    {
        if (stack.openuvs != null)
        {
            //             for (Iterator<UpvalueHolder> it = stack.openuvs.values().iterator(); it.hasNext();)
            //             {
            //                 UpvalueHolder uv = it.next();
            //                 if (uv.index >= a - 1)
            //                 {
            //                     uv.migrate();
            //                     it.remove();
            //                 }
            //             }
            List<int> willRemoveKey = new List<int>();
            foreach(var iter in stack.openuvs)
            {
                if(iter.Value.index >= a - 1)
                {
                    iter.Value.migrate();
                    willRemoveKey.Add(iter.Key);
                }
            }
            foreach(var iter in willRemoveKey)
            {
                stack.openuvs.Remove(iter);
            }
        }
    }

    public bool isCSharpFunction(int idx)
    {
        Object val = stack.get(idx);
        return val is Closure
                && ((Closure)val).csharpFunc != null;
    }

    public CSharpFunction toCSharpFunction(int idx)
    {
        Object val = stack.get(idx);
        return val is Closure
                ? ((Closure)val).csharpFunc
                : null;
    }

    public void pushCSharpFunction(CSharpFunction f)
    {
        stack.push(new Closure(f, 0));
    }

    public void pushCSharpClosure(CSharpFunction f, int n)
    {
        Closure closure = new Closure(f, n);
        for (int i = n; i > 0; i--)
        {
            Object val = stack.pop();
            closure.upvals[i - 1] = new UpvalueHolder(val); // TODO
        }
        stack.push(closure);
    }

    public void register(String name, CSharpFunction f)
    {
        pushCSharpFunction(f);
        setGlobal(name);
    }



    public void pushGlobalTable()
    {
        stack.push(registry.get(LuaConfig.LUA_RIDX_GLOBALS));
    }

    public LuaType getGlobal(String name)
    {
        Object t = registry.get(LuaConfig.LUA_RIDX_GLOBALS);
        return getTable(t, name, false);
    }

    public void setGlobal(String name)
    {
        Object t = registry.get(LuaConfig.LUA_RIDX_GLOBALS);
        Object v = stack.pop();
        setTable(t, name, v, false);
    }

    /* metatable */

    private LuaTable getMetatable(Object val)
    {
        if (val is LuaTable) {
            return ((LuaTable)val).metatable;
        }
        String key = "_MT" + LuaValue.typeOf(val);
        Object mt = registry.get(key);
        return mt != null ? (LuaTable)mt : null;
    }

    private void setMetatable(Object val, LuaTable mt)
    {
        if (val is LuaTable) {
            ((LuaTable)val).metatable = mt;
            return;
        }
        String key = "_MT" + LuaValue.typeOf(val);
        registry.put(key, mt);
    }

    private Object getMetafield(Object val, String fieldName)
    {
        LuaTable mt = getMetatable(val);
        return mt != null ? mt.get(fieldName) : null;
    }

    public Object getMetamethod(Object a, Object b, String mmName)
    {
        Object mm = getMetafield(a, mmName);
        if (mm == null)
        {
            mm = getMetafield(b, mmName);
        }
        return mm;
    }

    public Object callMetamethod(Object a, Object b, Object mm)
    {
        //stack.check(4)
        stack.push(mm);
        stack.push(a);
        stack.push(b);
        call(2, 1);
        return stack.pop();
    }

    public int rawLen(int idx)
    {
        Object val = stack.get(idx);
        if (val is String) {
            return ((String)val).Length;
        } else if (val is LuaTable) {
            return ((LuaTable)val).length();
        } else {
            return 0;
        }
    }

    public bool rawEqual(int idx1, int idx2)
    {
        if (!stack.isValid(idx1) || !stack.isValid(idx2))
        {
            return false;
        }

        Object a = stack.get(idx1);
        Object b = stack.get(idx2);
        return Comparison.eq(a, b, null);
    }

    public LuaType rawGet(int idx)
    {
        Object t = stack.get(idx);
        Object k = stack.pop();
        return getTable(t, k, true);
    }

    public LuaType rawGetI(int idx, long i)
    {
        Object t = stack.get(idx);
        return getTable(t, i, true);
    }

    public bool getMetatable(int idx)
    {
        Object val = stack.get(idx);
        Object mt = getMetatable(val);
        if (mt != null)
        {
            stack.push(mt);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void rawSet(int idx)
    {
        Object t = stack.get(idx);
        Object v = stack.pop();
        Object k = stack.pop();
        setTable(t, k, v, true);
    }

    public void rawSetI(int idx, long i)
    {
        Object t = stack.get(idx);
        Object v = stack.pop();
        setTable(t, i, v, true);
    }

    public void setMetatable(int idx)
    {
        Object val = stack.get(idx);
        Object mtVal = stack.pop();

        if (mtVal == null)
        {
            setMetatable(val, null);
        }
        else if (mtVal is LuaTable) {
            setMetatable(val, (LuaTable)mtVal);
        } else {
            throw new System.Exception("table expected!"); // todo
        }
    }

    public bool next(int idx)
    {
        Object val = stack.get(idx);
        if (val is LuaTable) {
            LuaTable t = (LuaTable)val;
            Object key = stack.pop();
            Object nextKey = t.nextKey(key);
            if (nextKey != null)
            {
                stack.push(nextKey);
                stack.push(t.get(nextKey));
                return true;
            }
            return false;
        }
        throw new System.Exception("table expected!");
    }
}