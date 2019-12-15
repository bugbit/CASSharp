using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CASSharp.Core.CAS
{
    public class EvalPromptResult
    {
        public EvalExprInResult[] EvalResults { get; set; }
        public string[] LinesNoParse { get; set; }
    }
}
