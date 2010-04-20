namespace Omicron
{
    public enum TokenType
    {
        EOF = -1,
        Int = -2,
        String = -3,
        Identifier = -4,
        
        Poly = -5,
        Def = -6,
        
        RightArrow = -7,
        
        LeftParen = 40,
        RightParen = 41,
        Asterisk = 42,
        Minus = 45,
        Colon = 58,
        Semicolon = 59,
        Equal = 61,
        LeftBracket = 91,
        RightBracket = 93,
        Hat = 94,
        LeftBrace = 123,
        RightBrace = 125,
        Tilda = 126,
    }
}
