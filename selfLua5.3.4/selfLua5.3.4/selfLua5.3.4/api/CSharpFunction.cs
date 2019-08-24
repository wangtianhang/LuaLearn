using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// public interface CSharpFunction
// {
//     int invoke(LuaState ls);
// }

public delegate int CSharpFunction(LuaState ls);
