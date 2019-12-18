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
    public sealed class STTokensReader
    {
        /*
        public class ParamsIs
        {
            public Func<LinkedListNode<STToken>, LinkedListNode<STToken>> GetNode { get; set; }
            public Func<STToken, bool> Cond { get; set; }
        }
        */

        public STTokens Tokens { get; set; }
        public CancellationToken CancelToken { get; }
        public LinkedList<STToken> TokensList { get; }
        public LinkedListNode<STToken> NodeAct { get; private set; }
        public STToken Token => NodeAct?.Value;
        public STTokenStr TokenStr => (STTokenStr)Token;
        public STTokenBlock TokenBlock => (STTokenBlock)Token;
        public bool EOF => NodeAct == null;

        public STTokensReader(STTokens argTokens, CancellationToken argCancelToken)
        {
            Tokens = argTokens;
            CancelToken = argCancelToken;
            TokensList = Tokens.Tokens;
            NodeAct = TokensList.First;
        }

        public bool Is(out LinkedListNode<STToken>[] argNodes, int argCount, Func<LinkedListNode<STToken>, LinkedListNode<STToken>> argGetNode, Func<STToken, bool> argCond)
        {
            var pNodes = new LinkedListNode<STToken>[argCount];
            var pNode = NodeAct;

            for (var i = 0; i < argCount; i++)
            {
                CancelToken.ThrowIfCancellationRequested();

                if (pNode == null || !argCond.Invoke(pNode.Value))
                {
                    argNodes = null;

                    return false;
                }
                pNodes[i] = pNode;
                pNode = argGetNode(pNode);
            }

            argNodes = pNodes;

            return true;
        }

        public bool Is(int argCount, Func<LinkedListNode<STToken>, LinkedListNode<STToken>> argGetNode, Func<STToken, bool> argCond)
        {
            var pNode = NodeAct;

            for (var i = 0; i < argCount; i++)
            {
                CancelToken.ThrowIfCancellationRequested();

                if (pNode == null || !argCond.Invoke(pNode.Value))
                    return false;

                pNode = argGetNode(pNode);
            }

            return true;
        }

        public bool IsPrev(out LinkedListNode<STToken>[] argNodes, int argCount, Func<STToken, bool> argCond) => Is(out argNodes, argCount, n => n.Previous, argCond);
        public bool IsPrev(int argCount, Func<STToken, bool> argCond) => Is(argCount, n => n.Previous, argCond);


        public bool IsNext(out LinkedListNode<STToken>[] argNodes, int argCount, Func<STToken, bool> argCond) => Is(out argNodes, argCount, n => n.Next, argCond);
        public bool IsNext(int argCount, Func<STToken, bool> argCond) => Is(argCount, n => n.Next, argCond);

        public bool IsNext(out LinkedListNode<STToken> argNodeNext, Func<STToken, bool> argCond)
        {
            var pNodeNext = NodeAct?.Next;

            if (pNodeNext == null)
            {
                argNodeNext = null;

                return false;
            }

            argNodeNext = pNodeNext;

            return argCond.Invoke(pNodeNext.Value);
        }

        public bool NextIfNodeNextValue(Func<STToken, bool> argCond)
        {
            var pNodeNext = NodeAct?.Next;

            if (pNodeNext == null)
                return false;

            return argCond.Invoke(pNodeNext.Value) && Next();
        }

        /*

        public bool Is(out LinkedListNode<STToken>[] argNodes, params ParamsIs[] argParams)
        {
            var pNodes = new List<LinkedListNode<STToken>>();
            var pNode = NodeAct;

            pNodes.Add(pNode);
            foreach (var pParam in argParams)
            {
                var pNodeCalc = pParam.GetNode(pNode);

                if (pNodeCalc == null || !pParam.Cond(pNodeCalc.Value))
                {
                    argNodes = null;

                    return false;
                }
                pNodes.Add(pNodeCalc);
            }
            argNodes = pNodes.ToArray();

            return true;
        }

        public bool Is(out LinkedListNode<STToken>[] argNodes, Func<LinkedListNode<STToken>, LinkedListNode<STToken>> argGetNode, Func<STToken, bool> argCond)
            => Is(out argNodes, new ParamsIs { GetNode = argGetNode, Cond = argCond });

        public bool Is(out LinkedListNode<STToken>[] argNodes, Func<LinkedListNode<STToken>, LinkedListNode<STToken>> argGetNode1, Func<STToken, bool> argCond1, Func<LinkedListNode<STToken>, LinkedListNode<STToken>> argGetNode2, Func<STToken, bool> argCond2)
            => Is(out argNodes, new ParamsIs { GetNode = argGetNode1, Cond = argCond1 }, new ParamsIs { GetNode = argGetNode2, Cond = argCond2 });

        public bool Is(Func<LinkedListNode<STToken>, LinkedListNode<STToken>> argGetNode, Func<STToken, bool> argCond)
            => Is(out LinkedListNode<STToken>[] argNodes, argGetNode, argCond);

        public bool Is(Func<LinkedListNode<STToken>, LinkedListNode<STToken>> argGetNode1, Func<STToken, bool> argCond1, Func<LinkedListNode<STToken>, LinkedListNode<STToken>> argGetNode2, Func<STToken, bool> argCond2)
            => Is(out LinkedListNode<STToken>[] argNodes, argGetNode1, argCond1, argGetNode2, argCond2);

    */

        public bool Next()
        {
            CancelToken.ThrowIfCancellationRequested();

            if (NodeAct == null)
                return false;

            NodeAct = NodeAct.Next;

            return NodeAct != null;
        }
    }
}
