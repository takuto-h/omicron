using System;
using Omicron;

class Program
{
    private static void Usage()
    {
        Console.Error.WriteLine("usage: omicron [file]");
    }
    
    static void Main(string[] args)
    {
        Interpreter interp = new Interpreter();
        interp.InterpretFile("omicron-init.om");
        switch (args.Length)
        {
        case 0:
            interp.InterpretInteractively();
            break;
        case 1:
            interp.InterpretFile(args[0]);
            break;
        default:
            Usage();
            break;
        }
    }
}
