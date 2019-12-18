#region LICENSE
/*
    MIT License

    Copyright (c) 2018 Software free CAS Sharp

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.

*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CASSharp.Core.Syntax
{
    public class STTokens
    {
        public LinkedList<STToken> Tokens { get; set; }

        public override string ToString() //=> (Tokens != null) ? string.Join(" ", Tokens) : string.Empty;
        {
            var pStr = string.Empty;
            var pReader = new STTokensReader(this, CancellationToken.None);

            while (!pReader.EOF)
            {
                if (pReader.IsPrev(2, n => n.Token == ESTToken.Number || n.Token == ESTToken.Word))
                    pStr += " ";
                pStr += pReader.Token.ToString();
                pReader.Next();
            }

            return pStr;

            /*
            var pTokens = Tokens.First;

            while (pTokens != null)
            {
                if (pTokens.Previous != null)
                    switch (pTokens.Previous.Value.Token)
                    {
                        case ESTToken.Number:
                        case ESTToken.Word:
                            switch (pTokens.Value.Token)
                            {
                                case ESTToken.Number:
                                case ESTToken.Word:
                                    pStr += " ";
                                    break;
                            }
                            break;
                    }
                pStr += pTokens.Value.ToString();
                pTokens = pTokens.Next;
            }

            return pStr;
            */
        }
    }
}
