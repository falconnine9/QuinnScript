using System;
using System.IO;

using QuinnScript.Compiler;
using QuinnScript.Helpers;
using QuinnScript.VM;

namespace QuinnScript;

class QuinnScript
{
    public static void Main(string[] args)
    {
        if (args.Length == 0) {
            Logger.LogFatal("No input file provided");
            return;
        }
        if (!File.Exists(args[0])) {
            Logger.LogFatal("Cannot find the input file");
            return;
        }

        string source = File.ReadAllText(args[0]);

        try {
            Executor.Code = CompilerMain.CompileSource(args[0], source);
            VariableConstants.SetConstantVariables();
            Executor.BeginCycle();
        }
        catch (Exception e) {
            Logger.LogFatal(e.ToString());
        }
    }
}
