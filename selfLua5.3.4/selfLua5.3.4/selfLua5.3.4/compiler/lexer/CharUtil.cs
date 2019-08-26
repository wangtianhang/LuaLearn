using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CharUtil
{
    public static bool isWhiteSpace(char c)
    {
        switch (c)
        {
            case '\t':
            case '\n':
            //case 0x0B: // \v
            case '\v':
            case '\f':
            case '\r':
            case ' ':
                return true;
        }
        return false;
    }

    public static bool isNewLine(char c)
    {
        return c == '\r' || c == '\n';
    }

    static bool isDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    static bool isLetter(char c)
    {
        return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
    }
}
