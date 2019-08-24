using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Closure
{

    public Prototype proto;
    public CSharpFunction csharpFunc;

    public Closure(Prototype proto)
    {
        this.proto = proto;
    }

    public Closure(CSharpFunction csharpFunc)
    {
        this.csharpFunc = csharpFunc;
    }
}

