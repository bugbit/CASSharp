using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CASSharp.Core.Sintaxis
{
    class STTokenizerResult
    {
        public ESTTokenizerTerminate Terminate { get; set; } = ESTTokenizerTerminate.No;
        public LinkedList<STBase> Sintaxis { get; set; }
        public string PromptNoParse { get; set; }
    }
}
