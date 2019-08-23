using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



class Program
{


    static void Main(string[] args)
    {
        //OpCode opCode = OpCode.GetValue(0);

        TestChapter2And3();

        TestChaper4();

        TestChapter5();

        Console.WriteLine("selflua end");
        Console.ReadLine();
    }

    static void TestChapter2And3()
    {
        Console.WriteLine("=============chapter2and3=============");
        byte[] data = File.ReadAllBytes(@"E:\gitHub\luaLearn\LuaLearn\originLua5.3.4\originLua5.3.4\Debug\luac.out");
        //byte[] data = File.ReadAllBytes(@"E:\Dev\GitHub_Self\luaLearn\LuaLearn\originLua5.3.4\originLua5.3.4\Debug\luac.out");
        BinaryChunk chunk = ProcessLuaData.ProcessData(data);
        PrintPrototype.list(ProcessLuaData.Undump(chunk));
        Console.WriteLine("==========================");
    }

    static void TestChaper4()
    {
        Console.WriteLine("=============chapter4=============");
        LuaState ls = new LuaStateImpl();

        ls.pushBoolean(true);
        PrintStackData.printStack(ls);
        ls.pushInteger(10);
        PrintStackData.printStack(ls);
        ls.pushNil();
        PrintStackData.printStack(ls);
        ls.pushString("hello");
        PrintStackData.printStack(ls);
        ls.pushValue(-4);
        PrintStackData.printStack(ls);
        ls.replace(3);
        PrintStackData.printStack(ls);
        ls.setTop(6);
        PrintStackData.printStack(ls);
        ls.remove(-3);
        PrintStackData.printStack(ls);
        ls.setTop(-5);
        PrintStackData.printStack(ls);
        Console.WriteLine("==========================");
    }

    static void TestChapter5()
    {
        Console.WriteLine("=============chapter5=============");
        LuaState ls = new LuaStateImpl();
        ls.pushInteger(1);
        ls.pushString("2.0");
        ls.pushString("3.0");
        ls.pushNumber(4.0);
        PrintStackData.printStack(ls);

        ls.arith(ArithOp.LUA_OPADD);
        PrintStackData.printStack(ls);
        ls.arith(ArithOp.LUA_OPBNOT);
        PrintStackData.printStack(ls);
        ls.len(2);
        PrintStackData.printStack(ls);
        ls.concat(3);
        PrintStackData.printStack(ls);
        ls.pushBoolean(ls.compare(1, 2, CmpOp.LUA_OPEQ));
        PrintStackData.printStack(ls);
        Console.WriteLine("==========================");
    }
}
//}
