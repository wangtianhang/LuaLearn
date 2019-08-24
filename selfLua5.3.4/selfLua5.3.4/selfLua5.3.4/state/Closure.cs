using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Closure
{

    public Prototype proto;
    public CSharpFunction csharpFunc;
    public UpvalueHolder[] upvals;

    public Closure(Prototype proto)
    {
        this.proto = proto;
        this.upvals = new UpvalueHolder[proto.upvalues.Length];
    }

    public Closure(CSharpFunction csharpFunc, int nUpvals)
    {
        this.csharpFunc = csharpFunc;
        this.upvals = new UpvalueHolder[nUpvals];
    }
}

