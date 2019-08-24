using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate void OpAction(int i, LuaVM vm);

public struct OpCode
{
    /*       T  A    B       C     mode */
    public static OpCode MOVE = new OpCode("MOVE", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC, Instructions.move); // R(A) := R(B)
    public static OpCode LOADK = new OpCode("LOADK", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgN, OpMode.iABx, Instructions.loadK); // R(A) := Kst(Bx)
    public static OpCode LOADKX = new OpCode("LOADKX", 0, 1, OpArgMask.OpArgN, OpArgMask.OpArgN, OpMode.iABx, Instructions.loadKx); // R(A) := Kst(extra arg)
    public static OpCode LOADBOOL = new OpCode("LOADBOOL", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC, Instructions.loadBool); // R(A) := (bool)B; if (C) pc++
    public static OpCode LOADNIL = new OpCode("LOADNIL", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC, Instructions.loadNil); // R(A), R(A+1), ..., R(A+B) := nil
    public static OpCode GETUPVAL = new OpCode("GETUPVAL", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC, null); // R(A) := UpValue[B]
    public static OpCode GETTABUP = new OpCode("GETTABUP", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgK, OpMode.iABC, null); // R(A) := UpValue[B][RK(C)]
    public static OpCode GETTABLE = new OpCode("GETTABLE", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgK, OpMode.iABC, null); // R(A) := R(B)[RK(C)]
    public static OpCode SETTABUP = new OpCode("SETTABUP", 0, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, null); // UpValue[A][RK(B)] := RK(C)
    public static OpCode SETUPVAL = new OpCode("SETUPVAL", 0, 0, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC, null); // UpValue[B] := R(A)
    public static OpCode SETTABLE = new OpCode("SETTABLE", 0, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, null); // R(A)[RK(B)] := RK(C)
    public static OpCode NEWTABLE = new OpCode("NEWTABLE", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC, null); // R(A) := {} (size = B,C)
    public static OpCode SELF = new OpCode("SELF", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgK, OpMode.iABC, null); // R(A+1) := R(B); R(A) := R(B)[RK(C)]
    public static OpCode ADD = new OpCode("ADD", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.add); // R(A) := RK(B) + RK(C)
    public static OpCode SUB = new OpCode("SUB", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.sub); // R(A) := RK(B) - RK(C)
    public static OpCode MUL = new OpCode("MUL", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.mul); // R(A) := RK(B) * RK(C)
    public static OpCode MOD = new OpCode("MOD", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.mod); // R(A) := RK(B) % RK(C)
    public static OpCode POW = new OpCode("POW", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.pow); // R(A) := RK(B) ^ RK(C)
    public static OpCode DIV = new OpCode("DIV", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.div); // R(A) := RK(B) / RK(C)
    public static OpCode IDIV = new OpCode("IDIV", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.idiv); // R(A) := RK(B) // RK(C)
    public static OpCode BAND = new OpCode("BAND", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.band); // R(A) := RK(B) & RK(C)
    public static OpCode BOR = new OpCode("BOR", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.bor); // R(A) := RK(B) | RK(C)
    public static OpCode BXOR = new OpCode("BXOR", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.bxor); // R(A) := RK(B) ~ RK(C)
    public static OpCode SHL = new OpCode("SHL", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.shl); // R(A) := RK(B) << RK(C)
    public static OpCode SHR = new OpCode("SHR", 0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.shr); // R(A) := RK(B) >> RK(C)
    public static OpCode UNM = new OpCode("UNM", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC, Instructions.unm); // R(A) := -R(B)
    public static OpCode BNOT = new OpCode("BNOT", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC, Instructions.bnot); // R(A) := ~R(B)
    public static OpCode NOT = new OpCode("NOT", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC, Instructions.not); // R(A) := not R(B)
    public static OpCode LEN = new OpCode("LEN", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC, Instructions.length); // R(A) := length of R(B)
    public static OpCode CONCAT = new OpCode("CONCAT", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgR, OpMode.iABC, Instructions.concat); // R(A) := R(B).. ... ..R(C)
    public static OpCode JMP = new OpCode("JMP", 0, 0, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iAsBx, Instructions.jmp); // pc+=sBx; if (A) close all upvalues >= R(A - 1)
    public static OpCode EQ = new OpCode("EQ", 1, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.eq); // if ((RK(B) == RK(C)) ~= A) then pc++
    public static OpCode LT = new OpCode("LT", 1, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.lt); // if ((RK(B) <  RK(C)) ~= A) then pc++
    public static OpCode LE = new OpCode("LE", 1, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC, Instructions.le); // if ((RK(B) <= RK(C)) ~= A) then pc++
    public static OpCode TEST = new OpCode("TEST", 1, 0, OpArgMask.OpArgN, OpArgMask.OpArgU, OpMode.iABC, Instructions.test); // if not (R(A) <=> C) then pc++
    public static OpCode TESTSET = new OpCode("TESTSET", 1, 1, OpArgMask.OpArgR, OpArgMask.OpArgU, OpMode.iABC, Instructions.testSet); // if (R(B) <=> C) then R(A) := R(B) else pc++
    public static OpCode CALL = new OpCode("CALL", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC, null); // R(A), ... ,R(A+C-2) := R(A)(R(A+1), ... ,R(A+B-1))
    public static OpCode TAILCALL = new OpCode("TAILCALL", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC, null); // return R(A)(R(A+1), ... ,R(A+B-1))
    public static OpCode RETURN = new OpCode("RETURN", 0, 0, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC, null); // return R(A), ... ,R(A+B-2)
    public static OpCode FORLOOP = new OpCode("FORLOOP", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iAsBx, Instructions.forLoop); // R(A)+=R(A+2); if R(A) <?= R(A+1) then { pc+=sBx; R(A+3)=R(A) }
    public static OpCode FORPREP = new OpCode("FORPREP", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iAsBx, Instructions.forPrep); // R(A)-=R(A+2); pc+=sBx
    public static OpCode TFORCALL = new OpCode("TFORCALL", 0, 0, OpArgMask.OpArgN, OpArgMask.OpArgU, OpMode.iABC, null); // R(A+3), ... ,R(A+2+C) := R(A)(R(A+1), R(A+2));
    public static OpCode TFORLOOP = new OpCode("TFORLOOP", 0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iAsBx, null); // if R(A+1) ~= nil then { R(A)=R(A+1); pc += sBx }
    public static OpCode SETLIST = new OpCode("SETLIST", 0, 0, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC, null); // R(A)[(C-1)*FPF+i] := R(A+i), 1 <= i <= B
    public static OpCode CLOSURE = new OpCode("CLOSURE", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABx, null); // R(A) := closure(KPROTO[Bx])
    public static OpCode VARARG = new OpCode("VARARG", 0, 1, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC, null); // R(A), R(A+1), ..., R(A+B-2) = vararg
    public static OpCode EXTRAARG = new OpCode("EXTRAARG", 0, 0, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iAx, null); // extra (larger) argument for previous opcode

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

