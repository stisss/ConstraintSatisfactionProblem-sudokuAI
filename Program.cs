using System;
using csp.CSP;
using csp.CSP.VariableHeuristics;

namespace csp
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("Enter the number of sudoku puzzle: ");
                int number = Int32.Parse(Console.ReadLine());

                SudokuCSP scsp = new SudokuCSP(number);

                Console.WriteLine("Enter the number of variable heuristics: ");
                Console.WriteLine("1-Basic          --> Choose variables in regular order, from left to right from top to bottom");
                Console.WriteLine("2-StaticRandom   --> Get a random order of choosing variables and keep this order for the whole searching");
                Console.WriteLine("3-DynamicRandom  --> Get a random variable from a set of 'unvisited' variables every time you need a new variable");

                int variableHeuristicsNumber = Int32.Parse(Console.ReadLine());

                if (variableHeuristicsNumber == 1)
                {
                    scsp.SetVariableHeuristics(new BasicVariableHeuristics<SudokuField>());
                }
                else if (variableHeuristicsNumber == 2)
                {
                    scsp.SetVariableHeuristics(new RandomStaticVariableHeuristics<SudokuField>());
                }
                else if (variableHeuristicsNumber == 3)
                {
                    scsp.SetVariableHeuristics(new RandomDynamicVariableHeuristics<SudokuField>());
                }
                else
                {
                    Console.WriteLine("Wrong choice, default basic heuristics has been chosen.");
                    scsp.SetVariableHeuristics(new BasicVariableHeuristics<SudokuField>());
                }
                scsp.DisplayWorld(scsp.Variables);


                Console.WriteLine("Enter the number of variable heuristics: ");
                Console.WriteLine("1-Backtracking");
                Console.WriteLine("2-ForwardChecking");
                Console.WriteLine("3-BOTH");

                int searchingMethod = Int32.Parse(Console.ReadLine());

                if (searchingMethod == 1)
                {
                    scsp.SolveBacktracking();
                }
                else if (searchingMethod == 2)
                {
                    scsp.SolveForwardChecking();
                }
                else if (searchingMethod == 3)
                {
                    scsp.SolveBacktracking();
                    scsp.ShowDiagnostics();
                    scsp.ResetResults();

                    scsp.SolveForwardChecking();

                }
                else
                {
                    Console.WriteLine("Wrong choice, backtracking method has been chosen.");
                    scsp.SolveBacktracking();
                }


                scsp.ShowDiagnostics();

                scsp.SaveSolutionsToFile();
            }

        }
    }
}
