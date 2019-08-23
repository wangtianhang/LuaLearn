using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public System.Object[] Constants;
    // upvalue;
    public Upvalue[] upvalues;
    public Prototype[] Protos;
    public UInt32[] LineInfo;
    // LocVar
    public LocVar[] LocVars;
    public string[] UpvalueNames;
}
