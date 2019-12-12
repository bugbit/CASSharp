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
using System.Threading;

namespace CASSharp.Core.Syntax
{
    public class STTokenizer
    {
        private CancellationToken mCancelToken;
        private string[] mLines;
        private string mText;
        private int mLine = 0;
        private int mLineAnt = 0;
        private int mPosition = 0;
        private int mPositionAnt = 0;
        private char mLastChar = STTokenChars.Null;

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
            mPosition = -1;
            mText = mLines.First();
            ReadChar();
            ParseLines(out STTokensTerminate[] pTokensOut, out string[] pLinesNoParse);

            return new STTokenizerResult { TokensOut = pTokensOut, LinesNoParse = pLinesNoParse };
        }

        private void ParseLines(out STTokensTerminate[] argTokensOut, out string[] argLinesNoParse)
        {
            var pTokensOut = new List<STTokensTerminate>();

            argLinesNoParse = mLines;
            while (mLastChar != STTokenChars.Null)
            {
                mCancelToken.ThrowIfCancellationRequested();

                if (!ParseTokensTerminate(out STTokensTerminate pTokens, out string[] pLinesNoParse))
                {
                    argLinesNoParse = pLinesNoParse;

                    break;
                }
                pTokensOut.Add(pTokens);
            }

            argTokensOut = pTokensOut.ToArray();
        }

        private bool ParseTokensTerminate(out STTokensTerminate argTokens, out string[] argLinesNoParse)
        {
            var pLinea0 = mLine;
            var pPos0 = mPosition;

            argTokens = ParseTokens<STTokensTerminate>();
            argTokens.TerminateChar = mLastChar;

            if (mLastChar == STTokenChars.Null)
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
            switch (mLastChar)
            {
                case STTokenChars.Terminate:
                case STTokenChars.TerminateNoShowOut:
                    return true;
                default:
                    throw new STException(string.Format(Properties.Resources.NoExpectTokenException, mLastChar), mLineAnt, mPositionAnt);
            }
        }

        private T ParseTokens<T>() where T : STTokens, new()
        {
            var pTokens = new T();

            for (; ; )
            {
                mCancelToken.ThrowIfCancellationRequested();

                if (mLastChar == STTokenChars.Null)
                    return pTokens;

                ParseToken(out STToken pToken, out char pCar);
                if (pToken == null)
                    return pTokens;

                if (pTokens.Tokens == null)
                    pTokens.Tokens = new LinkedList<STToken>();

                pTokens.Tokens.AddLast(pToken);
            }
        }

        private bool ParseToken(out STToken argToken, out char argCar)
        {
            mCancelToken.ThrowIfCancellationRequested();
            while (char.IsWhiteSpace(mLastChar))
                ReadChar();

            if (mLastChar == STTokenChars.Null)
            {
                argToken = null;
                argCar = '\x0';

                return false;
            }

            int pLinea = mLine;
            int pIni, pFin, pLen = 0;
            var pCar = mLastChar;

            pIni = pFin = mPosition;

            switch (pCar)
            {
                case STTokenChars.Terminate:
                case STTokenChars.TerminateNoShowOut:
                    argToken = null;
                    argCar = pCar;

                    return true;
                default:
                    if (char.IsDigit(pCar))
                    {
                        do
                        {
                            if (!ReadChar())
                                break;

                            pLen++;
                        } while (char.IsDigit(mLastChar));

                        argToken = new STTokenStr { Token = ESTToken.Numeric, Position = pIni, Text = mText.Substring(pIni, pLen) };
                        argCar = '\x0';

                        return true;
                    }
                    break;
            }

            throw new STException(string.Format(Properties.Resources.NoRecognizeStError, pCar), pLinea, pIni);
        }

        private bool ReadChar()
        {
            mLineAnt = mLine;
            mPositionAnt = mPosition;
            while (mPosition >= mText.Length)
            {
                mCancelToken.ThrowIfCancellationRequested();
                if (mLine >= mLines.Length)
                {
                    mLastChar = STTokenChars.Null;

                    return false;
                }

                mText = mLines[++mLine];
                mPosition = 0;
            }
            mCancelToken.ThrowIfCancellationRequested();

            mLastChar = mText[++mPosition];

            return true;
        }
    }
}
