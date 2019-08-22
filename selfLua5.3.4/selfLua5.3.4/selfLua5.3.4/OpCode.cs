using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum OpMode
{
    iABC, // [  B:9  ][  C:9  ][ A:8  ][OP:6]
    iABx, // [      Bx:18     ][ A:8  ][OP:6]
    iAsBx, // [     sBx:18     ][ A:8  ][OP:6]
    iAx // [           Ax:26        ][OP:6]
}

public enum OpArgMask
{
    OpArgN, // argument is not used
    OpArgU, // argument is used
    OpArgR, // argument is a register or a jump offset
    OpArgK // argument is a constant or register/constant
}

public class Instruction
{
    public static int MAXARG_Bx = (1 << 18) - 1;   // 262143
    public static int MAXARG_sBx = MAXARG_Bx >> 1; // 131071

    public static OpCode getOpCode(int i)
    {
        return OpCode.GetValue(i & 0x3F);
    }

    public static int getA(int i)
    {
        return (i >> 6) & 0xFF;
    }

    public static int getC(int i)
    {
        return (i >> 14) & 0x1FF;
    }

    public static int getB(int i)
    {
        return (i >> 23) & 0x1FF;
    }

    public static int getBx(int i)
    {
        return MathHelper.UIntMoveRight(i, 14);
    }

    public static int getSBx(int i)
    {
        return getBx(i) - MAXARG_sBx;
    }

    public static int getAx(int i)
    {
        return MathHelper.UIntMoveRight(i, 6);
    }
}

