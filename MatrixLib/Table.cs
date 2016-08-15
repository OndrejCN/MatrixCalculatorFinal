using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RationalLib;

namespace MatrixLib
{
    class Table
    {
        Field[,] tableField;

        public Table(List<MatrixRow> rows)
        {
            tableField = new Field[rows.Count, rows.Count+1];

            //fill each column
            for (int i = 0; i < rows.Count+1; i++)
            {
                int columnWidth = GetColumnWidth(i, rows);
                FillColumn(i, rows, columnWidth);
            }
        }

        public string GetTableString()
        {
            string output = "";
            //go through all lines of table
            for (int i = 0; i < tableField.GetLength(0); i++)
            {
                //go through all lines of field
                for (int j = 0; j < 3; j++)
                {
                    //go through all columns
                    for (int k = 0; k < tableField.GetLength(1); k++)
                    {
                        output += tableField[i, k].linesOfField[j];
                        output += "\t";
                    }
                    output += Environment.NewLine;
                }
                output += Environment.NewLine;
            }
            return output;
        }

        private void FillColumn(int columnPosition, List<MatrixRow> rows, int columnWidth)
        {
            if (columnPosition < rows.Count)
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    tableField[i, columnPosition] = new Field();
                    tableField[i, columnPosition].Fill(rows[i].Coefficients[columnPosition], columnWidth);
                }
            }
            else
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    tableField[i, columnPosition] = new Field();
                    tableField[i, columnPosition].Fill(rows[i].Result, columnWidth);
                }
            }
        }

        private int GetColumnWidth(int columnPosition, List<MatrixRow> rows)
        {
            int maxWidth = 0;
            //go through all lines in column
            if (columnPosition < rows.Count)
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    int currentRationalWidth = GetWidthOfRational(rows[i].Coefficients[columnPosition]);
                    maxWidth = (maxWidth > currentRationalWidth ? maxWidth : currentRationalWidth);
                }
            }
            else
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    int currentRationalWidth = GetWidthOfRational(rows[i].Result);
                    maxWidth = (maxWidth > currentRationalWidth ? maxWidth : currentRationalWidth);
                }
            }
            return maxWidth;
        }

        public static int GetWidthOfRational(Rational rationalNumber)
        {
            int numeratorLength = rationalNumber.Numerator.ToString().Length;
            int denominatorLength = rationalNumber.Denominator.ToString().Length;
            return (numeratorLength > denominatorLength ? numeratorLength : denominatorLength);
        }
    }
}
