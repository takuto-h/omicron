using System;
using System.IO;
using Omicron;

class Program
{
    static void Main()
    {
        //TestTypeChecking();
        REPL();
    }
    
    private static void REPL()
    {
        ITypeCtxt typeCtxt = new TypeCtxtGlobal();
        ITypeEnv typeEnv = new TypeEnvGlobal();
        IValueCtxt valueCtxt = new ValueCtxtGlobal();
        IValueEnv valueEnv = new ValueEnvGlobal();
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
            try
            {
                Lexer lexer = new Lexer(reader);
                Parser parser = new Parser(lexer);
                while (true)
                {
                    if (valueMode)
                    {
                        IValueExpr valueExpr = parser.ParseValueExpr();
                        if (valueExpr == null)
                        {
                            break;
                        }
                        IType type = valueExpr.Check(
                            typeCtxt, typeEnv, valueCtxt
                        );
                        IValue value = valueExpr.Eval(valueEnv);
                        Console.WriteLine(
                            "- : {0} = {1}", type.Show(), value.Show()
                        );
                    }
                    else
                    {
                        ITypeExpr typeExpr = parser.ParseTypeExpr();
                        if (typeExpr == null)
                        {
                            break;
                        }
                        IKind kind = typeExpr.Check(typeCtxt);
                        IType type = typeExpr.Eval(typeEnv);
                        Console.WriteLine(
                            "- : {0} = {1}", kind.Show(), type.Show()
                        );
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
        }
    }
    
    private static void TestTypeChecking()
    {
        ITypeCtxt typeCtxt = new TypeCtxtGlobal();
        ITypeEnv typeEnv = new TypeEnvGlobal();
        IValueCtxt valueCtxt = new ValueCtxtGlobal();
        TestTypeExpr(typeCtxt, typeEnv);
        TestValueExpr(typeCtxt, typeEnv, valueCtxt);
    }
    
    private static void TestTypeExpr(
        ITypeCtxt typeCtxt,
        ITypeEnv typeEnv
    )
    {
        ITypeExpr teInt = new TEVar("Int");
        ITypeExpr teString = new TEVar("String");
        ITypeExpr teFunc = new TEVar("Func");
        ITypeExpr teFuncInt = new TEApp(teFunc, teInt);
        ITypeExpr teFuncIntString = new TEApp(teFuncInt, teString);
        
        ITypeExpr teId =
            new TEApp(new TEPoly(KCType.Instance),
                new TEAbs("A", KCType.Instance,
                    new TEApp(new TEApp(new TEVar("Func"), new TEVar("A")),
                              new TEVar("A"))));
        
        ITypeExpr teConst =
            new TEApp(new TEPoly(KCType.Instance),
                new TEAbs("A", KCType.Instance,
                    new TEApp(new TEPoly(KCType.Instance),
                        new TEAbs("B", KCType.Instance,
                            new TEApp(
                                new TEApp(new TEVar("Func"), new TEVar("A")),
                                new TEApp(
                                    new TEApp(new TEVar("Func"), new TEVar("B")),
                                    new TEVar("A")))))));
        
        CheckAndEvalTypeExpr(typeCtxt, typeEnv, teInt);
        CheckAndEvalTypeExpr(typeCtxt, typeEnv, teString);
        CheckAndEvalTypeExpr(typeCtxt, typeEnv, teFunc);
        CheckAndEvalTypeExpr(typeCtxt, typeEnv, teFuncInt);
        CheckAndEvalTypeExpr(typeCtxt, typeEnv, teFuncIntString);
        CheckAndEvalTypeExpr(typeCtxt, typeEnv, teId);
        CheckAndEvalTypeExpr(typeCtxt, typeEnv, teConst);
    }
    
    private static void CheckAndEvalTypeExpr(
        ITypeCtxt typeCtxt,
        ITypeEnv typeEnv,
        ITypeExpr typeExpr
    )
    {
        IKind kind = typeExpr.Check(typeCtxt);
        IType type = typeExpr.Eval(typeEnv);
        Console.WriteLine(
            "{0} : {1} = {2}", typeExpr.Show(), kind.Show(), type.Show()
        );
    }
    
    private static void TestValueExpr(
        ITypeCtxt typeCtxt,
        ITypeEnv typeEnv,
        IValueCtxt valueCtxt
    )
    {
        IValueExpr veOne = new VEConst(new VCInt(1));
        IValueExpr veTwo = new VEConst(new VCInt(2));
        IValueExpr veABC = new VEConst(new VCString("abc"));
        IValueExpr veDEF = new VEConst(new VCString("def"));
        
        IValueExpr veId =
            new VETypeAbs("A", KCType.Instance,
                new VEAbs("x", new TEVar("A"),
                    new VEVar("x")));
        IValueExpr veIdInt = new VETypeApp(veId, new TEVar("Int"));
        IValueExpr veIdIntOne = new VEApp(veIdInt, veOne);
        
        IValueExpr veConst =
            new VETypeAbs("A", KCType.Instance,
                new VETypeAbs("B", KCType.Instance,
                    new VEAbs("x", new TEVar("A"),
                        new VEAbs("y", new TEVar("B"),
                            new VEVar("x")))));
        IValueExpr veConstInt =
            new VETypeApp(veConst, new TEVar("Int"));
        IValueExpr veConstIntString =
            new VETypeApp(veConstInt, new TEVar("String"));
        IValueExpr veConstIntStringOne =
            new VEApp(veConstIntString, veOne);
        IValueExpr veConstIntStringOneABC =
            new VEApp(veConstIntStringOne, veABC);
        
        CheckValueExpr(typeCtxt, typeEnv, valueCtxt, veOne);
        CheckValueExpr(typeCtxt, typeEnv, valueCtxt, veTwo);
        CheckValueExpr(typeCtxt, typeEnv, valueCtxt, veABC);
        CheckValueExpr(typeCtxt, typeEnv, valueCtxt, veDEF);
        CheckValueExpr(typeCtxt, typeEnv, valueCtxt, veId);
        CheckValueExpr(typeCtxt, typeEnv, valueCtxt, veIdInt);
        CheckValueExpr(typeCtxt, typeEnv, valueCtxt, veIdIntOne);
        CheckValueExpr(typeCtxt, typeEnv, valueCtxt, veConst);
        CheckValueExpr(typeCtxt, typeEnv, valueCtxt, veConstInt);
        CheckValueExpr(typeCtxt, typeEnv, valueCtxt, veConstIntString);
        CheckValueExpr(typeCtxt, typeEnv, valueCtxt, veConstIntStringOne);
        CheckValueExpr(typeCtxt, typeEnv, valueCtxt, veConstIntStringOneABC);
    }
    
    private static void CheckValueExpr(
        ITypeCtxt typeCtxt,
        ITypeEnv typeEnv,
        IValueCtxt valueCtxt,
        IValueExpr valueExpr
    )
    {
        IType type = valueExpr.Check(typeCtxt, typeEnv, valueCtxt);
        Console.WriteLine("{0} : {1}", valueExpr.Show(), type.Show());
    }
}
