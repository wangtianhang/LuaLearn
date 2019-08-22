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
            Console.Write("\t{0}\t[{1}]\t0x{2:X8}\n", i + 1, line, code[i]);
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

