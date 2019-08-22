using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class BinaryChunk
{
    public Header header;
    public byte sizeUpvalues;
    public Prototype mainFunc;
}

//namespace selfLua5._3._4
//{
public class Header
{
    public byte[] signature = new byte[4];
    public byte version;
    public byte format;
    public byte[] luacData = new byte[6];
    public byte cintSize  =4;
    public byte sizetSize = 8;
    public byte instructionSize = 4;
    public byte luaIntegerSize = 8;
    public byte luaNumberSize = 8;
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

        Header header = new Header();
        MemoryStream stream = new MemoryStream(data);
        BinaryReader reader = new BinaryReader(stream);
        for(int i = 0; i < header.signature.Length; ++i)
        {
            header.signature[i] = (byte)stream.ReadByte();
        }
        header.version = (byte)stream.ReadByte();
        header.format = (byte)stream.ReadByte();
        for(int i = 0; i < header.luacData.Length; ++i)
        {
            header.luacData[i] = (byte)stream.ReadByte();
        }
        header.cintSize = (byte)stream.ReadByte();
        header.sizetSize = (byte)stream.ReadByte();
        header.instructionSize = (byte)stream.ReadByte();
        header.luaIntegerSize = (byte)stream.ReadByte();
        header.luaNumberSize = (byte)stream.ReadByte();
        header.luacInt = reader.ReadInt64();
        header.luacNum = reader.ReadDouble();
        chunk.header = header;

        chunk.sizeUpvalues = reader.ReadByte();

        Prototype main = new Prototype();
        //byte length = reader.ReadByte();
        //byte length = reader.ReadByte();
        //byte length2 = reader.ReadByte();
        //byte[] bytes = reader.ReadBytes(length2 - 1);
        main.Source = ReadLuaString(reader);

        int test = 0;
    }

    static string ReadLuaString(BinaryReader reader)
    {
        byte size = reader.ReadByte();
        if (size == 0)
        {
            return "";
        }
        else if(size <= 253)
        {
            byte[] bytes = reader.ReadBytes(size - 1);
            return System.Text.Encoding.Default.GetString(bytes);
        }
        else if(size >= 253)
        {
            Int64 size2 = reader.ReadInt64();
            byte[] bytes = reader.ReadBytes((int)size2 - 1);
            return System.Text.Encoding.Default.GetString(bytes);
        }
        else
        {
            throw new Exception("读取luastring失败");
        }
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
