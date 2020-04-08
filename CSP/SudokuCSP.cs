using csp.CSP.ValueHeuristics;
using csp.CSP.VariableHeuristics;
using csp.Variables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace csp.CSP
{
    class SudokuCSP : CSP<SudokuField, char>
    {
        // 'size' means row length, column length or small grid capacity, it depends on context
        private readonly int GRID_SIZE = 9;
        private readonly int SMALL_GRID_SIZE = 3;
        private readonly char EMPTY_FIELD = '.';

        // Heuristics
        private IVariableHeuristics<SudokuField> VariableHeuristics;
        public IValueHeuristics<char> ValueHeuristics { get; set; }

        // Diagnostics
        Stopwatch swFirstSolution;
        Stopwatch swAllSolutions;
        int nodesFirstSolution;
        int nodesAllSolutions;
        int backtracksFirstSolution;
        int backtracksAllSolutions;


        public SudokuCSP(int puzzleNumber)
        {
            Variables = LoadVariables(puzzleNumber);
            Constraints = LoadConstraints(Variables);
            Solutions = new List<char[]>();
        }

        public IVariableHeuristics<SudokuField> GetVariableHeuristics()
        {
            return VariableHeuristics;
        }


        public void SetVariableHeuristics(IVariableHeuristics<SudokuField> variableHeuristics)
        {
            VariableHeuristics = variableHeuristics;
            VariableHeuristics.Variables = Variables;
        }


        public void ResetResults()
        {
            Solutions = new List<char[]>();
        }


        public List<IConstraint<SudokuField, char>>[] LoadConstraints(SudokuField[] variables)
        {
            var constraints = new List<IConstraint<SudokuField, char>>[variables.Length];

            List<int> relatedCellsIndices;

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
                    relatedCellsIndices = GetRelatedIndices(i);

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


        private SudokuField[] LoadVariables(int puzzleNumber)
        {
            char[] data = Loader.GetData(puzzleNumber);
            SudokuField[] fields = new SudokuField[data.Length];

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

            Console.WriteLine($"{"Mode",-20} {"Time [ms]",5} {"Nodes",8} {"Backtracks",8}\n" +
                    HORIZONTAL_SEPARATOR +
                  $"{"First solution",-20} {swFirstSolution.ElapsedMilliseconds,9} {nodesFirstSolution,8} {backtracksFirstSolution,8}\n" +
                  $"{"All solutions",-20} {swAllSolutions.ElapsedMilliseconds,9} {nodesAllSolutions,8} {backtracksAllSolutions,8}\n" +
                   HORIZONTAL_SEPARATOR +
                  $"Number of solutions: {Solutions.Count}\n");
        }


        private List<List<char>> GetFilterDomainsForInitialValues()
        {
            var filteredDomains = new List<List<char>>();
            for (int i = 0; i < Variables.Length; i++)
            {
                if(!(Variables[i].Value.Equals(EMPTY_FIELD)))
                {
                    filteredDomains.Add(new List<char>() { Variables[i].Value });
                }
                else
                {
                    filteredDomains.Add(new List<char>(Variables[i].Domain.Values));
                }
            }

            return filteredDomains;
        }


        private List<List<char>> GetFilterDomainsForAllVariables()
        {
            var filteredDomains = new List<List<char>>();
            for (int i = 0; i < Variables.Length; i++)
            {
                filteredDomains.Add(new List<char>(Variables[i].Domain.Values));
                foreach (var c in Constraints[i])
                {
                    filteredDomains[i] = filteredDomains[i].FindAll(c.Check);
                }
            }

            return filteredDomains;
        }


        public void SolveBacktracking()
        {
            StartMeasurements();

            List<int> indices = new List<int>();

            int startingIndex = VariableHeuristics.GetStartingIndex();

            List<List<char>> filteredDomains = GetFilterDomainsForInitialValues();

            Backtracking(Variables, filteredDomains, indices, startingIndex);

            swAllSolutions.Stop();
        }


        public void SolveForwardChecking()
        {
            StartMeasurements();

            List<int> indices = new List<int>();

            List<List<char>> filteredDomains = GetFilterDomainsForAllVariables();

            int startingIndex = VariableHeuristics.GetStartingIndex();

            ForwardChecking(Variables, filteredDomains, indices, startingIndex);

            swAllSolutions.Stop();
        }


        public void SolveQuiteNiceSearching()
        {
            StartMeasurements();

            List<int> indices = new List<int>();

            int startingIndex = VariableHeuristics.GetStartingIndex();

            List<List<char>> filteredDomains = GetFilterDomainsForInitialValues();

            QuiteANiceWayOfSearching(Variables, filteredDomains, indices, startingIndex);

            swAllSolutions.Stop();
        }


        public void StartMeasurements()
        {
            swAllSolutions = new Stopwatch();
            swAllSolutions.Start();

            swFirstSolution = new Stopwatch();
            swFirstSolution.Start();

            nodesFirstSolution = 0;
            nodesAllSolutions = 0;

            backtracksFirstSolution = 0;
            backtracksAllSolutions = 0;
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


        public void Backtracking(SudokuField[] fields, List<List<char>> filteredDomains, List<int> indices, int index)
        {
            if (indices.Count == fields.Length)
            {
                if (Solutions.Count == 0)
                {
                    swFirstSolution.Stop();
                    nodesFirstSolution = nodesAllSolutions;
                    backtracksFirstSolution = backtracksAllSolutions;
                }

                SaveTheResult(fields);
                backtracksAllSolutions++;

                return;
            }
            else
            {
                int next = VariableHeuristics.GetNext(indices, index);
                var temp = fields[index];
                var tempDomain = new Domain<char>(filteredDomains[index]);
                char initialValue = !temp.Value.Equals(EMPTY_FIELD) ? temp.Value : EMPTY_FIELD;
                bool constraintsViolated;

                for (int i = 0; i < filteredDomains[index].Count; i++)
                {
                    constraintsViolated = false;
                    temp.Value = ValueHeuristics.GetNext(tempDomain);
                    nodesAllSolutions++;

                    foreach (var c in Constraints[index])
                    {
                        if (!c.Check(temp.Value))
                        {
                            constraintsViolated = true;
                            backtracksAllSolutions++;
                            break;
                        }
                    }
                    if (!constraintsViolated)
                    {
                        Backtracking(fields, filteredDomains, indices, next);
                    }
                }
                indices.Remove(index);
                temp.Value = initialValue;
                backtracksAllSolutions++;
                return;
            }

        }

        public void ForwardChecking(SudokuField[] fields, List<List<char>> filteredDomains, List<int> indices, int index)
        {

            if (indices.Count == fields.Length)
            {
                if (Solutions.Count == 0)
                {
                    swFirstSolution.Stop();
                    nodesFirstSolution = nodesAllSolutions;
                    backtracksFirstSolution = backtracksAllSolutions;
                }

                SaveTheResult(fields);
                backtracksAllSolutions++;
                return;
            }
            else
            {
                int next = VariableHeuristics.GetNext(indices, index);
                var temp = fields[index];
                var tempDomain = new Domain<char>(filteredDomains[index]);
                var relatedIndices = GetRelatedIndices(index);
                char initialValue = !temp.Value.Equals(EMPTY_FIELD) ? temp.Value : EMPTY_FIELD;
                bool constraintsViolated;

                for (int i = 0; i < filteredDomains[index].Count; i++)
                {
                    constraintsViolated = false;
                    temp.Value = ValueHeuristics.GetNext(tempDomain);
                    nodesAllSolutions++;

                    for (int j = 0; j < relatedIndices.Count; j++)
                    {
                        int relatedIdx = relatedIndices[j];
                        if (filteredDomains[relatedIdx].FindAll(x => x != temp.Value).Count == 0)
                        {
                            constraintsViolated = true;
                            break;
                        }
                    }
                    if (!constraintsViolated)
                    {
                        var keepOldDomains = new List<List<char>>(filteredDomains);
                        for (int j = 0; j < relatedIndices.Count; j++)
                        {
                            int relatedIdx = relatedIndices[j];
                            filteredDomains[relatedIdx] = filteredDomains[relatedIdx].FindAll(x => x != temp.Value);
                        }
                        ForwardChecking(fields, filteredDomains, indices, next);
                        for (int j = 0; j < relatedIndices.Count; j++)
                        {
                            int relatedIdx = relatedIndices[j];
                            filteredDomains[relatedIdx] = keepOldDomains[relatedIdx];
                        }
                    }
                }
                indices.Remove(index);
                temp.Value = initialValue;
                backtracksAllSolutions++;
                return;
            }
        }


        public void QuiteANiceWayOfSearching(SudokuField[] fields, List<List<char>> filteredDomains, List<int> indices, int index)
        {

            if (indices.Count == fields.Length)
            {
                if (Solutions.Count == 0)
                {
                    swFirstSolution.Stop();
                    nodesFirstSolution = nodesAllSolutions;
                    backtracksFirstSolution = backtracksAllSolutions;
                }

                SaveTheResult(fields);
                backtracksAllSolutions++;
                return;
            }
            else
            {
                var temp = fields[index];
                int next = VariableHeuristics.GetNext(indices, index);
                char initialValue = !temp.Value.Equals(EMPTY_FIELD) ? temp.Value : EMPTY_FIELD;
                var tempDomain = new Domain<char>(filteredDomains[index]);

                foreach (var c in Constraints[index])
                {
                    tempDomain.Values = tempDomain.Values.FindAll(c.Check);
                }

                for (int i = 0; i < tempDomain.Values.Count; i++)
                {
                    nodesAllSolutions++;
                    temp.Value = ValueHeuristics.GetNext(tempDomain);
                    QuiteANiceWayOfSearching(fields, filteredDomains, indices, next);
                }

                temp.Value = initialValue;
                indices.Remove(index);
                backtracksAllSolutions++;
                return;
            }
        }


        public List<int> GetRelatedIndices(int index)
        {
            List<int> relatedCellsIndices = new List<int>();

            // Row
            int rowIndex = index / GRID_SIZE * GRID_SIZE;
            for (int j = 0; j < GRID_SIZE; j++, rowIndex++)
            {
                relatedCellsIndices.Add(rowIndex);
            }

            // Column
            int columnIndex = index % GRID_SIZE;
            for (int j = 0; j < GRID_SIZE; j++, columnIndex += GRID_SIZE)
            {
                relatedCellsIndices.Add(columnIndex);
            }

            // Small grid
            // each row of small grids consists of 3*9=27 fields
            int verticalGrid = index / (GRID_SIZE * SMALL_GRID_SIZE);

            // number of small grid in its row
            int horizontalGrid = index % GRID_SIZE / SMALL_GRID_SIZE;

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
            relatedCellsIndices.Remove(index);

            return relatedCellsIndices;
        }


        public void SaveSolutionsToFile()
        {
            Loader.SaveSolutions(Solutions);
        }
    }
}
