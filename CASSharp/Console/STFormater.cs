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
        public IDictionary<ESTType, ConsoleColor> TypesColores { get; } = new Dictionary<ESTType, ConsoleColor>
        {
            [ESTType.Error] = ColorError,
            [ESTType.Numeric] = ColorNumber
        };

        public SortedSet<TextFormat> Format(LinkedList<STBase> argSts)
        {
            var pFmt = new SortedSet<TextFormat>();
            var pSt = argSts.First;

            //pSt.Equals

            return pFmt;
        }
    }
}
