using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

class Lexer
{
    //private static final Pattern reSpaces = Pattern.compile("^\\s+");
    private static Regex reNewLine = new Regex("\r\n|\n\r|\n|\r");
    private static Regex reIdentifier = new Regex("^[_\\d\\w]+");
    private static Regex reNumber = new Regex("^0[xX][0-9a-fA-F]*(\\.[0-9a-fA-F]*)?([pP][+\\-]?[0-9]+)?|^[0-9]*(\\.[0-9]*)?([eE][+\\-]?[0-9]+)?");
    private static Regex reShortStr = new Regex("(?s)(^'(\\\\\\\\|\\\\'|\\\\\\n|\\\\z\\s*|[^'\\n])*')|(^\"(\\\\\\\\|\\\\\"|\\\\\\n|\\\\z\\s*|[^\"\\n])*\")");
    private static Regex reOpeningLongBracket = new Regex("^\\[=*\\[");

    private CharSeq chunk;
    private String chunkName;
    private int line;
    // to support lookahead
    private Token cachedNextToken;
    private int lineBackup;

    public Lexer(String chunk, String chunkName)
    {
        this.chunk = new CharSeq(chunk);
        this.chunkName = chunkName;
        this.line = 1;
    }

    public int getline()
    {
        return cachedNextToken != null ? lineBackup : line;
    }

    public T error<T>(String fmt, params Object[] args)
    {
        String msg = String.Format(fmt, args);
        msg = String.Format("{0}:{1}: {2}", chunkName, getline(), msg);
        throw new System.Exception(msg);
    }

    public TokenKind LookAhead()
    {
        if (cachedNextToken == null)
        {
            lineBackup = line;
            cachedNextToken = nextToken();
        }
        return cachedNextToken.kind;
    }

    public Token nextIdentifier()
    {
        return nextTokenOfKind(TokenKind.TOKEN_IDENTIFIER);
    }

    public Token nextTokenOfKind(TokenKind kind)
    {
        Token token = nextToken();
        if (token.kind != kind)
        {
            error<string>("syntax error near '%s'", token.value);
        }
        return token;
    }

    public Token nextToken()
    {
        if (cachedNextToken != null)
        {
            Token token = cachedNextToken;
            cachedNextToken = null;
            return token;
        }

        skipWhiteSpaces();
        if (chunk.length() <= 0)
        {
            return new Token(line, TokenKind.TOKEN_EOF, "EOF");
        }

        switch (chunk.charAt(0))
        {
            case ';': chunk.next(1); return new Token(line, TokenKind.TOKEN_SEP_SEMI, ";");
            case ',': chunk.next(1); return new Token(line, TokenKind.TOKEN_SEP_COMMA, ",");
            case '(': chunk.next(1); return new Token(line, TokenKind.TOKEN_SEP_LPAREN, "(");
            case ')': chunk.next(1); return new Token(line, TokenKind.TOKEN_SEP_RPAREN, ")");
            case ']': chunk.next(1); return new Token(line, TokenKind.TOKEN_SEP_RBRACK, "]");
            case '{': chunk.next(1); return new Token(line, TokenKind.TOKEN_SEP_LCURLY, "{");
            case '}': chunk.next(1); return new Token(line, TokenKind.TOKEN_SEP_RCURLY, "}");
            case '+': chunk.next(1); return new Token(line, TokenKind.TOKEN_OP_ADD, "+");
            case '-': chunk.next(1); return new Token(line, TokenKind.TOKEN_OP_MINUS, "-");
            case '*': chunk.next(1); return new Token(line, TokenKind.TOKEN_OP_MUL, "*");
            case '^': chunk.next(1); return new Token(line, TokenKind.TOKEN_OP_POW, "^");
            case '%': chunk.next(1); return new Token(line, TokenKind.TOKEN_OP_MOD, "%");
            case '&': chunk.next(1); return new Token(line, TokenKind.TOKEN_OP_BAND, "&");
            case '|': chunk.next(1); return new Token(line, TokenKind.TOKEN_OP_BOR, "|");
            case '#': chunk.next(1); return new Token(line, TokenKind.TOKEN_OP_LEN, "#");
            case ':':
                if (chunk.startsWith("::"))
                {
                    chunk.next(2);
                    return new Token(line, TokenKind.TOKEN_SEP_LABEL, "::");
                }
                else
                {
                    chunk.next(1);
                    return new Token(line, TokenKind.TOKEN_SEP_COLON, ":");
                }
            case '/':
                if (chunk.startsWith("//"))
                {
                    chunk.next(2);
                    return new Token(line, TokenKind.TOKEN_OP_IDIV, "//");
                }
                else
                {
                    chunk.next(1);
                    return new Token(line, TokenKind.TOKEN_OP_DIV, "/");
                }
            case '~':
                if (chunk.startsWith("~="))
                {
                    chunk.next(2);
                    return new Token(line, TokenKind.TOKEN_OP_NE, "~=");
                }
                else
                {
                    chunk.next(1);
                    return new Token(line, TokenKind.TOKEN_OP_WAVE, "~");
                }
            case '=':
                if (chunk.startsWith("=="))
                {
                    chunk.next(2);
                    return new Token(line, TokenKind.TOKEN_OP_EQ, "==");
                }
                else
                {
                    chunk.next(1);
                    return new Token(line, TokenKind.TOKEN_OP_ASSIGN, "=");
                }
            case '<':
                if (chunk.startsWith("<<"))
                {
                    chunk.next(2);
                    return new Token(line, TokenKind.TOKEN_OP_SHL, "<<");
                }
                else if (chunk.startsWith("<="))
                {
                    chunk.next(2);
                    return new Token(line, TokenKind.TOKEN_OP_LE, "<=");
                }
                else
                {
                    chunk.next(1);
                    return new Token(line, TokenKind.TOKEN_OP_LT, "<");
                }
            case '>':
                if (chunk.startsWith(">>"))
                {
                    chunk.next(2);
                    return new Token(line, TokenKind.TOKEN_OP_SHR, ">>");
                }
                else if (chunk.startsWith(">="))
                {
                    chunk.next(2);
                    return new Token(line, TokenKind.TOKEN_OP_GE, ">=");
                }
                else
                {
                    chunk.next(1);
                    return new Token(line, TokenKind.TOKEN_OP_GT, ">");
                }
            case '.':
                if (chunk.startsWith("..."))
                {
                    chunk.next(3);
                    return new Token(line, TokenKind.TOKEN_VARARG, "...");
                }
                else if (chunk.startsWith(".."))
                {
                    chunk.next(2);
                    return new Token(line, TokenKind.TOKEN_OP_CONCAT, "..");
                }
                else if (chunk.length() == 1 || !CharUtil.isDigit(chunk.charAt(1)))
                {
                    chunk.next(1);
                    return new Token(line, TokenKind.TOKEN_SEP_DOT, ".");
                }
                break;
            case '[':
                if (chunk.startsWith("[[") || chunk.startsWith("[="))
                {
                    return new Token(line, TokenKind.TOKEN_STRING, scanLongString());
                }
                else
                {
                    chunk.next(1);
                    return new Token(line, TokenKind.TOKEN_SEP_LBRACK, "[");
                }
            case '\'':
            case '"':
                return new Token(line, TokenKind.TOKEN_STRING, scanShortString());
        }

        char c = chunk.charAt(0);
        if (c == '.' || CharUtil.isDigit(c))
        {
            return new Token(line, TokenKind.TOKEN_NUMBER, scanNumber());
        }
        if (c == '_' || CharUtil.isLetter(c))
        {
            String id = scanIdentifier();
            return Token.keywords.ContainsKey(id)
                    ? new Token(line, Token.GetTokenKind(id), id)
                    : new Token(line, TokenKind.TOKEN_IDENTIFIER, id);
        }

        return error<Token>("unexpected symbol near %c", c);
    }

