using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class CharSequence
{

}

class CharSeq : CharSequence
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
        return str.StartsWith(prefix, pos);
    }

    int indexOf(String s)
    {
        return str.IndexOf(s, pos) - pos;
    }

    String substring(int beginIndex, int endIndex)
    {
        return str.Substring(beginIndex + pos, endIndex + pos);
    }

    String find(Pattern pattern)
    {
        Matcher matcher = pattern.matcher(this);
        return matcher.find()
                ? matcher.group(0) : null;
    }

    public int length()
    {
        return str.Length - pos;
    }

    public char charAt(int index)
    {
        return str[index + pos];
    }

    public CharSequence subSequence(int start, int end)
    {
        return str.subSequence(start + pos, end + pos);
    }
}

