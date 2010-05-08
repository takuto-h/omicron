namespace Omicron
{
    public enum TokenType
    {
        EOF = -1,
        Int = -2,
        String = -3,
        Identifier = -4,
        
        Poly = -5,
        Rec = -6,
        Def = -7,
        Fold = -8,
        Unfold = -9,
        
        RightArrow = -10,
        
        Dollar = 36,
        LeftParen = 40,
        RightParen = 41,
        Asterisk = 42,
        Comma = 44,
        Minus = 45,
        Dot = 46,
        Colon = 58,
        Semicolon = 59,
        Equal = 61,
        AtMark = 64,
        LeftBracket = 91,
        RightBracket = 93,
        Hat = 94,
        LeftBrace = 123,
        RightBrace = 125,
        Tilda = 126,
    }
}