    private void skipWhiteSpaces()
    {
        while (chunk.length() > 0)
        {
            if (chunk.startsWith("--"))
            {
                skipComment();
            }
            else if (chunk.startsWith("\r\n") || chunk.startsWith("\n\r"))
            {
                chunk.next(2);
                line += 1;
            }
            else if (CharUtil.isNewLine(chunk.charAt(0)))
            {
                chunk.next(1);
                line += 1;
            }
            else if (CharUtil.isWhiteSpace(chunk.charAt(0)))
            {
                chunk.next(1);
            }
            else
            {
                break;
            }
        }
    }

    private void skipComment()
    {
        chunk.next(2); // skip --

        // long comment ?
        if (chunk.startsWith("["))
        {
            if (chunk.find(reOpeningLongBracket) != null)
            {
                scanLongString();
                return;
            }
        }

        // short comment
        while (chunk.length() > 0 && !CharUtil.isNewLine(chunk.charAt(0)))
        {
            chunk.next(1);
        }
    }

    private String scanIdentifier()
    {
        return scan(reIdentifier);
    }

    private String scanNumber()
    {
        return scan(reNumber);
    }

    private String scan(Regex pattern)
    {
        String token = chunk.find(pattern);
        if (token == null)
        {
            throw new System.Exception("unreachable!");
        }
        chunk.next(token.Length);
        return token;
    }

    private String scanLongString()
    {
        String openingLongBracket = chunk.find(reOpeningLongBracket);
        if (openingLongBracket == null)
        {
            return error<string>("invalid long string delimiter near '%s'",
                    chunk.substring(0, 2));
        }

        String closingLongBracket = openingLongBracket.Replace("[", "]");
        int closingLongBracketIdx = chunk.indexOf(closingLongBracket);
        if (closingLongBracketIdx < 0)
        {
            return error<string>("unfinished long string or comment");
        }

        String str = chunk.substring(openingLongBracket.Length, closingLongBracketIdx);
        chunk.next(closingLongBracketIdx + closingLongBracket.Length);

        
        //str = reNewLine.matcher(str).replaceAll("\n");
        str = reNewLine.Match(str).Value.Replace("\n", "");
        int count = 0;
        for (int i = 0; i < str.Length; ++i)
        {
            if(str[i] == '\n')
            {
                count += 1;
            }
        }
        //line += str.chars().filter(c->c == '\n').count();
        line += count;
        if (str.StartsWith("\n"))
        {
            str = str.Substring(1);
        }

        return str;
    }

    private String scanShortString()
    {
        String str = chunk.find(reShortStr);
        if (str != null)
        {
            chunk.next(str.Length);
            int count = str.Length - 1 - 1;
            str = str.Substring(1, count);
            if (str.IndexOf('\\') >= 0)
            {
                line += reNewLine.Split(str).Length - 1;
                str = new Escaper(str, this).escape();
            }
            return str;
        }
        return error<string>("unfinished string");
    }
}

