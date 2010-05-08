using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Omicron
{
    public class Lexer
    {
        private static Dictionary<string, TokenType> mReserved;
        
        static Lexer()
        {
            mReserved = new Dictionary<string, TokenType>();
            mReserved.Add("Poly", TokenType.Poly);
            mReserved.Add("Rec", TokenType.Rec);
            mReserved.Add("def", TokenType.Def);
        }
        
        private TextReader mReader;
        
        public TokenType Token { get; private set; }
        public object Value { get; private set; }
        
        public Lexer(TextReader reader)
        {
            mReader = reader;
        }
        
        public bool Advance()
        {
            SkipWhiteSpace();
            int c = mReader.Peek();
            if (c == -1)
            {
                return false;
            }
            switch (c)
            {
            case '$':
            case '(':
            case ')':
            case '*':
            case ',':
            case '.':
            case ':':
            case ';':
            case '=':
            case '@':
            case '[':
            case ']':
            case '^':
            case '{':
            case '}':
            case '~':
                Token = (TokenType)mReader.Read();
                break;
            case '-':
                mReader.Read();
                if (mReader.Peek() == '>')
                {
                    mReader.Read();
                    Token = TokenType.RightArrow;
                }
                else
                {
                    Token = TokenType.Minus;
                }
                break;
            case '"':
                LexString();
                break;
            default:
                if (char.IsDigit((char)c))
                {
                    LexInt();
                }
                else if (IsIdentifierStart((char)c))
                {
                    LexIdentifier();
                }
                else
                {
                    throw new InvalidOperationException(
                        string.Format("unknown character: {0}", (char)c)
                    );
                }
                break;
            }
            return true;
        }
        
        private bool IsIdentifierStart(char c)
        {
            return char.IsLetter(c) || c == '_';
        }
        
        private bool IsIdentifierPart(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }
        
        private void LexString()
        {
            mReader.Read();
            StringBuilder sb = new StringBuilder();
            int c = mReader.Peek();
            while (c != '"')
            {
                if (c == -1)
                {
                    throw new InvalidOperationException(
                        string.Format("EOF inside a string")
                    );
                }
                sb.Append((char)mReader.Read());
                c = mReader.Peek();
            }
            mReader.Read();
            Token = TokenType.String;
            Value = sb.ToString();
        }
        
        private void LexInt()
        {
            int num = mReader.Read() - '0';
            int c = mReader.Peek();
            while (c != -1 && char.IsDigit((char)c))
            {
                num = num * 10 + mReader.Read() - '0';
                c = mReader.Peek();
            }
            Token = TokenType.Int;
            Value = num;
        }
        
        private void LexIdentifier()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((char)mReader.Read());
            int c = mReader.Peek();
            while (c != -1 && IsIdentifierPart((char)c))
            {
                sb.Append((char)mReader.Read());
                c = mReader.Peek();
            }
            TokenType token;
            string identifier = sb.ToString();
            if (mReserved.TryGetValue(identifier, out token))
            {
                Token = token;
            }
            else
            {
                Token = TokenType.Identifier;
                Value = identifier;
            }
        }
        
        private void SkipWhiteSpace()
        {
            int c = mReader.Peek();
            while (c != -1 && char.IsWhiteSpace((char)c))
            {
                mReader.Read();
                c = mReader.Peek();
            }
        }
    }
}
