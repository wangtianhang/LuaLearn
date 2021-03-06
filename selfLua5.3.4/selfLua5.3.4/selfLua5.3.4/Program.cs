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
        //OpCode opCode = OpCode.GetValue(0);

        //TestChapter2And3();

        //TestChaper4();

        //TestChapter5();

        //TestChapter6();

        //TestChapter7();

        //TestChapter08();

        //TestChapter09();

        //TestChapter10();
        //TestChapter11();

        //TestChapter12();

        TestChapter13();

        //TestChapter14();

        Console.WriteLine("selflua end");
        Console.ReadLine();
    }

    static void TestChapter2And3()
    {
        Console.WriteLine("=============chapter2and3=============");
        byte[] data = File.ReadAllBytes(@".\hello_world.luac.out");
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

    static void TestChapter6()
    {
        byte[] data = File.ReadAllBytes(@".\sum.luac.out");
        //byte[] data = File.ReadAllBytes(@"E:\Dev\GitHub_Self\luaLearn\LuaLearn\originLua5.3.4\originLua5.3.4\Debug\luac.out");
        BinaryChunk chunk = ProcessLuaData.ProcessData(data);
        LuaMain(chunk.mainFunc);
    }

    static void TestChapter7()
    {
        byte[] data = File.ReadAllBytes(@".\test_table.luac.out");
        //byte[] data = File.ReadAllBytes(@"E:\Dev\GitHub_Self\luaLearn\LuaLearn\originLua5.3.4\originLua5.3.4\Debug\luac.out");
        BinaryChunk chunk = ProcessLuaData.ProcessData(data);
        LuaMain(chunk.mainFunc);
    }

    static void LuaMain(Prototype proto)
    {
        return;

//         LuaVM vm = new LuaStateImpl(proto);
//         vm.setTop(proto.MaxStackSize);
//         for (; ; )
//         {
//             int pc = vm.getPC();
//             int i = vm.fetch();
//             OpCode opCode = Instruction.getOpCode(i);
//             if (opCode != OpCode.RETURN)
//             {
//                 opCode.action(i, vm);
// 
//                 Console.Write("[{0}] {1} ", pc + 1, opCode.name);
//                 PrintStackData.printStack(vm);
//             }
//             else
//             {
//                 break;
//             }
//         }
    }

    static void TestChapter08()
    {
        byte[] data = File.ReadAllBytes(@".\chapter08.luac.out");
        LuaState ls = new LuaStateImpl();
        ls.load(data, "gaga", "b");
        ls.call(0, 0);
    }

    static void TestChapter09()
    {
        byte[] data = File.ReadAllBytes(@".\hello_world.luac.out");
        LuaState ls = new LuaStateImpl();
        ls.register("print", BaseLib.print);
        ls.load(data, "gaga", "b");
        ls.call(0, 0);
    }

    static void TestChapter10()
    {
        byte[] data = File.ReadAllBytes(@".\closure_test.luac.out");
        LuaState ls = new LuaStateImpl();
        ls.register("print", BaseLib.print);
        ls.load(data, "gaga", "b");
        ls.call(0, 0);
    }

    static void TestChapter11()
    {
        byte[] data = File.ReadAllBytes(@".\metatable_test.luac.out");
        LuaState ls = new LuaStateImpl();
        ls.register("print", BaseLib.print);
        ls.register("getmetatable", BaseLib.getMetatable);
        ls.register("setmetatable", BaseLib.setMetatable);
        ls.load(data, "gaga", "b");
        ls.call(0, 0);
    }

    static void TestChapter12()
    {
        byte[] data = File.ReadAllBytes(@".\iter_test.luac.out");
        LuaState ls = new LuaStateImpl();
        ls.register("print", BaseLib.print);
        ls.register("getmetatable", BaseLib.getMetatable);
        ls.register("setmetatable", BaseLib.setMetatable);

        ls.register("next", BaseLib.next);
        ls.register("pairs", BaseLib.pairs);
        ls.register("ipairs", BaseLib.iPairs);

        ls.load(data, "gaga", "b");
        ls.call(0, 0);

    }

    static void TestChapter13()
    {
        byte[] data = File.ReadAllBytes(@".\error_test.luac.out");
        LuaState ls = new LuaStateImpl();
        ls.register("print", BaseLib.print);
        ls.register("getmetatable", BaseLib.getMetatable);
        ls.register("setmetatable", BaseLib.setMetatable);

        ls.register("next", BaseLib.next);
        ls.register("pairs", BaseLib.pairs);
        ls.register("ipairs", BaseLib.iPairs);

        ls.register("error", BaseLib.error);
        ls.register("pcall", BaseLib.pCall);

        ls.load(data, "gaga", "b");
        ls.call(0, 0);
    }

    static void TestChapter14()
    {
        // 编译部分不是很稳定。。先算了。。
        string text = File.ReadAllText("./metatable_test.lua");
        testLexer(text, "gaga");
    }

    private static void testLexer(String chunk, String chunkName)
    {
        Lexer lexer = new Lexer(chunk, chunkName);
        for (; ; )
        {
            Token token = lexer.nextToken();
            Console.Write("[{0}] [{1}] {2}\n",
                    token.line, kindToCategory(token.kind), token.value);
            if (token.kind == TokenKind.TOKEN_EOF)
            {
                break;
            }
        }
    }

    private static String kindToCategory(TokenKind kind)
    {
        if (kind < TokenKind.TOKEN_SEP_SEMI)
        {
            return "other";
        }
        if (kind <= TokenKind.TOKEN_SEP_RCURLY)
        {
            return "separator";
        }
        if (kind <= TokenKind.TOKEN_OP_NOT)
        {
            return "operator";
        }
        if (kind <= TokenKind.TOKEN_KW_WHILE)
        {
            return "keyword";
        }
        if (kind == TokenKind.TOKEN_IDENTIFIER)
        {
            return "identifier";
        }
        if (kind == TokenKind.TOKEN_NUMBER)
        {
            return "number";
        }
        if (kind == TokenKind.TOKEN_STRING)
        {
            return "string";
        }
        return "other";
    }
}
//}
