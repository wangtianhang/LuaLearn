using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

// class CharSequence
// {
// 
// }

class CharSeq 
{
    private String str;
    private int pos;

    public CharSeq(String str)
    {
        this.str = str;
    }

    // TODO rename
    public char nextChar()
    {
        return str[pos++];
    }

    public void next(int n)
    {
        pos += n;
    }

    public bool startsWith(String prefix)
    {
        return str.IndexOf(prefix, pos) > 0;
    }

    public int indexOf(String s)
    {
        return str.IndexOf(s, pos) - pos;
    }

    public String substring(int beginIndex, int endIndex)
    {
        return str.Substring(beginIndex + pos, endIndex + pos);
    }

    public String find(Regex pattern)
    {
        Match matcher = pattern.Match(this.str);
        if(matcher != Match.Empty)
        {
            return matcher.Groups[0].Value;
        }
        else
        {
            return null;
        }
    }

    public int length()
    {
        return str.Length - pos;
    }

    public char charAt(int index)
    {
        return str[index + pos];
    }

    public CharSeq subSequence(int start, int end)
    {
        //return str.subSequence(start + pos, end + pos);
        string subStr = str.Substring(start + pos, end - start);
        return new CharSeq(str);
    }
}

