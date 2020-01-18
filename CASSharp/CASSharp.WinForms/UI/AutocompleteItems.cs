using FastColoredTextBoxNS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CASSharp.WinForms.UI
{
    class AutocompleteItems : IEnumerable<AutocompleteItem>
    {
        private AutocompleteMenu mMenu;
        private BoardTextBox mBoard;

        public AutocompleteItems(AutocompleteMenu argMenu, BoardTextBox argBoard)
        {
            mMenu = argMenu;
            mBoard = argBoard;
        }

        public IEnumerator<AutocompleteItem> GetEnumerator()
        {
            if (mMenu.Fragment.End <= mBoard.InstruccionEnd)
                foreach (var n in mBoard.InstructionsNames)
                    yield return new AutocompleteItem(n);

            foreach (var n in mBoard.FunctionsNames)
                yield return new AutocompleteItem(n);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
