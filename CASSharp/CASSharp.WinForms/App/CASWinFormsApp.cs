using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core = CASSharp.Core;
using CAS = CASSharp.Core.CAS;
using Exprs = CASSharp.Core.Exprs;
using ST = CASSharp.Core.Syntax;

namespace CASSharp.WinForms.App
{
    public sealed class CASWinFormsApp : Core.App.CASApp
    {
        protected override void BeforeRun()
        {
            base.BeforeRun();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }
    }
}
