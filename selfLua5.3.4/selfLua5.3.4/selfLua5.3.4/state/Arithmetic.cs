using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


delegate long LongOp(long a, long b);
delegate double DoubleOp(double a, double b);

class Arithmetic
{

    static List<LongOp> s_longOpList = new List<LongOp>();
    //     private static LongBinaryOperator[] integerOps = {
    //             (a, b) -> a + b,     // LUA_OPADD
    //             (a, b) -> a - b,     // LUA_OPSUB
    //             (a, b) -> a* b,     // LUA_OPMUL
    //             Math::floorMod,      // LUA_OPMOD
    //             null,                // LUA_OPPOW
    //             null,                // LUA_OPDIV
    //             Math::floorDiv,      // LUA_OPIDIV
    //             (a, b) -> a & b,     // LUA_OPBAND
    //             (a, b) -> a | b,     // LUA_OPBOR
    //             (a, b) -> a ^ b,     // LUA_OPBXOR
    //             LuaMath::shiftLeft,  // LUA_OPSHL
    //             LuaMath::shiftRight, // LUA_OPSHR
    //             (a, b) -> -a,        // LUA_OPUNM
    //             (a, b) -> ~a,        // LUA_OPBNOT
    //     };

    static List<DoubleOp> s_doubleOpList = new List<DoubleOp>();
    //     private static DoubleBinaryOperator[] floatOps = {
    //             (a, b) -> a + b,   // LUA_OPADD
    //             (a, b) -> a - b,   // LUA_OPSUB
    //             (a, b) -> a* b,   // LUA_OPMUL
    //             LuaMath::floorMod, // LUA_OPMOD
    //             Math::pow,         // LUA_OPPOW
    //             (a, b) -> a / b,   // LUA_OPDIV
    //             LuaMath::floorDiv, // LUA_OPIDIV
    //             null,              // LUA_OPBAND
    //             null,              // LUA_OPBOR
    //             null,              // LUA_OPBXOR
    //             null,              // LUA_OPSHL
    //             null,              // LUA_OPSHR
    //             (a, b) -> -a,      // LUA_OPUNM
    //             null,              // LUA_OPBNOT
    //     };

    static bool s_hasInit = false;
    static void Init()
    {
        if(s_hasInit)
        {
            return;
        }
        s_hasInit = true;

        s_longOpList.Add((a, b) => a + b);
        s_longOpList.Add((a, b) => a - b);
        s_longOpList.Add((a, b) => a * b);
        s_longOpList.Add((a, b) => MathHelper.floorMod(a, b));
        s_longOpList.Add(null);
        s_longOpList.Add(null);
        s_longOpList.Add((a, b) => MathHelper.floorDiv(a, b));
        s_longOpList.Add((a, b) => a & b);
        s_longOpList.Add((a, b) => a | b);
        s_longOpList.Add((a, b) => a ^ b);
        s_longOpList.Add((a, b) => LuaMath.shiftLeft(a, (int)b));
        s_longOpList.Add((a, b) => LuaMath.shiftRight(a, (int)b));
        s_longOpList.Add((a, b) => -a);
        s_longOpList.Add((a, b) => ~a);

        s_doubleOpList.Add((a, b) => a + b);
        s_doubleOpList.Add((a, b) => a - b);
        s_doubleOpList.Add((a, b) => a * b);
        s_doubleOpList.Add((a, b) => LuaMath.floorMod(a, b));
        s_doubleOpList.Add((a, b) => Math.Pow(a, b));
        s_doubleOpList.Add((a, b) => a / b);
        s_doubleOpList.Add((a, b) => LuaMath.floorDiv(a, b));
        s_doubleOpList.Add(null);
        s_doubleOpList.Add(null);
        s_doubleOpList.Add(null);
        s_doubleOpList.Add(null);
        s_doubleOpList.Add(null);
        s_doubleOpList.Add((a, b)=> - a);
        s_doubleOpList.Add(null);
    }

    static Object arith(Object a, Object b, ArithOp op)
    {
        Init();

        LongOp integerFunc = s_longOpList[(int)op];
        DoubleOp floatFunc = s_doubleOpList[(int)op];

        if (floatFunc == null)
        { // bitwise
            long x = 0;
            if (LuaValue.toInteger(a, ref x))
            {
                long y = 0;
                if (LuaValue.toInteger(b, ref y))
                {
                    return integerFunc(x, y);
                }
            }
        }
        else
        { // arith
            if (integerFunc != null)
            { // add,sub,mul,mod,idiv,unm
                if (a is long && b is long) {
                    return integerFunc((long)a, (long)b);
                }
            }
            Double x = 0;
            if (LuaValue.toFloat(a, ref x))
            {
                Double y = 0;
                if (LuaValue.toFloat(b, ref y))
                {
                    return floatFunc(x, y);
                }
            }
        }
        return null;
    }

}

