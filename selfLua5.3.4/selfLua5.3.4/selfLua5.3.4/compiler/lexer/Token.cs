using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Token
{
    static Dictionary<String, TokenKind> keywords = new Dictionary<String, TokenKind>();

    static int Init()
    {
        keywords.Add("and", TokenKind.TOKEN_OP_AND);
        keywords.Add("break", TokenKind.TOKEN_KW_BREAK);
        keywords.Add("do", TokenKind.TOKEN_KW_DO);
        keywords.Add("else", TokenKind.TOKEN_KW_ELSE);
        keywords.Add("elseif", TokenKind.TOKEN_KW_ELSEIF);
        keywords.Add("end", TokenKind.TOKEN_KW_END);
        keywords.Add("false", TokenKind.TOKEN_KW_FALSE);
        keywords.Add("for", TokenKind.TOKEN_KW_FOR);
        keywords.Add("function", TokenKind.TOKEN_KW_FUNCTION);
        keywords.Add("goto", TokenKind.TOKEN_KW_GOTO);
        keywords.Add("if", TokenKind.TOKEN_KW_IF);
        keywords.Add("in", TokenKind.TOKEN_KW_IN);
        keywords.Add("local", TokenKind.TOKEN_KW_LOCAL);
        keywords.Add("nil", TokenKind.TOKEN_KW_NIL);
        keywords.Add("not", TokenKind.TOKEN_OP_NOT);
        keywords.Add("or", TokenKind.TOKEN_OP_OR);
        keywords.Add("repeat", TokenKind.TOKEN_KW_REPEAT);
        keywords.Add("return", TokenKind.TOKEN_KW_RETURN);
        keywords.Add("then", TokenKind.TOKEN_KW_THEN);
        keywords.Add("true", TokenKind.TOKEN_KW_TRUE);
        keywords.Add("until", TokenKind.TOKEN_KW_UNTIL);
        keywords.Add("while", TokenKind.TOKEN_KW_WHILE);

        return 0;
    }

    static int tmp = Init();

    private int line;
    private TokenKind kind;
    private String value;

    public Token(int line, TokenKind kind, String value)
    {
        this.line = line;
        this.kind = kind;
        this.value = value;
    }
}

