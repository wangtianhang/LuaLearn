using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LuaStateImpl : LuaState, LuaVM
{

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
        Object result = Arithmetic.arith(a, b, op);
        if (result != null)
        {
            stack.push(result);
        }
        else
        {
            throw new System.Exception("arithmetic error!");
        }
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
            case CmpOp.LUA_OPEQ: return Comparison.eq(a, b);
            case CmpOp.LUA_OPLT: return Comparison.lt(a, b);
            case CmpOp.LUA_OPLE: return Comparison.le(a, b);
            default: throw new System.Exception("invalid compare op!");
        }
    }

    /* miscellaneous functions */


    public void len(int idx)
    {
        Object val = stack.get(idx);
        if (val is String) {
            pushInteger(((String)val).Length);
        }
        else if (val is LuaTable) {
            pushInteger(((LuaTable)val).length());
        }else {
            throw new System.Exception("length error!");
        }
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
        return getTable(t, k);
    }

    public LuaType getField(int idx, String k)
    {
        Object t = stack.get(idx);
        return getTable(t, k);
    }

    public LuaType getI(int idx, long i)
    {
        Object t = stack.get(idx);
        return getTable(t, i);
    }

    private LuaType getTable(Object t, Object k)
    {
        if (t is LuaTable) {
            Object v = ((LuaTable)t).get(k);
            stack.push(v);
            return LuaValue.typeOf(v);
        }
        throw new System.Exception("not a table!"); // todo
    }

    /* set functions (stack -> Lua) */

    public void setTable(int idx)
    {
        Object t = stack.get(idx);
        Object v = stack.pop();
        Object k = stack.pop();
        setTable(t, k, v);
    }

    public void setField(int idx, String k)
    {
        Object t = stack.get(idx);
        Object v = stack.pop();
        setTable(t, k, v);
    }

    public void setI(int idx, long i)
    {
        Object t = stack.get(idx);
        Object v = stack.pop();
        setTable(t, i, v);
    }

    private void setTable(Object t, Object k, Object v)
    {
        if (t is LuaTable) {
            ((LuaTable)t).put(k, v);
            return;
        }
        throw new System.Exception("not a table!");
    }

    /* 'load' and 'call' functions */
    public ThreadStatus load(byte[] chunk, String chunkName, String mode)
    {
        Prototype proto = ProcessLuaData.Undump(chunk); // todo
        stack.push(new Closure(proto));
        return ThreadStatus.LUA_OK;
    }

    public void call(int nArgs, int nResults)
    {
        Object val = stack.get(-(nArgs + 1));
        if (val is Closure) {
            Closure c = (Closure)val;
            Console.Write("call {0}<{1},{2}>\n", c.proto.Source,
                    c.proto.LineDefined, c.proto.LastLineDefined);
            callLuaClosure(nArgs, nResults, c);
        } else {
            throw new System.Exception("not function!");
        }
    }

    private void callLuaClosure(int nArgs, int nResults, Closure c)
    {
        int nRegs = c.proto.MaxStackSize;
        int nParams = c.proto.NumParams;
        bool isVararg = c.proto.IsVararg == 1;

        // create new lua stack
        LuaStack newStack = new LuaStack(/*nRegs + 20*/);
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
        stack.push(new Closure(proto));
    }
}