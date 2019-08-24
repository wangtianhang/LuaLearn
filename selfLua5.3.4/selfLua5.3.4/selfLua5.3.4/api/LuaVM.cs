using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface LuaVM : LuaState
{
    int getPC();
    void addPC(int n);
    int fetch();
    void getConst(int idx);
    void getRK(int rk);
}

