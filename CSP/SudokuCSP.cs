using csp.CSP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace csp.Variables
{
    class SudokuCSP : CSP<SudokuField, char>
    {
        private readonly int GRID_SIZE = 9;
        private readonly char EMPTY_FIELD = '.';


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
                // Initialising constraints
                constraints[i] = new List<IConstraint<SudokuField, char>>();

                // add initial constraint
                if (!variables[i].Value.Equals(EMPTY_FIELD))
                {
                    constraints[i].Add(new UnaryConstraint<SudokuField, char>(variables[i].Value));
                }
                else
                {
                    // row
                    int rowIndex = i / GRID_SIZE * 9;
                    for (int j = 0; j < GRID_SIZE; j++, rowIndex++)
                    {

                        relatedCellsIndices.Add(rowIndex);

                    }

                    // column
                    int columnIndex = i % GRID_SIZE;
                    for (int j = 0; j < GRID_SIZE; j++, columnIndex += GRID_SIZE)
                    {
                        relatedCellsIndices.Add(columnIndex);
                    }

                    // small grid
                    int verticalGrid = i / 27; // każdy rząd smallgridów ma 27 pól, numer smallgrida w pionie
                    int horizontalGrid = i % 9 / 3;  // numer smallgrida w poziomie
                    int smallGridIndex = verticalGrid * 27 + horizontalGrid * 3;
                    for (int j = 0; j < 3; j++, smallGridIndex += GRID_SIZE)
                    {
                        for (int k = smallGridIndex; k < smallGridIndex + 3; k++)
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
                    //for (int j = 0; j < relatedCellsIndices.Count; j++)
                    //{
                    //    Console.Write(relatedCellsIndices[j] + " ");
                    //}
                    //Console.WriteLine();
                    //Console.WriteLine(i);
                    //Console.ReadKey();
                    relatedCellsIndices.Clear();
                }

            }
            return constraints;
        }

        public void DisplayWorld(SudokuField[] world)
        {
            int counter = 0;
            for (int i = 0; i < GRID_SIZE; i++)
            {
                if (i % 3 == 0)
                {
                    Console.WriteLine("–––––––––––––––––––––––––");
                }
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    if (j % 3 == 0)
                    {
                        Console.Write("| ");
                    }
                    Console.Write($"{world[counter++].Value} ");
                }
                Console.Write("| ");
                Console.WriteLine();
            }
            Console.WriteLine("–––––––––––––––––––––––––");
        }

        private SudokuField[] LoadVariables()
        {
            char[] data = Loader.GetData();
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

        //private void UpdateDomain(SudokuField sf)
        //{
        //    var temp = new SudokuField(sf);
        //    List<char> fullSudokuDomain = new List<char>();
        //    string numbers = "123456789";
        //    for (int i = 0; i < 9; i++)
        //    {
        //        fullSudokuDomain.Add(numbers[i]);
        //    }
        //    sf.Domain.Values = fullSudokuDomain;

        //    foreach (var c in temp.Domain.Values)
        //    {
        //        temp.Value = c;
        //        if (!(RowCheck(temp) && ColumnCheck(temp) && GridCheck(temp)))
        //        {
        //            sf.Domain.Values.Remove(c);
        //        }
        //    }
        //}

        //private void UpdateRelatedFields(SudokuField sf)
        //{
        //    for (int i = 0; i < GRID_SIZE; i++)
        //    {
        //        for (int j = 0; j < GRID_SIZE; j++)
        //        {
        //            if (Variables[i][j].Row == sf.Row
        //                || Variables[i][j].Column == sf.Column
        //                || Variables[i][j].Grid == sf.Grid
        //                && !Variables[i][j].Equals(sf))
        //            {
        //                UpdateDomain(Variables[i][j]);
        //            }
        //        }
        //    }
        //}

        public void Solve()
        {
            Backtracking(Variables, 0);
            Console.WriteLine($"Found {Solutions.Count} solutions");
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
            // Console.Clear();
            // Console.WriteLine(index);
            // DisplayWorld(fields);
            // Console.ReadKey();                 
            if (fields.Length == index)
            {
                DisplayWorld(fields);
                Console.ReadKey();
                SaveTheResult(fields);
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

                    temp.Value = value;
                    Backtracking(fields, next);

                }
                if (1 < Constraints[index].Count)
                {
                    temp.Value = EMPTY_FIELD;
                }
                return;
            }
        }
    }
}
