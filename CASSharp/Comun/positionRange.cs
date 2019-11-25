using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CASSharp.Comun
{
    class PositionRange : IComparable<PositionRange>
    {
        public int PosIni { get; set; }
        public int PosFin { get; set; }

        public int CompareTo(PositionRange obj)
        {
            var pCmp = PosIni - obj.PosIni;

            if (pCmp == 0)
                pCmp = PosFin - obj.PosFin;

            return pCmp;
        }
    }
}
