using System;
using System.Collections.Generic;

namespace Omicron
{
    public class Parser
    {
        private Lexer mLexer;
        private TokenType mHeadToken;
        
        private void LookAhead()
        {
            if (mLexer.Advance())
            {
                mHeadToken = mLexer.Token;
            }
            else
            {
                mHeadToken = TokenType.EOF;
            }
        }
        
        public Parser(Lexer lexer)
        {
            mLexer = lexer;
            LookAhead();
        }
        
        public IValueExpr ParseValueExpr()
        {
            if (mHeadToken == TokenType.EOF)
            {
                return null;
            }
            return ParseStatement();
        }
        
        private IValueExpr ParseStatement()
        {
            IValueExpr result = ParseExpression();
            switch (mHeadToken)
            {
            case TokenType.Semicolon:
                LookAhead();
                break;
            default:
                throw new InvalidOperationException(Expected("Semicolon"));
            }
            return result;
        }
        
        private IValueExpr ParseExpression()
        {
            return ParsePrimary();
        }
        
        private IValueExpr ParsePrimary()
        {
            IValueExpr result = ParseAtom();
            while (mHeadToken == TokenType.LeftParen ||
                   mHeadToken == TokenType.LeftBracket ||
                   mHeadToken == TokenType.Dot)
            {
                switch (mHeadToken)
                {
                case TokenType.LeftParen:
                    result = ParseApplication(result);
                    break;
                case TokenType.LeftBracket:
                    result = ParseTypeApplication(result);
                    break;
                case TokenType.Dot:
                    result = ParseObjectReference(result);
                    break;
                }
            }
            return result;
        }
        
        private IValueExpr ParseApplication(IValueExpr function)
        {
            LookAhead();
            IValueExpr argument = ParseExpression();
            if (mHeadToken != TokenType.RightParen)
            {
                throw new InvalidOperationException(Expected("RightParen"));
            }
            LookAhead();
            return new VEApp(function, argument);
        }
        
        private IValueExpr ParseTypeApplication(IValueExpr function)
        {
            LookAhead();
            ITypeExpr argument = ParseTypeLevelExpression();
            if (mHeadToken != TokenType.RightBracket)
            {
                throw new InvalidOperationException(Expected("RightBracket"));
            }
            LookAhead();
            return new VETypeApp(function, argument);
        }
        
        private IValueExpr ParseObjectReference(IValueExpr valueExpr)
        {
            LookAhead();
            if (mHeadToken != TokenType.Identifier)
            {
                throw new InvalidOperationException(Expected("Identifier"));
            }
            string methodName = (string)mLexer.Value;
            LookAhead();
            return new VERef(valueExpr, methodName);
        }
        
        private IValueExpr ParseAtom()
        {
            IValueExpr result;
            switch (mHeadToken)
            {
            case TokenType.Int:
                result = new VEConst(new VCInt((int)mLexer.Value));
                LookAhead();
                break;
            case TokenType.String:
                result = new VEConst(new VCString((string)mLexer.Value));
                LookAhead();
                break;
            case TokenType.Identifier:
                result = new VEVar((string)mLexer.Value);
                LookAhead();
                break;
            case TokenType.Hat:
                result = ParseAbstraction();
                break;
            case TokenType.Tilda:
                result = ParseTypeAbstraction();
                break;
            case TokenType.Def:
                result = ParseDefinition();
                break;
            case TokenType.Dollar:
                result = ParseObject();
                break;
            default:
                throw new InvalidOperationException(Expected());
            }
            return result;
        }
        
        private IValueExpr ParseAbstraction()
        {
            LookAhead();
            if (mHeadToken != TokenType.Identifier)
            {
                throw new InvalidOperationException(Expected("Identifier"));
            }
            string valueVarName = (string)mLexer.Value;
            LookAhead();
            if (mHeadToken != TokenType.Colon)
            {
                throw new InvalidOperationException(Expected("Colon"));
            }
            LookAhead();
            ITypeExpr typeExpr = ParseTypeLevelExpression();
            if (mHeadToken != TokenType.LeftBrace)
            {
                throw new InvalidOperationException(Expected("LeftBrace"));
            }
            LookAhead();
            IValueExpr valueExpr = ParseExpression();
            if (mHeadToken != TokenType.RightBrace)
            {
                throw new InvalidOperationException(Expected("RightBrace"));
            }
            LookAhead();
            return new VEAbs(valueVarName, typeExpr, valueExpr);
        }
        
