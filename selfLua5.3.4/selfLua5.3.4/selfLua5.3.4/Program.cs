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

class Program
{
    static void ProcessData(byte[] data)
    {
        BinaryChunk chunk = new BinaryChunk();
        MemoryStream stream = new MemoryStream(data);
        for(int i = 0; i < chunk.signature.Length; ++i)
        {
            chunk.signature[i] = (byte)stream.ReadByte();
        }
    }

    static void Main(string[] args)
    {
        byte[] data = File.ReadAllBytes("");
        ProcessData(data);

        Console.WriteLine("test");
        Console.ReadLine();
    }
}
//}
