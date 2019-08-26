using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

class Escaper
{
    private static Regex reDecEscapeSeq = new Regex("^\\\\[0-9]{1,3}");
    private static Regex reHexEscapeSeq = new Regex("^\\\\x[0-9a-fA-F]{2}");
    private static Regex reUnicodeEscapeSeq = new Regex("^\\\\u\\{[0-9a-fA-F]+}");

    private CharSeq rawStr;
    private Lexer lexer;
    private StringBuilder buf = new StringBuilder();

    public Escaper(String rawStr, Lexer lexer)
    {
        this.rawStr = new CharSeq(rawStr);
        this.lexer = lexer;
    }

    public String escape()
    {
        while (rawStr.length() > 0)
        {
            if (rawStr.charAt(0) != '\\')
            {
                buf.Append(rawStr.nextChar());
                continue;
            }

            if (rawStr.length() == 1)
            {
                return lexer.error<string>("unfinished string");
            }

            switch (rawStr.charAt(1))
            {
                case 'a': buf.Append((char)0x07); rawStr.next(2); continue; // Bell
                case 'v': buf.Append((char)0x0B); rawStr.next(2); continue; // Vertical tab
                case 'b': buf.Append('\b'); rawStr.next(2); continue;
                case 'f': buf.Append('\f'); rawStr.next(2); continue;
                case 'n': buf.Append('\n'); rawStr.next(2); continue;
                case 'r': buf.Append('\r'); rawStr.next(2); continue;
                case 't': buf.Append('\t'); rawStr.next(2); continue;
                case '"': buf.Append('"'); rawStr.next(2); continue;
                case '\'': buf.Append('\''); rawStr.next(2); continue;
                case '\\': buf.Append('\\'); rawStr.next(2); continue;
                case '\n': buf.Append('\n'); rawStr.next(2); continue;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9': escapeDecSeq(); continue; // \ddd
                case 'x': escapeHexSeq(); continue; // \xXX
                case 'u': escapeUnicodeSeq(); continue; // \ u{XXX}
                case 'z':
                    rawStr.next(2);
                    skipWhitespaces();
                    continue;
            }
            reportInvalidEscapeSeq();
        }

        return buf.ToString();
    }

    private void reportInvalidEscapeSeq()
    {
        lexer.error<string>("invalid escape sequence near '\\%c'", rawStr.charAt(1));
    }

    // \ddd
    private void escapeDecSeq()
    {
        String seq = rawStr.find(reDecEscapeSeq);
        if (seq == null)
        {
            reportInvalidEscapeSeq();
        }

        try
        {
            int d = int.Parse(seq.Substring(1));
            if (d <= 0xFF)
            {
                buf.Append((char)d);
                rawStr.next(seq.Length);
                return;
            }
        }
        catch (System.Exception ignored) { }

        lexer.error<string>("decimal escape too large near '%s'", seq);
    }

    // \xXX
    private void escapeHexSeq()
    {
        String seq = rawStr.find(reHexEscapeSeq);
        if (seq == null)
        {
            reportInvalidEscapeSeq();
        }

        int d = int.Parse(seq.Substring(2), System.Globalization.NumberStyles.HexNumber);
        buf.Append((char)d);
        rawStr.next(seq.Length);
    }

    // \ u{XXX}
    private void escapeUnicodeSeq()
    {
        String seq = rawStr.find(reUnicodeEscapeSeq);
        if (seq == null)
        {
            reportInvalidEscapeSeq();
        }

        throw new System.Exception("暂时没想明白如何实现");
//         try
//         {
//             int length = seq.Length - 1 - 3;
//             int d = int.Parse(seq.Substring(3, length), System.Globalization.NumberStyles.HexNumber);
//             if (d <= 0x10FFFF)
//             {
//                 buf.AppendCodePoint(d);
//                 rawStr.next(seq.Length);
//                 return;
//             }
//         }
//         catch (System.Exception ignored) { }

        lexer.error<string>("UTF-8 value too large near '%s'", seq);
    }

    private void skipWhitespaces()
    {
        while (rawStr.length() > 0
                && CharUtil.isWhiteSpace(rawStr.charAt(0)))
        {
            rawStr.next(1);
        }
    }
}