        private IValueExpr ParseTypeAbstraction()
        {
            LookAhead();
            if (mHeadToken != TokenType.Identifier)
            {
                throw new InvalidOperationException(Expected("Identifier"));
            }
            string typeVarName = (string)mLexer.Value;
            LookAhead();
            if (mHeadToken != TokenType.Colon)
            {
                throw new InvalidOperationException(Expected("Colon"));
            }
            LookAhead();
            IKind kind = ParseKind();
            if (mHeadToken != TokenType.LeftBrace)
            {
                throw new InvalidOperationException(Expected("LeftBrace"));
            }
            LookAhead();
            IValueExpr valueExpr = ParseExpression();
            if (mHeadToken != TokenType.RightBrace)
            {
                throw new InvalidOperationException(Expected("RightBrace"));
            }
            LookAhead();
            return new VETypeAbs(typeVarName, kind, valueExpr);
        }
        
        private IValueExpr ParseDefinition()
        {
            LookAhead();
            if (mHeadToken != TokenType.Identifier)
            {
                throw new InvalidOperationException(Expected("Identifier"));
            }
            string valueVarName = (string)mLexer.Value;
            LookAhead();
            if (mHeadToken != TokenType.Equal)
            {
                throw new InvalidOperationException(Expected("Equal"));
            }
            LookAhead();
            IValueExpr valueExpr = ParseExpression();
            return new VEDef(valueVarName, valueExpr);
        }
        
        private IValueExpr ParseObject()
        {
            var methodValueExprs = new Dictionary<string, IValueExpr>();
            LookAhead();
            if (mHeadToken != TokenType.LeftBrace)
            {
                throw new InvalidOperationException(Expected("LeftBrace"));
            }
            LookAhead();
            if (mHeadToken != TokenType.RightBrace)
            {
                ParseMethodValueExpression(methodValueExprs);
                while (mHeadToken == TokenType.Comma)
                {
                    LookAhead();
                    ParseMethodValueExpression(methodValueExprs);
                }
                if (mHeadToken != TokenType.RightBrace)
                {
                    throw new InvalidOperationException(Expected("RightBrace"));
                }
            }
            LookAhead();
            return new VEObj(methodValueExprs);
        }
        
        private void ParseMethodValueExpression(
            IDictionary<string, IValueExpr> methodValueExprs
        )
        {
            if (mHeadToken != TokenType.Identifier)
            {
                throw new InvalidOperationException(Expected("Identifier"));
            }
            string methodName = (string)mLexer.Value;
            LookAhead();
            if (mHeadToken != TokenType.Equal)
            {
                throw new InvalidOperationException(Expected("Equal"));
            }
            LookAhead();
            IValueExpr methodValueExpr = ParseExpression();
            methodValueExprs.Add(methodName, methodValueExpr);
        }
        
        private IKind ParseKind()
        {
            IKind result = ParseBaseKind();
            if (mHeadToken == TokenType.RightArrow)
            {
                LookAhead();
                IKind rhs = ParseKind();
                result = new KArrow(result, rhs);
            }
            return result;
        }
        
        private IKind ParseBaseKind()
        {
            IKind result;
            switch (mHeadToken)
            {
            case TokenType.Asterisk:
                result = KCType.Instance;
                LookAhead();
                break;
            case TokenType.LeftParen:
                LookAhead();
                result = ParseKind();
                if (mHeadToken != TokenType.RightParen)
                {
                    throw new InvalidOperationException(Expected("RightParen"));
                }
                LookAhead();
                break;
            default:
                throw new InvalidOperationException(Expected("Asterisk"));
            }
            return result;
        }
        
        public ITypeExpr ParseTypeExpr()
        {
            if (mHeadToken == TokenType.EOF)
            {
                return null;
            }
            return ParseTypeLevelStatement();
        }
        
        private ITypeExpr ParseTypeLevelStatement()
        {
            ITypeExpr result = ParseTypeLevelExpression();
            switch (mHeadToken)
            {
            case TokenType.Semicolon:
                LookAhead();
                break;
            default:
                throw new InvalidOperationException(Expected("Semicolon"));
            }
            return result;
        }
        
        private ITypeExpr ParseTypeLevelExpression()
        {
            ITypeExpr result = ParseTypeLevelPrimary();
            if (mHeadToken == TokenType.RightArrow)
            {
                LookAhead();
                ITypeExpr rhs = ParseTypeLevelExpression();
                result = new TEApp(new TEApp(new TEVar("Func"), result), rhs);
            }
            return result;
        }
        
        private ITypeExpr ParseTypeLevelPrimary()
        {
            ITypeExpr result = ParseTypeLevelAtom();
            while (mHeadToken == TokenType.LeftParen)
            {
                result = ParseTypeLevelApplication(result);
            }
            return result;
        }
        
