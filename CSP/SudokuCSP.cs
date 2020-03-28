using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace csp.Variables
{
    class SudokuCSP : CSP<SudokuField, char>
    {
        private int GRID_SIZE = 9;
        private char emptyField = '.';
        List<SudokuField[][]> solutions;
        public SudokuCSP()
        {
            Variables = new SudokuField[GRID_SIZE][];
            Constraints = new List<Constraint<SudokuField>>();
            solutions = new List<SudokuField[][]>();

            Constraint<SudokuField> rowConstraint = new Constraint<SudokuField>(x => RowCheck(x));
            Constraint<SudokuField> columnConstraint = new Constraint<SudokuField>(x => ColumnCheck(x));
            Constraint<SudokuField> gridConstraint = new Constraint<SudokuField>(x => GridCheck(x));

            Constraints.Add(rowConstraint);
            Constraints.Add(columnConstraint);
            Constraints.Add(gridConstraint);

            LoadVariables();
            DisplayWorld(Variables);

        }

        public bool RowCheck(SudokuField sf)
        {
            for (int i = 0; i < Variables[sf.Row].Length; i++)
            {
                if (Variables[sf.Row][i].Value == sf.Value && !Variables[sf.Row][i].Equals(sf))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ColumnCheck(SudokuField sf)
        {
            for (int i = 0; i < Variables.Length; i++)
            {
                if (Variables[i][sf.Column].Value == sf.Value && !Variables[i][sf.Column].Equals(sf))
                {
                    return false;
                }
            }
            return true;
        }

        public bool GridCheck(SudokuField sf)
        {
            for (int i = 0; i < Variables.Length; i++)
            {
                for (int j = 0; j < Variables[i].Length; j++)
                {
                    if (Variables[i][j].Grid == sf.Grid && Variables[i][j].Value == sf.Value && !Variables[i][j].Equals(sf))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void DisplayWorld(SudokuField[][] world)
        {
            for (int i = 0; i < world.Length; i++)
            {
                if (i % 3 == 0)
                {
                    Console.WriteLine("–––––––––––––––––––––––––");
                }
                for (int j = 0; j < world[0].Length; j++)
                {
                    if (j % 3 == 0)
                    {
                        Console.Write("| ");
                    }
                    Console.Write($"{world[i][j].Value} ");
                }
                Console.Write("| ");
                Console.WriteLine();
            }
            Console.WriteLine("–––––––––––––––––––––––––");
        }

        private void LoadVariables()
        {
            char[] data = Loader.GetData();

            int counter = 0;
            for (int i = 0; i < GRID_SIZE; i++)
            {
                Variables[i] = new SudokuField[GRID_SIZE];
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    Variables[i][j] = new SudokuField(i, j, data[counter]);
                    if (!data[counter].Equals(emptyField))
                    {
                        Variables[i][j].Domain.Values = new List<char>() { data[counter] };
                    }
                    counter++;
                }
            }
        }

        private void UpdateDomain(SudokuField sf)
        {
            var temp = new SudokuField(sf);
            List<char> fullSudokuDomain = new List<char>();

            for (int i = 0; i < 9; i++)
            {
                fullSudokuDomain.Add((char)i);
            }
            sf.Domain.Values = fullSudokuDomain;

            foreach (var c in temp.Domain.Values)
            {
                temp.Value = c;
                if (!(RowCheck(temp) && ColumnCheck(temp) && GridCheck(temp)))
                {
                    sf.Domain.Values.Remove(c);
                }
            }
        }

        private void UpdateRelatedFields(SudokuField sf)
        {
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    if (Variables[i][j].Row == sf.Row
                        || Variables[i][j].Column == sf.Column
                        || Variables[i][j].Grid == sf.Grid
                        && !Variables[i][j].Equals(sf))
                    {
                        UpdateDomain(Variables[i][j]);
                    }
                }
            }
        }

        public void InitialiseDomains()
        {

        }

        public void Solve()
        {
            List<SudokuField> fields = new List<SudokuField>();
            for (int i = 0; i < Variables.Length; i++)
            {
                fields.AddRange(Variables[i]);
            }
            for (int i = 0; i < fields.Count; i++)
            {
                UpdateDomain(fields[i]);
            }
            fields.Sort();
            for (int i = 0; i < fields.Count; i++)
            {
                Console.Write(fields[i].Value);
            }
            Console.WriteLine();
            Backtracking(fields, 0);
            Console.WriteLine(solutions.Count);


        }

        public void SaveTheResult(List<SudokuField> solution)
        {
            SudokuField[][] newSolution = new SudokuField[GRID_SIZE][];

            int counter = 0;
            for (int i = 0; i < GRID_SIZE; i++)
            {
                newSolution[i] = new SudokuField[GRID_SIZE];
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    newSolution[i][j] = solution[counter++];
                }
            }

            solutions.Add(newSolution);
            DisplayWorld(newSolution);
        }

        public void Backtracking(List<SudokuField> fields, int index)
        {
            if (fields.Count == index)
            {
                Console.WriteLine("HALO");
                SaveTheResult(fields);
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine(index);
                var temp = fields[index];
                int next = index + 1;


                var tempDomain = new Domain<char>(temp.Domain);
                foreach (var value in tempDomain.Values)
                {
                    temp.Value = value;
                    UpdateRelatedFields(temp);
                    Backtracking(fields, next);

                }
                if (temp.Domain.Values.Count == 0)
                {
                    temp.Value = emptyField;
                    UpdateRelatedFields(temp);
                    return;
                }
                return;
            }
        }
    }
}
