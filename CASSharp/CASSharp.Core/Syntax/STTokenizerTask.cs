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
using System.Threading.Tasks;

namespace CASSharp.Core.Syntax
{
    class STTokenizerTask
    {
        private object mLock = new object();
        private CancellationToken mCancelToken;
        private TaskCompletionSource<string> mParseContinue;
        private TaskCompletionSource<STTokenizerResult> mParseCompleted;
        private string[] mLineas;
        private string mText;
        private STTokens mTokens = new STTokens();
        private int mLinea = 0;
        private int mLineaAnt = 0;
        private int mPosition = 0;
        private int mPositionAnt = 0;
        private char? mLastChar = null;

        public STTokenizerResult Parse(string[] argLineas, CancellationToken argCancelToken)
        {
            if (argLineas == null || argLineas.Length < 1)
                return null;

            mLineas = argLineas;
            mLinea = 0;
            mPosition = -1;
            mText = mLineas.First();
            ReadChar();

            return null;
        }

        public STTokenizerResult Parse(string argText, CancellationToken argCancelToken)
        {
            Task<STTokenizerResult> pTaskR;

            lock (mLock)
            {
                mParseCompleted = new TaskCompletionSource<STTokenizerResult>();
                pTaskR = mParseCompleted.Task;
            }

            TaskCompletionSource<string> pTaskCont;

            lock (mLock)
            {
                pTaskCont = mParseContinue;
                mCancelToken = argCancelToken;
            }

            if (pTaskCont == null)
            {
                mText = argText;

                Task.Factory.StartNew(() => Start());
            }
            else
            {
                pTaskCont.SetResult(argText);
                pTaskCont.Task.Wait(argCancelToken);
            }

            pTaskR.Wait(argCancelToken);

            return pTaskR.Result;
        }

        private void Start()
        {
            mPosition = 0;
            mTokens.Tokens = new LinkedList<STToken>();

            try
            {
                ParseTokens();
            }
            catch (Exception ex)
            {
                mTokens.Tokens = new LinkedList<STToken>();
                mParseCompleted.SetException(ex);
            }
        }

        private void Yield(ESTTokenizerTerminate argTerminate)
        {
            CancellationToken pCancelToken;
            TaskCompletionSource<string> pTask;
            string pPromptNoParse;

            lock (mLock)
            {
                pCancelToken = mCancelToken;
                if (argTerminate == ESTTokenizerTerminate.No)
                {
                    mParseContinue = pTask = new TaskCompletionSource<string>();
                    pPromptNoParse = null;
                }
                else
                {
                    mParseContinue = pTask = null;
                    if (mPosition < mText.Length)
                    {
                        pPromptNoParse = mText.Substring(mPosition + 1).Trim();
                        if (string.IsNullOrWhiteSpace(pPromptNoParse))
                            pPromptNoParse = null;
                    }
                    else
                        pPromptNoParse = null;
                }
            }
            mParseCompleted.SetResult(new STTokenizerResult { Tokens = mTokens, Terminate = argTerminate, PromptNoParse = pPromptNoParse });
            if (pTask != null)
            {
                pTask.Task.Wait(pCancelToken);
                mText = pTask.Task.Result;
                mPosition = 0;
            }
        }

        private bool ReadChar()
        {
            mLineaAnt = mLinea;
            mPositionAnt = mPosition;
            while (mPosition >= mText.Length)
            {
                mCancelToken.ThrowIfCancellationRequested();
                if (mLinea >= mLineas.Length)
                    return false;

                mText = mLineas[++mLinea];
                mPosition = 0;
            }
            mCancelToken.ThrowIfCancellationRequested();

            mLastChar = mText[mPosition++];

            return true;
        }

        private bool ParseToken(out STToken argToken, out char argCar)
        {
            mCancelToken.ThrowIfCancellationRequested();
            while (mLastChar.HasValue && char.IsWhiteSpace(mLastChar.Value))
                ReadChar();

            if (!mLastChar.HasValue)
            {
                argToken = null;
                argCar = '\x0';

                return false;
            }

            int pLinea = mLinea;
            int pIni, pFin, pLen = 0;
            var pCar = mLastChar.Value;

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
                            ReadChar();
                            pLen++;
                        } while (mLastChar.HasValue && char.IsDigit(mLastChar.Value));

                        argToken = new STTokenStr { Token = ESTToken.Numeric, Position = pIni, Text = mText.Substring(pIni, pLen) };
                        argCar = '\x0';

                        return true;
                    }
                    break;
            }

            throw new STException(string.Format(Properties.Resources.NoRecognizeStError, pCar), pLinea, pIni);
        }

        private void ParseTokensTerminate(STTokensTerminate argTokens)
        {
            ParseTokens(argTokens);
            if (!mLastChar.HasValue)
                return;

            switch (mLastChar.Value)
            {
                case STTokenChars.Terminate:
                case STTokenChars.TerminateNoShowOut:
                    return;
                default:
                    throw new STException(string.Format(Properties.Resources.NoExpectTokenException, mLastChar), mLineaAnt, mPositionAnt);
            }
        }

        private void ParseTokens(STTokens argTokens)
        {
            for (; ; )
            {
                mCancelToken.ThrowIfCancellationRequested();

                if (!mLastChar.HasValue)
                    return;

                ParseToken(out STToken pToken, out char pCar);
                if (pToken == null)
                    return;

                argTokens.Tokens.AddLast(pToken);
            }
        }

        private void ParseTokens()
        {
            for (; ; )
            {
                mCancelToken.ThrowIfCancellationRequested();

                if (mPosition > mText.Length)
                    Yield(ESTTokenizerTerminate.No);
                ParseToken(out STToken pToken, out char pCar);
                if (pToken != null)
                    mTokens.Tokens.AddLast(pToken);
                else
                    switch (pCar)
                    {
                        case '\x0':
                            Yield(ESTTokenizerTerminate.No);

                            break;
                        case STTokenChars.Terminate:
                            Yield(ESTTokenizerTerminate.ShowResult);

                            return;
                        case STTokenChars.TerminateNoShowOut:
                            Yield(ESTTokenizerTerminate.HideResult);

                            return;
                        default:
                            throw new NotImplementedException();
                    }
            }
        }
    }
}
