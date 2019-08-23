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

        byte[] data = File.ReadAllBytes(@"E:\gitHub\luaLearn\LuaLearn\originLua5.3.4\originLua5.3.4\Debug\luac.out");
        //byte[] data = File.ReadAllBytes(@"E:\Dev\GitHub_Self\luaLearn\LuaLearn\originLua5.3.4\originLua5.3.4\Debug\luac.out");
        BinaryChunk chunk = ProcessLuaData.ProcessData(data);
        PrintPrototype.list(ProcessLuaData.Undump(chunk));

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

        Console.WriteLine("selflua end");
        Console.ReadLine();
    }
}
//}
