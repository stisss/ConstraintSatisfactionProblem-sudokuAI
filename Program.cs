using System;
using csp.CSP;
using csp.CSP.ValueHeuristics;
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

                IVariableHeuristics<SudokuField> variableHeuristics = ChooseVariableHeuristics();
                IValueHeuristics<char> valueHeuristics = ChooseValueHeuristics(scsp.Variables);

                scsp.SetVariableHeuristics(variableHeuristics);
                scsp.ValueHeuristics = valueHeuristics;

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
                    scsp.SaveSolutionsToFile();
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
                scsp.ResetResults();
            }

        }

        private static IVariableHeuristics<SudokuField> ChooseVariableHeuristics()
        {
            Console.WriteLine("Enter the number of variable heuristics: ");
            Console.WriteLine("1-Basic           --> Choose variables in regular order, from left to right from top to bottom");
            Console.WriteLine("2-StaticRandom    --> Get a random order of choosing variables and keep this order for the whole searching");
            Console.WriteLine("3-DynamicRandom   --> Get a random variable from a set of 'unvisited' variables every time you need a new variable");
            Console.WriteLine("4-Snake           --> Pick variables from the right until you reach the grid's border. Then jump to another row and change direction");

            int variableHeuristicsNumber = Int32.Parse(Console.ReadLine());
            IVariableHeuristics<SudokuField> variableHeuristics;

            if (variableHeuristicsNumber == 1)
            {
                variableHeuristics = new BasicVariableHeuristics<SudokuField>();
            }
            else if (variableHeuristicsNumber == 2)
            {
                variableHeuristics = new RandomStaticVariableHeuristics<SudokuField>();
            }
            else if (variableHeuristicsNumber == 3)
            {
                variableHeuristics = new RandomDynamicVariableHeuristics<SudokuField>();
            }
            else if (variableHeuristicsNumber == 4)
            {
                variableHeuristics = new SnakeVariableHeuristics<SudokuField>();
            }
            else
            {
                Console.WriteLine("Wrong choice, default basic heuristics has been chosen.");
                variableHeuristics = new BasicVariableHeuristics<SudokuField>();
            }

            return variableHeuristics;
        }

        private static IValueHeuristics<char> ChooseValueHeuristics(SudokuField[] variables)
        {
            IValueHeuristics<char> valueHeuristics;

            Console.WriteLine("Enter the number of variable heuristics: ");
            Console.WriteLine("1-Basic           --> Choose values in order from last to first element in domain");
            Console.WriteLine("2-Random          --> Get a random order of choosing values from domain");
            Console.WriteLine("3-MostCommonValue --> Choose values in order from most to least common value of all assigned values");

            int valueHeuristicsNumber = Int32.Parse(Console.ReadLine());

            if (valueHeuristicsNumber == 1)
            {
                valueHeuristics = new BasicValueHeuristics<char>();
            }
            else if (valueHeuristicsNumber == 2)
            {
                valueHeuristics = new RandomValueHeuristics<char>();
            }
            else if (valueHeuristicsNumber == 3)
            {
                var heuristic = new MostCommonValueHeuristics<SudokuField, char>();
                heuristic.Variables = variables;
                valueHeuristics = heuristic;
            }
            else
            {
                Console.WriteLine("Wrong choice, default basic heuristics has been chosen.");
                valueHeuristics = new BasicValueHeuristics<char>();
            }

            return valueHeuristics;
        }
    }
}
