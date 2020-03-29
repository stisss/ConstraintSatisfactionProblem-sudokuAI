using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using csp.Variables;

namespace csp
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            SudokuCSP scsp = new SudokuCSP();
            scsp.DisplayWorld(scsp.Variables);
            scsp.Solve();
            scsp.ShowDiagnostics();
            scsp.SaveSolutionsToFile();

        }
    }
}
