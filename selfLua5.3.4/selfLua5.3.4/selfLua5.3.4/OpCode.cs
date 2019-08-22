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

public struct OpCode
{
    /*       T  A    B       C     mode */
    static OpCode MOVE = new OpCode(0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC); // R(A) := R(B)
    static OpCode LOADK = new OpCode(0, 1, OpArgMask.OpArgK, OpArgMask.OpArgN, OpMode.iABx); // R(A) := Kst(Bx)
    static OpCode LOADKX = new OpCode(0, 1, OpArgMask.OpArgN, OpArgMask.OpArgN, OpMode.iABx); // R(A) := Kst(extra arg)
    static OpCode LOADBOOL = new OpCode(0, 1, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC); // R(A) := (bool)B; if (C) pc++
    static OpCode LOADNIL = new OpCode(0, 1, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC); // R(A), R(A+1), ..., R(A+B) := nil
    static OpCode GETUPVAL = new OpCode(0, 1, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC); // R(A) := UpValue[B]
    static OpCode GETTABUP = new OpCode(0, 1, OpArgMask.OpArgU, OpArgMask.OpArgK, OpMode.iABC); // R(A) := UpValue[B][RK(C)]
    static OpCode GETTABLE = new OpCode(0, 1, OpArgMask.OpArgR, OpArgMask.OpArgK, OpMode.iABC); // R(A) := R(B)[RK(C)]
    static OpCode SETTABUP = new OpCode(0, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // UpValue[A][RK(B)] := RK(C)
    static OpCode SETUPVAL = new OpCode(0, 0, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC); // UpValue[B] := R(A)
    static OpCode SETTABLE = new OpCode(0, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A)[RK(B)] := RK(C)
    static OpCode NEWTABLE = new OpCode(0, 1, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC); // R(A) := {} (size = B,C)
    static OpCode SELF = new OpCode(0, 1, OpArgMask.OpArgR, OpArgMask.OpArgK, OpMode.iABC); // R(A+1) := R(B); R(A) := R(B)[RK(C)]
    static OpCode ADD = new OpCode(0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) + RK(C)
    static OpCode SUB = new OpCode(0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) - RK(C)
    static OpCode MUL = new OpCode(0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) * RK(C)
    static OpCode MOD = new OpCode(0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) % RK(C)
    static OpCode POW = new OpCode(0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) ^ RK(C)
    static OpCode DIV = new OpCode(0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) / RK(C)
    static OpCode IDIV = new OpCode(0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) // RK(C)
    static OpCode BAND = new OpCode(0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) & RK(C)
    static OpCode BOR = new OpCode(0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) | RK(C)
    static OpCode BXOR = new OpCode(0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) ~ RK(C)
    static OpCode SHL = new OpCode(0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) << RK(C)
    static OpCode SHR = new OpCode(0, 1, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // R(A) := RK(B) >> RK(C)
    static OpCode UNM = new OpCode(0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC); // R(A) := -R(B)
    static OpCode BNOT = new OpCode(0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC); // R(A) := ~R(B)
    static OpCode NOT = new OpCode(0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC); // R(A) := not R(B)
    static OpCode LEN = new OpCode(0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iABC); // R(A) := length of R(B)
    static OpCode CONCAT = new OpCode(0, 1, OpArgMask.OpArgR, OpArgMask.OpArgR, OpMode.iABC); // R(A) := R(B).. ... ..R(C)
    static OpCode JMP = new OpCode(0, 0, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iAsBx); // pc+=sBx; if (A) close all upvalues >= R(A - 1)
    static OpCode EQ = new OpCode(1, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // if ((RK(B) == RK(C)) ~= A) then pc++
    static OpCode LT = new OpCode(1, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // if ((RK(B) <  RK(C)) ~= A) then pc++
    static OpCode LE = new OpCode(1, 0, OpArgMask.OpArgK, OpArgMask.OpArgK, OpMode.iABC); // if ((RK(B) <= RK(C)) ~= A) then pc++
    static OpCode TEST = new OpCode(1, 0, OpArgMask.OpArgN, OpArgMask.OpArgU, OpMode.iABC); // if not (R(A) <=> C) then pc++
    static OpCode TESTSET = new OpCode(1, 1, OpArgMask.OpArgR, OpArgMask.OpArgU, OpMode.iABC); // if (R(B) <=> C) then R(A) := R(B) else pc++
    static OpCode CALL = new OpCode(0, 1, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC); // R(A), ... ,R(A+C-2) := R(A)(R(A+1), ... ,R(A+B-1))
    static OpCode TAILCALL = new OpCode(0, 1, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC); // return R(A)(R(A+1), ... ,R(A+B-1))
    static OpCode RETURN = new OpCode(0, 0, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC); // return R(A), ... ,R(A+B-2)
    static OpCode FORLOOP = new OpCode(0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iAsBx); // R(A)+=R(A+2); if R(A) <?= R(A+1) then { pc+=sBx; R(A+3)=R(A) }
    static OpCode FORPREP = new OpCode(0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iAsBx); // R(A)-=R(A+2); pc+=sBx
    static OpCode TFORCALL = new OpCode(0, 0, OpArgMask.OpArgN, OpArgMask.OpArgU, OpMode.iABC); // R(A+3), ... ,R(A+2+C) := R(A)(R(A+1), R(A+2));
    static OpCode TFORLOOP = new OpCode(0, 1, OpArgMask.OpArgR, OpArgMask.OpArgN, OpMode.iAsBx); // if R(A+1) ~= nil then { R(A)=R(A+1); pc += sBx }
    static OpCode SETLIST = new OpCode(0, 0, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iABC); // R(A)[(C-1)*FPF+i] := R(A+i), 1 <= i <= B
    static OpCode CLOSURE = new OpCode(0, 1, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABx); // R(A) := closure(KPROTO[Bx])
    static OpCode VARARG = new OpCode(0, 1, OpArgMask.OpArgU, OpArgMask.OpArgN, OpMode.iABC); // R(A), R(A+1), ..., R(A+B-2) = vararg
    static OpCode EXTRAARG = new OpCode(0, 0, OpArgMask.OpArgU, OpArgMask.OpArgU, OpMode.iAx); // extra (larger) argument for previous opcode

    int testFlag; // operator is a test (next instruction must be a jump)
    int setAFlag; // instruction set register A
    OpArgMask argBMode; // B arg mode
    OpArgMask argCMode; // C arg mode
    OpMode opMode; // op mode

    OpCode(int testFlag, int setAFlag,
       OpArgMask argBMode, OpArgMask argCMode, OpMode opMode)
    {
        this.testFlag = testFlag;
        this.setAFlag = setAFlag;
        this.argBMode = argBMode;
        this.argCMode = argCMode;
        this.opMode = opMode;
    }
}
