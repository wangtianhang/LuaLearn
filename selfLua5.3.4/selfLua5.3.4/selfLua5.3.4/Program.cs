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
        OpCode opCode = OpCode.GetValue(0);

        byte[] data = File.ReadAllBytes(@"E:\gitHub\luaLearn\LuaLearn\originLua5.3.4\originLua5.3.4\Debug\luac.out");
        BinaryChunk chunk = ProcessLuaData.ProcessData(data);
        PrintPrototype.list(ProcessLuaData.Undump(chunk));

        Console.WriteLine("selflua end");
        Console.ReadLine();
    }
}
//}
