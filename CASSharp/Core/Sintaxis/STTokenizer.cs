﻿#region LICENSE
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

namespace CASSharp.Core.Sintaxis
{
    class STTokenizer
    {
        public LinkedList<STBase> Parse(string argText)
        {
            var pSTs = new LinkedList<STBase>();
            var i = 0;

            while (i < argText.Length)
                ParseToken(pSTs, argText, ref i);

            return pSTs;
        }

        private void ParseToken(LinkedList<STBase> argSts, string argText, ref int i)
        {
            while (i < argText.Length && char.IsWhiteSpace(argText[i]))
                i++;

            if (i >= argText.Length)
                return;

            int pIni, pFin;

            pIni = pFin = i;

            if (char.IsDigit(argText[i]))
            {
                do pFin = i++; while (i < argText.Length && char.IsDigit(argText[i]));

                argSts.AddLast(STBase.CreateByTheText(ESTType.Numeric, pIni, pFin, argText));
            }
            else
            {
                do pFin = i++; while (i < argText.Length && !char.IsWhiteSpace(argText[i]));

                argSts.AddLast(STError.CreateByTheText(pIni, pFin, argText, Properties.Resources.NoRecognizeStError));
            }
        }
    }
}