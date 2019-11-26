using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CASSharp.Core.Sintaxis;

namespace CASSharp.Console
{
    class STFormater
    {
        public const ConsoleColor ColorError = ConsoleColor.Red;
        public const ConsoleColor ColorNumber = ConsoleColor.DarkBlue;
        public static IDictionary<ESTType, ConsoleColor> TypesColores { get; } = new Dictionary<ESTType, ConsoleColor>
        {
            [ESTType.Error] = ColorError,
            [ESTType.Numeric] = ColorNumber
        };

        public SortedSet<TextFormat> Format(LinkedList<STBase> argSts)
        {
            var pFmt = new SortedSet<TextFormat>();

            Format(pFmt, argSts);

            return pFmt;
        }

        private void Format(SortedSet<TextFormat> argFmt, LinkedList<STBase> argSts)
        {
            foreach (var pSt in argSts)
            {
                if (TypesColores.TryGetValue(pSt.Type, out ConsoleColor argColor))
                    argFmt.Add(new TextFormat { Position = pSt.Position, BackColor = argColor });
            }
        }
    }
}
