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
    long toIntegerX(int idx);
    double toNumber(int idx);
    Double toNumberX(int idx);
    String toString(int idx);
    /* push functions (Go -> stack); */
    void pushNil();
    void pushBoolean(bool b);
    void pushInteger(long n);
    void pushNumber(double n);
    void pushString(String s);

}
