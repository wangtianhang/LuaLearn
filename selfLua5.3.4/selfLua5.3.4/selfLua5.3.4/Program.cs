﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



class Program
{


    static void Main(string[] args)
    {
        byte[] data = File.ReadAllBytes(@"E:\gitHub\luaLearn\LuaLearn\originLua5.3.4\originLua5.3.4\Debug\luac.out");
        BinaryChunk chunk = ProcessLuaData.ProcessData(data);

        Console.WriteLine("test");
        Console.ReadLine();
    }
}
//}