        private ITypeExpr ParseTypeLevelApplication(ITypeExpr function)
        {
            LookAhead();
            ITypeExpr argument = ParseTypeLevelExpression();
            if (mHeadToken != TokenType.RightParen)
            {
                throw new InvalidOperationException(Expected("RightParen"));
            }
            LookAhead();
            return new TEApp(function, argument);
        }
        
        private ITypeExpr ParseTypeLevelAtom()
        {
            ITypeExpr result;
            switch (mHeadToken)
            {
            case TokenType.Identifier:
                result = new TEVar((string)mLexer.Value);
                LookAhead();
                break;
            case TokenType.Hat:
                result = ParseTypeLevelAbstraction();
                break;
            case TokenType.LeftParen:
                LookAhead();
                result = ParseTypeLevelExpression();
                if (mHeadToken != TokenType.RightParen)
                {
                    throw new InvalidOperationException(Expected("RightParen"));
                }
                LookAhead();
                break;
            case TokenType.Poly:
                result = ParsePoly();
                break;
            case TokenType.Def:
                result = ParseTypeLevelDefinition();
                break;
            case TokenType.Dollar:
                result = ParseObjectType();
                break;
            default:
                throw new InvalidOperationException(Expected());
            }
            return result;
        }
        
        private ITypeExpr ParseTypeLevelAbstraction()
        {
            LookAhead();
            if (mHeadToken != TokenType.Identifier)
            {
                throw new InvalidOperationException(Expected("Identifier"));
            }
            string typeVarName = (string)mLexer.Value;
            LookAhead();
            if (mHeadToken != TokenType.Colon)
            {
                throw new InvalidOperationException(Expected("Colon"));
            }
            LookAhead();
            IKind kind = ParseKind();
            if (mHeadToken != TokenType.LeftBrace)
            {
                throw new InvalidOperationException(Expected("LeftBrace"));
            }
            LookAhead();
            ITypeExpr typeExpr = ParseTypeLevelExpression();
            if (mHeadToken != TokenType.RightBrace)
            {
                throw new InvalidOperationException(Expected("RightBrace"));
            }
            LookAhead();
            return new TEAbs(typeVarName, kind, typeExpr);
        }
        
        private ITypeExpr ParsePoly()
        {
            LookAhead();
            if (mHeadToken != TokenType.LeftBracket)
            {
                throw new InvalidOperationException(Expected("LeftBracket"));
            }
            LookAhead();
            IKind kind = ParseKind();
            if (mHeadToken != TokenType.RightBracket)
            {
                throw new InvalidOperationException(Expected("RightBracket"));
            }
            LookAhead();
            return new TEPoly(kind);
        }
        
        private ITypeExpr ParseTypeLevelDefinition()
        {
            LookAhead();
            if (mHeadToken != TokenType.Identifier)
            {
                throw new InvalidOperationException(Expected("Identifier"));
            }
            string typeVarName = (string)mLexer.Value;
            LookAhead();
            if (mHeadToken != TokenType.Equal)
            {
                throw new InvalidOperationException(Expected("Equal"));
            }
            LookAhead();
            ITypeExpr typeExpr = ParseTypeLevelExpression();
            return new TEDef(typeVarName, typeExpr);
        }
        
        private ITypeExpr ParseObjectType()
        {
            var methodTypeExprs = new Dictionary<string, ITypeExpr>();
            LookAhead();
            if (mHeadToken != TokenType.LeftBrace)
            {
                throw new InvalidOperationException(Expected("LeftBrace"));
            }
            LookAhead();
            if (mHeadToken != TokenType.RightBrace)
            {
                ParseMethodTypeExpression(methodTypeExprs);
                while (mHeadToken == TokenType.Comma)
                {
                    LookAhead();
                    ParseMethodTypeExpression(methodTypeExprs);
                }
                if (mHeadToken != TokenType.RightBrace)
                {
                    throw new InvalidOperationException(Expected("RightBrace"));
                }
            }
            LookAhead();
            return new TEObj(methodTypeExprs);
        }
        
        private void ParseMethodTypeExpression(
            IDictionary<string, ITypeExpr> methodTypeExprs
        )
        {
            if (mHeadToken != TokenType.Identifier)
            {
                throw new InvalidOperationException(Expected("Identifier"));
            }
            string methodName = (string)mLexer.Value;
            LookAhead();
            if (mHeadToken != TokenType.Colon)
            {
                throw new InvalidOperationException(Expected("Colon"));
            }
            LookAhead();
            ITypeExpr methodTypeExpr = ParseTypeLevelExpression();
            methodTypeExprs.Add(methodName, methodTypeExpr);
        }
        
        private string Expected()
        {
            return string.Format("unexpected {0}", mHeadToken);
        }
        
        private string Expected(string expected)
        {
            return string.Format(
                "unexpected {0}, expected {1}", mHeadToken, expected
            );
        }
    }
}
