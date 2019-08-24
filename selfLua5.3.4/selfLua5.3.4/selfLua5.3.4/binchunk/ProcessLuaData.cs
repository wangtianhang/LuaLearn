using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



//namespace selfLua5._3._4
//{
public class Header
{
    public byte[] signature = new byte[4];
    public byte version;
    public byte format;
    public byte[] luacData = new byte[6];
    public byte cintSize = 4;
    public byte sizetSize = 8;
    public byte instructionSize = 4;
    public byte luaIntegerSize = 8;
    public byte luaNumberSize = 8;
    public Int64 luacInt;
    public double luacNum;
}




class ProcessLuaData
{
    private const int TAG_NIL = 0x00;
    private const int TAG_BOOLEAN = 0x01;
    private const int TAG_NUMBER = 0x03;
    private const int TAG_INTEGER = 0x13;
    private const int TAG_SHORT_STR = 0x04;
    private const int TAG_LONG_STR = 0x14;

    public static BinaryChunk ProcessData(byte[] data)
    {
        BinaryChunk chunk = new BinaryChunk();

        Header header = new Header();
        MemoryStream stream = new MemoryStream(data);
        BinaryReader reader = new BinaryReader(stream);
        for (int i = 0; i < header.signature.Length; ++i)
        {
            header.signature[i] = (byte)stream.ReadByte();
        }
        header.version = (byte)stream.ReadByte();
        header.format = (byte)stream.ReadByte();
        for (int i = 0; i < header.luacData.Length; ++i)
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

        chunk.mainFunc = ReadProtoType(reader);

        //int test = 0;

        return chunk;
    }

    static Upvalue ReadUpvalue(BinaryReader reader)
    {
        Upvalue ret = new Upvalue();
        ret.instack = reader.ReadByte();
        ret.idx = reader.ReadByte();
        return ret;
    }

    static Upvalue[] ReadUpvalues(BinaryReader reader)
    {
        List<Upvalue> upvalueList = new List<Upvalue>();
        UInt32 size = reader.ReadUInt32();
        for (int i = 0; i < size; ++i)
        {
            upvalueList.Add(ReadUpvalue(reader));
        }
        return upvalueList.ToArray();
    }

    static UInt32[] ReadLineInfo(BinaryReader reader)
    {
        List<UInt32> lineInfoList = new List<uint>();
        UInt32 size = reader.ReadUInt32();
        for (int i = 0; i < size; ++i)
        {
            lineInfoList.Add(reader.ReadUInt32());
        }
        return lineInfoList.ToArray();
    }

    static UInt32[] ReadCode(BinaryReader reader)
    {
        List<UInt32> opList = new List<uint>();
        UInt32 size = reader.ReadUInt32();
        for (int i = 0; i < size; ++i)
        {
            opList.Add(reader.ReadUInt32());
        }
        return opList.ToArray();
    }

    static Object ReadConstant(BinaryReader reader)
    {
        int type = reader.ReadByte();
        switch (type)
        {
            case TAG_NIL:
                return null;
            case TAG_BOOLEAN:
                return reader.ReadByte() != 0;
            case TAG_INTEGER:
                return ReadLuaInteger(reader);
            case TAG_NUMBER:
                return ReadLuaNumber(reader);
            case TAG_SHORT_STR:
                return ReadLuaString(reader);
            case TAG_LONG_STR:
                return ReadLuaString(reader);
            default:
                throw new Exception("不支持的constant类型");
        }
    }

    static Object[] ReadConstants(BinaryReader reader)
    {
        List<Object> objList = new List<object>();
        UInt32 size = reader.ReadUInt32();
        for (int i = 0; i < size; ++i)
        {
            objList.Add(ReadConstant(reader));
        }
        return objList.ToArray();
    }

    static Prototype ReadProtoType(BinaryReader reader)
    {
        Prototype protoType = new Prototype();
        //byte length = reader.ReadByte();
        //byte length = reader.ReadByte();
        //byte length2 = reader.ReadByte();
        //byte[] bytes = reader.ReadBytes(length2 - 1);
        protoType.Source = ReadLuaString(reader);
        protoType.LineDefined = reader.ReadUInt32();
        protoType.LastLineDefined = reader.ReadUInt32();
        protoType.NumParams = reader.ReadByte();
        protoType.IsVararg = reader.ReadByte();
        protoType.MaxStackSize = reader.ReadByte();
        protoType.Code = ReadCode(reader);
        protoType.Constants = ReadConstants(reader);
        protoType.upvalues = ReadUpvalues(reader);
        UInt32 protoSize = reader.ReadUInt32();
        List<Prototype> protoList = new List<Prototype>();
        for (int i = 0; i < protoSize; ++i)
        {
            protoList.Add(ReadProtoType(reader));
        }
        protoType.Protos = protoList.ToArray();
        protoType.LineInfo = ReadLineInfo(reader);
        protoType.LocVars = ReadLocVars(reader);
        protoType.UpvalueNames = ReadUpvalueNames(reader);

        return protoType;
    }

    static LocVar ReadLocVar(BinaryReader reader)
    {
        LocVar ret = new LocVar();
        ret.varName = ReadLuaString(reader);
        ret.startPC = reader.ReadInt32();
        ret.endPC = reader.ReadInt32();
        return ret;
    }

    static LocVar[] ReadLocVars(BinaryReader reader)
    {
        List<LocVar> locVarList = new List<LocVar>();
        UInt32 size = reader.ReadUInt32();
        for (int i = 0; i < size; ++i)
        {
            locVarList.Add(ReadLocVar(reader));
        }
        return locVarList.ToArray();
    }

    static string[] ReadUpvalueNames(BinaryReader reader)
    {
        List<string> upvalueNameList = new List<string>();
        UInt32 size = reader.ReadUInt32();
        for (int i = 0; i < size; ++i)
        {
            upvalueNameList.Add(ReadLuaString(reader));
        }
        return upvalueNameList.ToArray();
    }

    static string ReadLuaString(BinaryReader reader)
    {
        byte size = reader.ReadByte();
        if (size == 0)
        {
            return "";
        }
        else if (size <= 253)
        {
            byte[] bytes = reader.ReadBytes(size - 1);
            return System.Text.Encoding.Default.GetString(bytes);
        }
        else if (size >= 253)
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

    static long ReadLuaInteger(BinaryReader reader)
    {
        return reader.ReadInt64();
    }

    static double ReadLuaNumber(BinaryReader reader)
    {
        return reader.ReadDouble();
    }

    public static Prototype Undump(BinaryChunk chunk)
    {
        return chunk.mainFunc;
    }

    public static Prototype Undump(byte[] chunk)
    {
        return ProcessData(chunk).mainFunc;
    }
}

