using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//namespace selfLua5._3._4
//{
public class BinaryChunk
{
    public byte[] signature = new byte[4];
    public byte version;
    public byte format;
    public byte[] luacData = new byte[6];
    public byte cintSize;
    public byte sizetSize;
    public byte instructionSize;
    public byte luaIntegerSize;
    public byte luaNumberSize;
    public Int64 luacInt;
    public double luacNum;
}

public class Prototype
{
    public string Source;
    public UInt32 LineDefined;
    public UInt32 LastLineDefined;
    public byte NumParams;
    public byte IsVararg;
    public byte MaxStackSize;
    public UInt32[] Code;
    // constants;
    // upvalue;
    public Prototype[] Protos;
    public UInt32[] LineInfo;
    // LocVar
    public string[] UpvalueNames;
}

class Program
{
    static void ProcessData(byte[] data)
    {
        BinaryChunk chunk = new BinaryChunk();
        MemoryStream stream = new MemoryStream(data);
        BinaryReader reader = new BinaryReader(stream);
        for(int i = 0; i < chunk.signature.Length; ++i)
        {
            chunk.signature[i] = (byte)stream.ReadByte();
        }
        chunk.version = (byte)stream.ReadByte();
        chunk.format = (byte)stream.ReadByte();
        for(int i = 0; i < chunk.luacData.Length; ++i)
        {
            chunk.luacData[i] = (byte)stream.ReadByte();
        }
        chunk.cintSize = (byte)stream.ReadByte();
        chunk.sizetSize = (byte)stream.ReadByte();
        chunk.instructionSize = (byte)stream.ReadByte();
        chunk.luaIntegerSize = (byte)stream.ReadByte();
        chunk.luaNumberSize = (byte)stream.ReadByte();
        chunk.luacInt = reader.ReadInt64();
        chunk.luacNum = reader.ReadDouble();

        int test = 0;
    }

    static void Main(string[] args)
    {
        byte[] data = File.ReadAllBytes(@"E:\gitHub\luaLearn\LuaLearn\originLua5.3.4\originLua5.3.4\Debug\luac.out");
        ProcessData(data);

        Console.WriteLine("test");
        Console.ReadLine();
    }
}
//}
