using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    static void Main(string[] args)
    {
        Console.WriteLine("test");
        Console.ReadLine();
    }
}
//}