    public int testFlag; // operator is a test (next instruction must be a jump)
    public int setAFlag; // instruction set register A
    public OpArgMask argBMode; // B arg mode
    public OpArgMask argCMode; // C arg mode
    public OpMode opMode; // op mode
    public string name;
    public OpAction action;

    OpCode(string name, int testFlag, int setAFlag,
       OpArgMask argBMode, OpArgMask argCMode, OpMode opMode, OpAction action)
    {
        this.testFlag = testFlag;
        this.setAFlag = setAFlag;
        this.argBMode = argBMode;
        this.argCMode = argCMode;
        this.opMode = opMode;
        this.name = name;
        this.action = action;
    }

    public static bool operator ==(OpCode lhs, OpCode rhs)
    {
        //return lhs == rhs;
        return lhs.testFlag == rhs.testFlag
            && lhs.setAFlag == rhs.setAFlag
            && lhs.argBMode == rhs.argBMode
            && lhs.argCMode == rhs.argCMode
            && lhs.opMode == rhs.opMode
            && lhs.name == rhs.name
            && lhs.action == rhs.action;
    }

    public static bool operator !=(OpCode lhs, OpCode rhs)
    {
        return !(lhs == rhs);
    }

//     public override int GetHashCode()
//     {
//         return this.GetHashCode();
//     }
// 
//     public override bool Equals(object other)
//     {
//         if (!(other is OpCode))
//         {
//             return false;
//         }
//         OpCode color = (OpCode)other;
//         return this.r.Equals(color.r) 
//             && this.g.Equals(color.g) 
//             && this.b.Equals(color.b) 
//             && this.a.Equals(color.a);
//     }
}
