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
        private string mText;
        private STTokens mTokens = new STTokens();
        private int mPosition = 0;

        public STTokenizerResult Parse(string argText, CancellationToken argCancelToken)
        {
            TaskCompletionSource<string> pTaskCont;

            lock (mLock)
            {
                pTaskCont = mParseContinue;
                mCancelToken = argCancelToken;
            }

            if (pTaskCont == null)
                mText = argText;
            else
                pTaskCont.SetResult(argText);

            Task<STTokenizerResult> pTaskR;

            lock (mLock)
            {
                mParseCompleted = new TaskCompletionSource<STTokenizerResult>();
                pTaskR = mParseCompleted.Task;
            }

            Task.Factory.StartNew(() => Start());

            pTaskR.Wait(argCancelToken);

            return pTaskR.Result;
        }

        private void Start()
        {
            //Parse(mTokens, out char pCar);
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
                    pTask = null;
                    pPromptNoParse = (mPosition < mText.Length) ? mText.Substring(mPosition + 1).Trim() : null;
                }
            }
            mParseCompleted.SetResult(new STTokenizerResult { Tokens = mTokens, Terminate = argTerminate, PromptNoParse = pPromptNoParse });
            if (pTask != null)
            {
                pTask.Task.Wait(pCancelToken);
                mText = pTask.Task.Result;
            }
        }

        //private void ParseToken(ISTTokens argTokens, out char argCar)
        //{
        //    while (mPosition < mText.Length && char.IsWhiteSpace(mText[mPosition]))
        //        mPosition++;

        //    if (mPosition >= mText.Length)
        //    {
        //        argCar = '\x0';

        //        return;
        //    }

        //    int pIni, pFin;

        //    pIni = pFin = mPosition;
        //}

        /*
         
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

         */
    }
}
