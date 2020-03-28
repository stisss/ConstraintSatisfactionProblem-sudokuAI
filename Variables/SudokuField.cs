using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace csp
{
    class SudokuField : Variable<char>
    {

        public int Row { get; private set; }
        public int Column { get; private set; }
        public int Grid { get; private set; } // czy to jest tu potrzebne?
       

        public SudokuField(int row, int column, char value)
        {
            Row = row;
            Column = column;
            Grid = row/3*3 + column / 3;
            Value = value;

            List<char> fullSudokuDomain = new List<char>();  // czy powinienem wyciągnąć inicjalizację pełnej dziedziny do osobnej metody?
            string numbers = "123456789";
            for(int i = 0; i < 9; i++)
            {
                fullSudokuDomain.Add(numbers[i]);
            }

            Domain = new Domain<char>(fullSudokuDomain);
        }

        public SudokuField(SudokuField sf)
        {
            this.Row = sf.Row;
            this.Column = sf.Column;
            this.Grid = sf.Grid;
            this.Domain = new Domain<char>(sf.Domain);
        }



    }
}
