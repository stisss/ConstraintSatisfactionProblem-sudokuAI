using System;
using System.Collections;
using System.Collections.Generic;
using csp.Variables;

namespace csp
{
    class Program
    {
        static void Main(string[] args)
        {
            SudokuCSP scsp = new SudokuCSP();

            scsp.DisplayWorld(scsp.Variables);
            scsp.Solve();


        }

        //public List<int>[] LoadConstraints(int[] variables)
        //{
        //    var constraints = new List<int>();

        //    var relatedCellsIndices = new List<int>();

        //    for (int i = 0; i < variables.Length; i++)
        //    {
        //        // row
        //        int rowIndex = i / GRID_SIZE * 9;
        //        for (int j = 0; j < GRID_SIZE; j++, rowIndex++)
        //        {

        //            relatedCellsIndices.Add(rowIndex);

        //        }

        //        // column
        //        int columnIndex = i % GRID_SIZE;
        //        for (int j = 0; j < GRID_SIZE; j++, columnIndex += GRID_SIZE)
        //        {
        //            relatedCellsIndices.Add(columnIndex);
        //        }

        //        // small grid
        //        int smallGridIndex = i / GRID_SIZE * 9;
        //        for (int j = 0; j < GRID_SIZE; j++, smallGridIndex += (GRID_SIZE - 3))
        //        {
        //            for (int k = 0; k < 3; k++)
        //            {
        //                relatedCellsIndices.Add(smallGridIndex++);
        //            }
        //        }

        //        // delete duplicates
        //        relatedCellsIndices = relatedCellsIndices.Distinct().ToList();

        //        // delete cell's own index
        //        relatedCellsIndices.Remove(i);

        //        // Adding constraints
        //        constraints[i] = new List<IConstraint<SudokuField, char>>();


        //        // add initial constraint
        //        if (!variables[i].Value.Equals(EMPTY_FIELD))
        //        {
        //            var initial_value = variables[i].Value;
        //            constraints[i].Add(new UnaryConstraint<SudokuField, char>(variables[i].Value));
        //        }
        //        else
        //        {
        //            // add binary constraints
        //            for (int j = 0; j < relatedCellsIndices.Count; j++)
        //            {
        //                int index = j;
        //                constraints[i].Add(new BinaryConstraint<SudokuField, char>(relatedCellsIndices[index], variables));
        //            }
        //        }

        //        relatedCellsIndices.Clear();

        //    }
        //    return constraints;
        //}


    }
}
