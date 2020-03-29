using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace csp
{
    class Loader
    {
        private static readonly int GRID_SIZE = 9;
        private static readonly int SMALL_GRID_SIZE = 3;

        public static char[] GetData()
        {
            string PATH = @"../../../Data/Sudoku.csv";
            int FILE_LENGTH = 47;
            int NUMBER_OF_PUZZLE = 43;

            string buffer = "";

            string[] temp;
            string[] id = new string[FILE_LENGTH];
            string[] difficulty = new string[FILE_LENGTH];
            string[] puzzle = new string[FILE_LENGTH];
            string[] solution = new string[FILE_LENGTH];

            int counter = 0;
            if (File.Exists(PATH))
            {
                using (var sr = File.OpenText(PATH))
                {
                    while ((buffer = sr.ReadLine()) != null)
                    {
                        temp = buffer.Split(";");
                        id[counter] = temp[0];
                        difficulty[counter] = temp[1];
                        puzzle[counter] = temp[2];
                        solution[counter] = temp[3];
                        counter++;
                    }

                }
            }

            return puzzle[NUMBER_OF_PUZZLE].ToCharArray();
        }


        public static void SaveSolutions(List<char[]> solutions)
        {
            string PATH = @"../../../Data/SudokuSolutions.txt";
            string buffer = "";
            string HORIZONTAL_SEPARATOR = "–––––––––––––––––––––––––\n";
            string VERTICAL_SEPARATOR = "| ";


            buffer += $"Number of solutions: {solutions.Count}\n";


            for (int i = 0; i < solutions.Count; i++)
            {
                int counter = 0;
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    if (j % SMALL_GRID_SIZE == 0)
                    {
                        buffer += HORIZONTAL_SEPARATOR;
                    }
                    for (int k = 0; k < GRID_SIZE; k++)
                    {
                        if (k % SMALL_GRID_SIZE == 0)
                        {
                            buffer += VERTICAL_SEPARATOR;
                        }
                        buffer += $"{(solutions[j])[counter]} ";
                        counter++;
                    }
                    buffer += VERTICAL_SEPARATOR + "\n";
                }
                buffer += HORIZONTAL_SEPARATOR;
            }

            File.WriteAllText(PATH, buffer);
        }
    }
}
