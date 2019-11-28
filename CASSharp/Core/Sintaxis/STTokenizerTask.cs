using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CASSharp.Core.Sintaxis
{
    class STTokenizerTask
    {
        private CancellationTokenSource mCancelToken;
        private TaskCompletionSource<string> mParseContinue;
        private TaskCompletionSource<STTokenizerResult> mParseCompleted;

        public Task<STTokenizerResult> StartNew(string argText)
        {
            mParseCompleted = new TaskCompletionSource<STTokenizerResult>();

            Task.Factory.StartNew(() => Start(argText));

            return mParseCompleted.Task;
        }

        public STTokenizerResult Start(string argText)
        {
            return null;
        }
    }
}
