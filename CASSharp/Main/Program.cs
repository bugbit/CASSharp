using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using static System.Console;

namespace CASSharp.Main
{
    static class Program
    {
        static readonly Param[] mParams = new[]
        {
            new Param(Properties.Resources.HelpParamDescr,Help,"/?","-help","--h"),
#if DEBUG
            new Param(Properties.Resources.TestParamDescr,Test,"/test","/t")
#endif
        };

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static int Main(string[] args)
        {
            PrintHeader();
            WriteLine();
            try
            {
                ParseCommandLine(args, out bool pExit);
                if (pExit)
                    return 0;
            }
            catch (Exception ex)
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine($"Error: {ex.Message}");

                return -1;
            }

            System.ReadLine.Read("(%1) ");

            return 0;
        }

        static private void PrintHeader()
        {
            var pAssembly = Assembly.GetExecutingAssembly();
            var pAttrs = pAssembly.GetCustomAttributes(false);
            var pName = pAttrs.OfType<AssemblyTitleAttribute>().First().Title;
            var pVersion = pAssembly.GetName().Version.ToString();
            var pDescription = pAttrs.OfType<AssemblyDescriptionAttribute>().First().Description;
            var pLicense = pAttrs.OfType<AssemblyCopyrightAttribute>().First().Copyright;

            Title = $"{pName} {pVersion}";
            WriteLine
            (
                 $@"
{pName} Version {pVersion}
{pDescription}
https://github.com/bugbit/cassharp

{pLicense}
MIT LICENSE"
            );
        }

        private static void ParseCommandLine(string[] args, out bool argExit)
        {
            var i = 0;

            argExit = false;
            while (i < args.Length)
            {
                var pKey = args[i++];
                var pParam = mParams.FirstOrDefault(p => p.Keys.Contains(pKey, StringComparer.InvariantCultureIgnoreCase));

                if (pParam == null)
                    throw new Exception(string.Format(Properties.Resources.NoRecognizeParamError, pKey));

                pParam.Action.Invoke(pKey, args, ref i, out argExit);

                if (argExit)
                    return;
            }
        }

        private static void Help(string argKey, string[] args, ref int i, out bool argExit)
        {
            argExit = true;
            WriteLine("Usage:\nCASSharp [flags]\nFlags:\n");

            foreach (var pParam in mParams)
            {
                var pKeys = string.Join("|", pParam.Keys);
                var pTabs = new string('\t', 4 - (pKeys.Length / 8));

                WriteLine($"  {pKeys}{pTabs}{pParam.IRDescription}");
            }
        }

#if DEBUG
        private static void Test(string argKey, string[] args, ref int i, out bool argExit)
        {
            argExit = true;

            var pTests = typeof(CASAppConsole).Assembly.GetCustomAttributes(true).OfType<Tests.TestAttribute>().OrderBy(a => a.Order);

            foreach (var t in pTests)
            {
                var pTest = Activator.CreateInstance(t.TestType);

                (pTest as Tests.ITest)?.Run();
            }
        }
#endif
    }
}
