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
using System.Text.RegularExpressions;
using System.Threading;

namespace CASSharp.Core.Syntax
{
    public class STTokenizer
    {
        private const string grNumber = "number";
        private const string grSep = "sep";
        private static readonly Regex mRegExTokens = new Regex(@"(\s*)(?<" + grNumber + @">\d+)|(?<word>\w+)|(?<" + grSep + @">[;$])");

        private CancellationToken mCancelToken;
        private string[] mLines;
        private string mText;
        private Match mMatch = null;
        private int mLine = 0;
        private int mPosition = 0;
        private char mChar = STTokenChars.Null;

        public STTokenizer() { }

        public STTokenizer(string[] argLineas, CancellationToken argCancelToken)
        {
            mLines = argLineas;
            mCancelToken = argCancelToken;
        }

        public static STTokenizerResult Parse(string[] argLineas, CancellationToken argCancelToken)
        {
            var pTokenizer = new STTokenizer(argLineas, argCancelToken);
            var pRet = pTokenizer.Parse();

            return pRet;
        }

        private STTokenizerResult Parse()
        {
            if (mLines == null || mLines.Length < 1)
                return null;

            mLine = 0;
            ReadLineTokens(mLines.First());
            //mLine = mPosition = 0;
            //mText = mLines.First();
            //ReadChar();
            ParseLines(out STTokensTerminate[] pTokensOut, out string[] pLinesNoParse);

            return new STTokenizerResult { TokensOut = pTokensOut, LinesNoParse = pLinesNoParse };
        }

        private void ParseLines(out STTokensTerminate[] argTokensOut, out string[] argLinesNoParse)
        {
            var pTokensOut = new List<STTokensTerminate>();

            argLinesNoParse = mLines;
            do
            {
                mCancelToken.ThrowIfCancellationRequested();

                if (!ParseTokensTerminate(out STTokensTerminate pTokens, out string[] pLinesNoParse))
                {
                    argLinesNoParse = pLinesNoParse;

                    break;
                }
                pTokensOut.Add(pTokens);
            } while (mChar != STTokenChars.Null);

            argTokensOut = pTokensOut.ToArray();
        }

        private bool ParseTokensTerminate(out STTokensTerminate argTokens, out string[] argLinesNoParse)
        {
            var pLinea0 = mLine;
            var pPos0 = mPosition;

            argTokens = ParseTokens<STTokensTerminate>();
            argTokens.TerminateChar = mChar;

            if (mChar == STTokenChars.Null)
            {
                var pLines = new List<string>();

                argTokens.Terminate = ESTTokenizerTerminate.No;
                if (pLinea0 < mLines.Length && pPos0 < mLines[pLinea0].Length)
                    pLines.Add(mLines[pLinea0].Substring(pPos0));
                pLines.AddRange(mLines.Skip(pLinea0 + 1));

                argLinesNoParse = pLines.ToArray();

                return false;
            }

            argLinesNoParse = null;
            switch (mChar)
            {
                case STTokenChars.Terminate:
                case STTokenChars.TerminateNoShowOut:
                    return true;
                default:
                    throw new STException(string.Format(Properties.Resources.NoExpectTokenException, mChar), mLine, mPosition);
            }
        }

        private T ParseTokens<T>() where T : STTokens, new()
        {
            var pTokens = new T();

            for (; ; )
            {
                mCancelToken.ThrowIfCancellationRequested();

                var pToken = ParseToken();

                if (pToken == null)
                    return pTokens;

                if (pTokens.Tokens == null)
                    pTokens.Tokens = new LinkedList<STToken>();

                pTokens.Tokens.AddLast(pToken);
            }
        }

        private STToken ParseToken()
        {
            do
            {
                mCancelToken.ThrowIfCancellationRequested();

                if (mMatch.Success)
                {
                    if (mMatch.Index != mPosition)
                        throw new STException(string.Format(Properties.Resources.NoRecognizeStError, mText[mPosition]), mLine, mPosition);

                    var pGr = mMatch.Groups[1];

                    if (pGr.Success)
                        mPosition = pGr.Index + pGr.Length;

                    pGr = null;
                    STToken pSt = null;
                    char pCar = STTokenChars.Null;

                    if (mMatch.Groups[grNumber].Success)
                    {
                        pGr = mMatch.Groups[grNumber];
                        pSt = new STTokenStr { Token = ESTToken.Numeric, Position = pGr.Index, Text = pGr.Value };
                    }
                    else if (mMatch.Groups[grSep].Success)
                    {
                        pGr = mMatch.Groups[grSep];
                        pCar = pGr.Value[0];
                    }
                    else
                        throw new STException(string.Format(Properties.Resources.NoRecognizeStError, mText[mPosition]), mLine, mPosition);

                    if (pGr != null)
                        mPosition = pGr.Index + pGr.Length;
                    mChar = pCar;
                    mMatch = mMatch.NextMatch();

                    return pSt;
                }
            } while (ReadNextLine());

            mChar = STTokenChars.Null;

            return null;
        }

        private void ReadLineTokens(string argText)
        {
            mText = argText;
            mMatch = mRegExTokens.Match(argText);
            mPosition = 0;
            mChar = STTokenChars.Null;
        }

        private bool ReadNextLine()
        {
            if (mLine + 1 >= mLines.Length)
                return false;

            ReadLineTokens(mLines[++mLine]);

            return true;
        }
    }
}
