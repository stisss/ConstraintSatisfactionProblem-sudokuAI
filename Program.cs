using System;
using csp.CSP;
using csp.CSP.VariableHeuristics;

namespace csp
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter the number of sudoku puzzle: ");
            int number = Int32.Parse(Console.ReadLine());

            SudokuCSP scsp = new SudokuCSP(number);
            scsp.SetVariableHeuristics(new BasicVariableHeuristics<SudokuField>());
            scsp.DisplayWorld(scsp.Variables);

            scsp.SolveBacktracking();
            scsp.ShowDiagnostics();
            scsp.ResetResults();

            scsp.SolveForwardChecking();
            scsp.ShowDiagnostics();

            scsp.SaveSolutionsToFile();
        }
    }
}
