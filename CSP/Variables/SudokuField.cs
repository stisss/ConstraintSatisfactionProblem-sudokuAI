using System.Collections.Generic;

namespace csp
{
    class SudokuField : Variable<char>
    {

        public int Row { get; private set; }
        public int Column { get; private set; }       


        public SudokuField(int row, int column, char value)
        {
            Row = row;
            Column = column;
            Value = value;
            Domain = GetSudokuDomain();
        }


        public SudokuField(SudokuField sf)
        {
            Row = sf.Row;
            Column = sf.Column;
            Domain = new Domain<char>(sf.Domain);
        }


        public Domain<char> GetSudokuDomain()
        {
            List<char> fullSudokuDomain = new List<char>();
            string numbers = "123456789";
            for (int i = 0; i < numbers.Length; i++)
            {
                fullSudokuDomain.Add(numbers[i]);
            }

            return new Domain<char>(fullSudokuDomain);
        }

    }
}
