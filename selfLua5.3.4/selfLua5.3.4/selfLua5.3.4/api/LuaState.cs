using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface LuaState
{


    /* basic stack manipulation */
    int getTop();
    int absIndex(int idx);
    bool checkStack(int n);
    void pop(int n);
    void copy(int fromIdx, int toIdx);
    void pushValue(int idx);
    void replace(int idx);
    void insert(int idx);
    void remove(int idx);
    void rotate(int idx, int n);
    void setTop(int idx);
    /* access functions (stack -> Go); */
    String typeName(LuaType tp);
    LuaType type(int idx);
    bool isNone(int idx);
    bool isNil(int idx);
    bool isNoneOrNil(int idx);
    bool isBoolean(int idx);
    bool isInteger(int idx);
    bool isNumber(int idx);
    bool isString(int idx);
    bool isTable(int idx);
    bool isThread(int idx);
    bool isFunction(int idx);
    bool toBoolean(int idx);
    long toInteger(int idx);
    bool toIntegerX(int idx, ref long ret);
    double toNumber(int idx);
    bool toNumberX(int idx, ref double ret);
    String toString(int idx);
    /* push functions (Go -> stack); */
    void pushNil();
    void pushBoolean(bool b);
    void pushInteger(long n);
    void pushNumber(double n);
    void pushString(String s);

    /* comparison and arithmetic functions */
    void arith(ArithOp op);
    bool compare(int idx1, int idx2, CmpOp op);
    /* miscellaneous functions */
    void len(int idx);
    void concat(int n);

    /* get functions (Lua -> stack) */
    void newTable();
    void createTable(int nArr, int nRec);
    LuaType getTable(int idx);
    LuaType getField(int idx, String k);
    LuaType getI(int idx, long i);
    /* set functions (stack -> Lua) */
    void setTable(int idx);
    void setField(int idx, String k);
    void setI(int idx, long i);

    /* 'load' and 'call' functions (load and run Lua code) */
    ThreadStatus load(byte[] chunk, String chunkName, String mode);
    void call(int nArgs, int nResults);

    bool isCSharpFunction(int idx);
    CSharpFunction toCSharpFunction(int idx);
    void pushCSharpFunction(CSharpFunction f);
    void register(String name, CSharpFunction f);
    void pushCSharpClosure(CSharpFunction f, int n);

    void pushGlobalTable();
    LuaType getGlobal(String name);
    void setGlobal(String name);

    int rawLen(int idx);
    bool rawEqual(int idx1, int idx2);
    LuaType rawGet(int idx);
    LuaType rawGetI(int idx, long i);
    bool getMetatable(int idx);
    void rawSet(int idx);
    void rawSetI(int idx, long i);
    void setMetatable(int idx);

    bool next(int idx);
}