public struct OpCode
{
    /*       T  A    B       C     mode */
    static OpCode MOVE = new OpCode("MOVE", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC); // R(A) := R(B)
    static OpCode LOADK = new OpCode("LOADK", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgN, OpMode.iABx); // R(A) := Kst(Bx)
    static OpCode LOADKX = new OpCode("LOADKX", 0, 1, OpArgMask.OpArgN, OpArgMask.OpArgN, OpMode.iABx); // R(A) := Kst(extra arg)
    static OpCode LOADBOOL = new OpCode("LOADBOOL", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC); // R(A) := (bool)B; if (C) pc++
    static OpCode LOADNIL = new OpCode("LOADNIL", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC); // R(A), R(A+1), ..., R(A+B) := nil
    static OpCode GETUPVAL = new OpCode("GETUPVAL", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC); // R(A) := UpValue[B]
    static OpCode GETTABUP = new OpCode("GETTABUP", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgK, OpMode.iABC); // R(A) := UpValue[B][RK(C)]
    static OpCode GETTABLE = new OpCode("GETTABLE", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgK, OpMode.iABC); // R(A) := R(B)[RK(C)]
    static OpCode SETTABUP = new OpCode("SETTABUP", 0, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // UpValue[A][RK(B)] := RK(C)
    static OpCode SETUPVAL = new OpCode("SETUPVAL", 0, 0, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC); // UpValue[B] := R(A)
    static OpCode SETTABLE = new OpCode("SETTABLE", 0, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A)[RK(B)] := RK(C)
    static OpCode NEWTABLE = new OpCode("NEWTABLE", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC); // R(A) := {} (size = B,C)
    static OpCode SELF = new OpCode("SELF", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgK, OpMode.iABC); // R(A+1) := R(B); R(A) := R(B)[RK(C)]
    static OpCode ADD = new OpCode("ADD", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) + RK(C)
    static OpCode SUB = new OpCode("SUB", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) - RK(C)
    static OpCode MUL = new OpCode("MUL", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) * RK(C)
    static OpCode MOD = new OpCode("MOD", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) % RK(C)
    static OpCode POW = new OpCode("POW", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) ^ RK(C)
    static OpCode DIV = new OpCode("DIV", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) / RK(C)
    static OpCode IDIV = new OpCode("IDIV", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) // RK(C)
    static OpCode BAND = new OpCode("BAND", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) & RK(C)
    static OpCode BOR = new OpCode("BOR", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) | RK(C)
    static OpCode BXOR = new OpCode("BXOR", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) ~ RK(C)
    static OpCode SHL = new OpCode("SHL", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) << RK(C)
    static OpCode SHR = new OpCode("SHR", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) >> RK(C)
    static OpCode UNM = new OpCode("UNM", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC); // R(A) := -R(B)
    static OpCode BNOT = new OpCode("BNOT", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC); // R(A) := ~R(B)
    static OpCode NOT = new OpCode("NOT", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC); // R(A) := not R(B)
    static OpCode LEN = new OpCode("LEN", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC); // R(A) := length of R(B)
    static OpCode CONCAT = new OpCode("CONCAT", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgR, OpMode.iABC); // R(A) := R(B).. ... ..R(C)
    static OpCode JMP = new OpCode("JMP", 0, 0, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iAsBx); // pc+=sBx; if (A) close all upvalues >= R(A - 1)
    static OpCode EQ = new OpCode("EQ", 1, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // if ((RK(B) == RK(C)) ~= A) then pc++
    static OpCode LT = new OpCode("LT", 1, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // if ((RK(B) <  RK(C)) ~= A) then pc++
    static OpCode LE = new OpCode("LE", 1, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // if ((RK(B) <= RK(C)) ~= A) then pc++
    static OpCode TEST = new OpCode("TEST", 1, 0, OpArgMask.OpArgN, OpArgMask.OpArgU, OpMode.iABC); // if not (R(A) <=> C) then pc++
    static OpCode TESTSET = new OpCode("TESTSET", 1, 1, OpArgMask.OpArgR, OpArgMask.OpArgU, OpMode.iABC); // if (R(B) <=> C) then R(A) := R(B) else pc++
    static OpCode CALL = new OpCode("CALL", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC); // R(A), ... ,R(A+C-2) := R(A)(R(A+1), ... ,R(A+B-1))
    static OpCode TAILCALL = new OpCode("TAILCALL", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC); // return R(A)(R(A+1), ... ,R(A+B-1))
    static OpCode RETURN = new OpCode("RETURN", 0, 0, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC); // return R(A), ... ,R(A+B-2)
    static OpCode FORLOOP = new OpCode("FORLOOP", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iAsBx); // R(A)+=R(A+2); if R(A) <?= R(A+1) then { pc+=sBx; R(A+3)=R(A) }
    static OpCode FORPREP = new OpCode("FORPREP", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iAsBx); // R(A)-=R(A+2); pc+=sBx
    static OpCode TFORCALL = new OpCode("TFORCALL", 0, 0, OpArgMask.OpArgN, OpArgMask.OpArgU, OpMode.iABC); // R(A+3), ... ,R(A+2+C) := R(A)(R(A+1), R(A+2));
    static OpCode TFORLOOP = new OpCode("TFORLOOP", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iAsBx); // if R(A+1) ~= nil then { R(A)=R(A+1); pc += sBx }
    static OpCode SETLIST = new OpCode("SETLIST", 0, 0, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC); // R(A)[(C-1)*FPF+i] := R(A+i), 1 <= i <= B
    static OpCode CLOSURE = new OpCode("CLOSURE", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABx); // R(A) := closure(KPROTO[Bx])
    static OpCode VARARG = new OpCode("VARARG", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC); // R(A), R(A+1), ..., R(A+B-2) = vararg
    static OpCode EXTRAARG = new OpCode("EXTRAARG", 0, 0, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iAx); // extra (larger) argument for previous opcode

    static bool s_Init = false;
    static List<OpCode> s_opCodeList = new List<OpCode>();

    static void Init()
    {
        if(s_Init)
        {
            return;
        }
        s_Init = true;

        s_opCodeList.Add(MOVE);
        s_opCodeList.Add(LOADK);
         s_opCodeList.Add(LOADKX);
        s_opCodeList.Add(LOADBOOL);
        s_opCodeList.Add(LOADNIL);
        s_opCodeList.Add(GETUPVAL);
        s_opCodeList.Add(GETTABUP);
        s_opCodeList.Add(GETTABLE);
        s_opCodeList.Add(SETTABUP);
        s_opCodeList.Add(SETUPVAL);
        s_opCodeList.Add(SETTABLE);
        s_opCodeList.Add(NEWTABLE);
        s_opCodeList.Add(SELF);
        s_opCodeList.Add(ADD);
        s_opCodeList.Add(SUB);
        s_opCodeList.Add(MUL);
        s_opCodeList.Add(MOD);
        s_opCodeList.Add(POW);
        s_opCodeList.Add(DIV);
        s_opCodeList.Add(IDIV);
        s_opCodeList.Add(BAND);
        s_opCodeList.Add(BOR);
        s_opCodeList.Add(BXOR);
        s_opCodeList.Add(SHL);
        s_opCodeList.Add(SHR);
        s_opCodeList.Add(UNM);
        s_opCodeList.Add(BNOT);
        s_opCodeList.Add(NOT);
        s_opCodeList.Add(LEN);
        s_opCodeList.Add(CONCAT);
        s_opCodeList.Add(JMP);
        s_opCodeList.Add(EQ);
        s_opCodeList.Add(LT);
        s_opCodeList.Add(LE);
        s_opCodeList.Add(TEST);
        s_opCodeList.Add(TESTSET);
        s_opCodeList.Add(CALL);
        s_opCodeList.Add(TAILCALL);
        s_opCodeList.Add(RETURN);
        s_opCodeList.Add(FORLOOP);
        s_opCodeList.Add(FORPREP);
        s_opCodeList.Add(TFORCALL);
        s_opCodeList.Add(TFORLOOP);
        s_opCodeList.Add(SETLIST);
        s_opCodeList.Add(CLOSURE);
        s_opCodeList.Add(VARARG);
        s_opCodeList.Add(EXTRAARG);
    }

    public static OpCode GetValue(int i)
    {
        Init();

        return s_opCodeList[i];
    }

    int testFlag; // operator is a test (next instruction must be a jump)
    int setAFlag; // instruction set register A
    OpArgMask argBMode; // B arg mode
    OpArgMask argCMode; // C arg mode
    OpMode opMode; // op mode
    string name;

    OpCode(string name, int testFlag, int setAFlag,
       OpArgMask argBMode, OpArgMask argCMode, OpMode opMode)
    {
        this.testFlag = testFlag;
        this.setAFlag = setAFlag;
        this.argBMode = argBMode;
        this.argCMode = argCMode;
        this.opMode = opMode;
        this.name = name;
    }
}
