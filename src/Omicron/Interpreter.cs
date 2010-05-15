using System;
using System.IO;

namespace Omicron
{
    public class Interpreter
    {
        private ITypeCtxt mTypeCtxt;
        private ITypeEnv mTypeEnv;
        private IValueCtxt mValueCtxt;
        private IValueEnv mValueEnv;
        
        public Interpreter()
        {
            mTypeCtxt = new TypeCtxtGlobal();
            mTypeEnv = new TypeEnvGlobal();
            mValueCtxt = new ValueCtxtGlobal();
            mValueEnv = new ValueEnvGlobal();
        }
        
        public void InterpretInteractively()
        {
            bool valueMode = true;
            while (true)
            {
                if (valueMode)
                {
                    Console.Write("v> ");
                }
                else
                {
                    Console.Write("t> ");
                }
                string input = Console.ReadLine();
                if (input == ":q")
                {
                    break;
                }
                else if (input == ":l")
                {
                    Console.Write("l> ");
                    string fileName = Console.ReadLine();
                    InterpretFile(fileName);
                    continue;
                }
                else if (input == ":v")
                {
                    valueMode = true;
                    continue;
                }
                else if (input == ":t")
                {
                    valueMode = false;
                    continue;
                }
                TextReader reader = new StringReader(input);
                if (valueMode)
                {
                    InterpretValueExpr(reader, true);
                }
                else
                {
                    InterpretTypeExpr(reader, true);
                }
            }
        }
        
        public void InterpretFile(string fileName)
        {
            if (fileName == "")
            {
                return;
            }
            else if (!File.Exists(fileName))
            {
                Console.Error.WriteLine("file not found: {0}", fileName);
                return;
            }
            using (TextReader reader = new StreamReader(fileName))
            {
                InterpretValueExpr(reader, false);
            }
        }
        
        public void InterpretValueExpr(TextReader reader, bool interactive)
        {
            try
            {
                Lexer lexer = new Lexer(reader);
                Parser parser = new Parser(lexer);
                while (true)
                {
                    IValueExpr valueExpr = parser.ParseValueExpr();
                    if (valueExpr == null)
                    {
                        break;
                    }
                    IType type = valueExpr.Check(
                        mTypeCtxt, mTypeEnv, mValueCtxt
                    );
                    IValue value = valueExpr.Eval(mValueEnv);
                    if (interactive)
                    {
                        Console.WriteLine(
                            "- : {0} = {1}", type.Show(), value.Show()
                        );
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR: {0}", e.Message);
            }
        }
        
        public void InterpretTypeExpr(TextReader reader, bool interactive)
        {
            try
            {
                Lexer lexer = new Lexer(reader);
                Parser parser = new Parser(lexer);
                while (true)
                {
                    ITypeExpr typeExpr = parser.ParseTypeExpr();
                    if (typeExpr == null)
                    {
                        break;
                    }
                    IKind kind = typeExpr.Check(mTypeCtxt);
                    IType type = typeExpr.Eval(mTypeEnv);
                    if (interactive)
                    {
                        Console.WriteLine(
                            "- : {0} = {1}", kind.Show(), type.Show()
                        );
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR: {0}", e.Message);
            }
        }
    }
}
