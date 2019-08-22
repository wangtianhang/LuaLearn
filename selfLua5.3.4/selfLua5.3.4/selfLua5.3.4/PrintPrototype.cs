using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class PrintPrototype
{
    public static void list(Prototype f)
    {
        printHeader(f);
        printCode(f);
        printDetail(f);
        foreach (var iter in f.Protos)
        {
            list(iter);
        }
    }

    private static void printHeader(Prototype f)
    {
        String funcType = f.LineDefined > 0 ? "function" : "main";
        String varargFlag = f.IsVararg > 0 ? "+" : "";

        string tmp = string.Format("\n{0} <{1}:{2},{3}> ({4} instructions)\n",
                funcType, f.Source, f.LineDefined, f.LastLineDefined,
                f.Code.Length);
        Console.Write(tmp);

        Console.Write("{0}{1} params, {2} slots, {3} upvalues, ",
                f.NumParams, varargFlag, f.MaxStackSize, f.upvalues.Length);

        Console.Write("{0} locals, {1} constants, {2} functions\n",
                f.LocVars.Length, f.Constants.Length, f.Protos.Length);
    }

    private static void printCode(Prototype f)
    {
        UInt32[] code = f.Code;
        UInt32[] lineInfo = f.LineInfo;
        for (int i = 0; i < code.Length; i++)
        {
            String line = lineInfo.Length > 0 ? lineInfo[i].ToString() : "-";
            Console.Write("\t{0}\t[{1}]\t{2}\t", i + 1, line, Instruction.getOpCode((int)code[i]).name);
            printOperands((int)code[i]);
            Console.WriteLine();
        }
    }

    private static void printOperands(int i)
    {
        OpCode opCode = Instruction.getOpCode(i);
        int a = Instruction.getA(i);
        switch (opCode.opMode)
        {
            case OpMode.iABC:
                Console.Write("{0}", a);
                if (opCode.argBMode != OpArgMask.OpArgN)
                {
                    int b = Instruction.getB(i);
                    Console.Write(" {0}", b > 0xFF ? -1 - (b & 0xFF) : b);
                }
                if (opCode.argCMode != OpArgMask.OpArgN)
                {
                    int c = Instruction.getC(i);
                    Console.Write(" {0}", c > 0xFF ? -1 - (c & 0xFF) : c);
                }
                break;
            case OpMode.iABx:
                Console.Write("{0}", a);
                int bx = Instruction.getBx(i);
                if (opCode.argBMode == OpArgMask.OpArgK)
                {
                    Console.Write(" {0}", -1 - bx);
                }
                else if (opCode.argBMode == OpArgMask.OpArgU)
                {
                    Console.Write(" {0}", bx);
                }
                break;
            case OpMode.iAsBx:
                int sbx = Instruction.getSBx(i);
                Console.Write("{0} {1}", a, sbx);
                break;
            case OpMode.iAx:
                int ax = Instruction.getAx(i);
                Console.Write("{0}", -1 - ax);
                break;
        }
    }

    private static void printDetail(Prototype f)
    {
        Console.Write("constants ({0}):\n", f.Constants.Length);
        int i = 1;
        foreach (var k in f.Constants)
        {
            Console.Write("\t{0}\t{1}\n", i++, constantToString(k));
        }

        i = 0;
        Console.Write("locals ({0}):\n", f.LocVars.Length);
        foreach (var locVar in f.LocVars)
        {
            Console.Write("\t{0}\t{1}\t{2}\t{3}\n", i++,
                    locVar.varName, locVar.startPC + 1, locVar.endPC + 1);
        }

        i = 0;
        Console.Write("upvalues ({0}):\n", f.upvalues.Length);
        foreach (var upval in f.upvalues)
        {
            String name = f.UpvalueNames.Length > 0 ? f.UpvalueNames[i] : "-";
            Console.Write("\t{0}\t{1}\t{2}\t{3}\n", i++,
                    name, upval.instack, upval.idx);
        }
    }

    private static String constantToString(Object k)
    {
        if (k == null)
        {
            return "nil";
        }
        else if (k is String) {
            return "\"" + k + "\"";
        } else {
            return k.ToString();
        }
    }
}

