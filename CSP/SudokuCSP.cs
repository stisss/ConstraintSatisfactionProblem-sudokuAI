using csp.CSP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace csp.Variables
{
    class SudokuCSP : CSP<SudokuField, char>
    {
        // 'size' means row length, column length or small grid capacity, it depends on context
        private readonly int GRID_SIZE = 9;
        private readonly int SMALL_GRID_SIZE = 3;
        private readonly char EMPTY_FIELD = '.';

        // Diagnostics
        Stopwatch swFirstSolution;
        Stopwatch swAllSolutions;
        int nodesFirstSolution;
        int nodesAllSolutions;
        int backtrackingFirstSolution;
        int backtrackingAllSolutions;


        public SudokuCSP()
        {
            Variables = LoadVariables();
            Constraints = LoadConstraints(Variables);
            Solutions = new List<char[]>();
        }


        public List<IConstraint<SudokuField, char>>[] LoadConstraints(SudokuField[] variables)
        {
            var constraints = new List<IConstraint<SudokuField, char>>[variables.Length];

            var relatedCellsIndices = new List<int>();

            for (int i = 0; i < variables.Length; i++)
            {
                // Initialise constraints
                constraints[i] = new List<IConstraint<SudokuField, char>>();

                // Add unary constraint for fields with initial values
                if (!variables[i].Value.Equals(EMPTY_FIELD))
                {
                    constraints[i].Add(new UnaryConstraint<SudokuField, char>(variables[i].Value));
                }
                else
                {
                    // Row
                    int rowIndex = i / GRID_SIZE * GRID_SIZE;
                    for (int j = 0; j < GRID_SIZE; j++, rowIndex++)
                    {
                        relatedCellsIndices.Add(rowIndex);
                    }

                    // Column
                    int columnIndex = i % GRID_SIZE;
                    for (int j = 0; j < GRID_SIZE; j++, columnIndex += GRID_SIZE)
                    {
                        relatedCellsIndices.Add(columnIndex);
                    }

                    // Small grid
                    // each row of small grids consists of 3*9=27 fields
                    int verticalGrid = i / (GRID_SIZE * SMALL_GRID_SIZE);

                    // number of small grid in its row
                    int horizontalGrid = i % GRID_SIZE / SMALL_GRID_SIZE;

                    int smallGridIndex = verticalGrid * (GRID_SIZE * SMALL_GRID_SIZE) + horizontalGrid * SMALL_GRID_SIZE;
                    for (int j = 0; j < SMALL_GRID_SIZE; j++, smallGridIndex += GRID_SIZE)
                    {
                        for (int k = smallGridIndex; k < smallGridIndex + SMALL_GRID_SIZE; k++)
                        {
                            relatedCellsIndices.Add(k);
                        }
                    }

                    // delete duplicates
                    relatedCellsIndices = relatedCellsIndices.Distinct().ToList();

                    // delete cell's own index
                    relatedCellsIndices.Remove(i);

                    // add binary constraints
                    for (int j = 0; j < relatedCellsIndices.Count; j++)
                    {
                        int index = j;
                        constraints[i].Add(new BinaryConstraint<SudokuField, char>(relatedCellsIndices[index], variables));
                    }

                    relatedCellsIndices.Clear();
                }
            }
            return constraints;
        }


        public void DisplayWorld(SudokuField[] world)
        {
            string HORIZONTAL_SEPARATOR = "–––––––––––––––––––––––––";
            string VERTICAL_SEPARATOR = "| ";

            int counter = 0;
            for (int i = 0; i < GRID_SIZE; i++)
            {
                if (i % SMALL_GRID_SIZE == 0)
                {
                    Console.WriteLine(HORIZONTAL_SEPARATOR);
                }
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    if (j % SMALL_GRID_SIZE == 0)
                    {
                        Console.Write(VERTICAL_SEPARATOR);
                    }
                    Console.Write($"{world[counter++].Value} ");
                }
                Console.Write(VERTICAL_SEPARATOR);
                Console.WriteLine();
            }
            Console.WriteLine(HORIZONTAL_SEPARATOR);
        }


        private SudokuField[] LoadVariables()
        {
            char[] data = Loader.GetData(); //tablica charów z wartościami komórek
            SudokuField[] fields = new SudokuField[data.Length]; // tablica zmiennych

            int counter = 0;
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    fields[counter] = new SudokuField(i, j, data[counter]);
                    counter++;
                }
            }

            return fields;
        }

        public void ShowDiagnostics()
        {
            string HORIZONTAL_SEPARATOR = "---------------------------------------------------------\n";

            Console.WriteLine($"{"Mode",-20} {"Time [ms]",5} {"Nodes",8} {"Back",8}\n" +
                    HORIZONTAL_SEPARATOR +
                  $"{"First solution",-20} {swFirstSolution.ElapsedMilliseconds,9} {nodesFirstSolution,8} {backtrackingFirstSolution,8}\n" +
                  $"{"All solutions",-20} {swAllSolutions.ElapsedMilliseconds,9} {nodesAllSolutions,8} {backtrackingAllSolutions,8}\n" +
                   HORIZONTAL_SEPARATOR +
                  $"Number of solutions: {Solutions.Count}");
        }


        public void Solve()
        {
            swAllSolutions = new Stopwatch();
            swAllSolutions.Start();

            swFirstSolution = new Stopwatch();
            swFirstSolution.Start();

            nodesFirstSolution = 0;
            nodesAllSolutions = 0;

            backtrackingFirstSolution = 0;
            backtrackingAllSolutions = 0;


            Backtracking(Variables, 0);


            swAllSolutions.Stop();
        }


        public void SaveTheResult(SudokuField[] solution)
        {
            char[] newSolution = new char[solution.Length];

            for (int i = 0; i < solution.Length; i++)
            {
                newSolution[i] = solution[i].Value;
            }

            Solutions.Add(newSolution);
        }


        public void Backtracking(SudokuField[] fields, int index)
        {

            if (fields.Length == index)
            {
                // a solution has been found
                if (Solutions.Count == 0)
                {
                    swFirstSolution.Stop();
                    nodesFirstSolution = nodesAllSolutions;
                    backtrackingFirstSolution = backtrackingAllSolutions;
                }

                SaveTheResult(fields);
                backtrackingAllSolutions++;
                return;
            }
            else
            {
                var temp = fields[index];
                int next = index + 1;

                var tempDomain = new Domain<char>(temp.Domain);
                foreach (var c in Constraints[index])
                {
                    tempDomain.Values = tempDomain.Values.FindAll(c.Check);
                }

                foreach (var value in tempDomain.Values)
                {
                    nodesAllSolutions++;
                    temp.Value = value;
                    Backtracking(fields, next);
                }

                // check if current field has a constant value
                if (1 < Constraints[index].Count)
                {
                    temp.Value = EMPTY_FIELD;
                }
                backtrackingAllSolutions++;
                return;
            }
        }


        public void SaveSolutionsToFile()
        {
            Loader.SaveSolutions(Solutions);
        }
    }
}
