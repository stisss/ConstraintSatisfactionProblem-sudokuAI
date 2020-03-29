using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace csp
{
    class Loader
    {
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
    }
}
