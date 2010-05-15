using System;
using System.IO;
using Omicron;

class Program
{
    static void Main()
    {
        REPL();
    }
    
    private static void REPL()
    {
        ITypeCtxt typeCtxt = new TypeCtxtGlobal();
        ITypeEnv typeEnv = new TypeEnvGlobal();
        IValueCtxt valueCtxt = new ValueCtxtGlobal();
        IValueEnv valueEnv = new ValueEnvGlobal();
        bool valueMode = true;
        Load("omicron-init.om", typeCtxt, typeEnv, valueCtxt, valueEnv);
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
                Load(fileName, typeCtxt, typeEnv, valueCtxt, valueEnv);
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
                InterpretValueExpr(
                    reader, true, typeCtxt, typeEnv, valueCtxt, valueEnv
                );
            }
            else
            {
                InterpretTypeExpr(
                    reader, true, typeCtxt, typeEnv
                );
            }
        }
    }
    
    private static void Load(
        string fileName,
        ITypeCtxt typeCtxt,
        ITypeEnv typeEnv,
        IValueCtxt valueCtxt,
        IValueEnv valueEnv
    )
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
            InterpretValueExpr(
                reader, false, typeCtxt, typeEnv, valueCtxt, valueEnv
            );
        }
    }
    
    private static void InterpretValueExpr(
        TextReader reader,
        bool interactive,
        ITypeCtxt typeCtxt,
        ITypeEnv typeEnv,
        IValueCtxt valueCtxt,
        IValueEnv valueEnv
    )
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
                IType type = valueExpr.Check(typeCtxt, typeEnv, valueCtxt);
                IValue value = valueExpr.Eval(valueEnv);
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
    
    private static void InterpretTypeExpr(
        TextReader reader,
        bool interactive,
        ITypeCtxt typeCtxt,
        ITypeEnv typeEnv
    )
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
                IKind kind = typeExpr.Check(typeCtxt);
                IType type = typeExpr.Eval(typeEnv);
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
