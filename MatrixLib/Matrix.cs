using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RationalLib;
using System.Drawing;
using Common;

namespace MatrixLib
{
    /// <summary>
    /// Matrix class for calculation equations with multiple unknown variables.
    /// </summary>
    public class Matrix
    {
        /// <summary>
        /// Enumeration used to show how many solutions does the matrix have.
        /// </summary>
        public enum MatrixResult : int
        {
            /// <summary>Exactly one solution</summary>
            Exact = 1,
            /// <summary>Infinite amount of solutions</summary>
            Infinite,
            /// <summary>None solution</summary>
            None,
        }
        
        /// <summary>List of strings with saved matrix compution history</summary>
        public List<string> ComputeHistory { get; private set; }
        /// <summary>List of matrix rows</summary>
        public List<MatrixRow> Rows { get; private set; }
        /// <summary>Amount of rows contained in matrix</summary>
        public int RowCount { get { return Rows.Count; } }
        /// <summary>Amount of columns cointained in matrix</summary>
        public int ColumnCount { get { return Rows[0].Coefficients.Length; } }

        /// <summary>
        /// Matrix constructor
        /// </summary>
        public Matrix()
        {
            this.Rows = new List<MatrixRow>();
            this.ComputeHistory = new List<string>();
        }

        /// <summary>
        /// Validates, if dimmensions of matrix are correct.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            foreach (MatrixRow row in this.Rows)
            {
                if (row.Coefficients.Length != this.Rows.Count)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Solves matrix.
        /// </summary>
        /// <param name="results">Array of rational numbers where results will be saved</param>
        /// <returns></returns>
        public MatrixResult Solve(out Rational[] results)
        {
            this.ComputeHistory.Clear();
            this.ComputeHistory.Add(this.ToString());

            List<MatrixRow> rowsForSolving = GetRowsCopy();
            if (!Validate())
            {
                throw new ApplicationException("Matrix validation not successful when solving matrix.");
            }
            for (int i = 0; i < rowsForSolving.Count; i++)    //go through all lines from the begining
            {
                var rowValidation = rowsForSolving[i].Validate();

                if (rowValidation == MatrixRow.ValidationResult.OK)    //if current row can have one solution
                {
                    if (SimplifyColumnOfMatrix(rowsForSolving, ref i))
                    {
                        this.ComputeHistory.Add(new Matrix() { Rows = rowsForSolving }.ToString());
                    }
                    else
                    {
                        results = null;
                        return MatrixResult.Infinite;
                    }
                }
                else
                {
                    results = null;
                    if (rowValidation == MatrixRow.ValidationResult.InfiniteSolutions)
                    {
                        return MatrixResult.Infinite;
                    }
                    else if (rowValidation == MatrixRow.ValidationResult.NoSolution)
                    {
                        return MatrixResult.None;
                    }
                }
            }
            TransformToMatrixOfOnes(rowsForSolving);

            this.ComputeHistory.Add(new Matrix() { Rows = rowsForSolving }.ToString());
            results = GetResults(rowsForSolving);
            return MatrixResult.Exact;
        }

        private Rational[] GetResults(List<MatrixRow> rowsForSolving)
        {
            return rowsForSolving.Select(x => x.Result).ToArray();
        }

        private bool SimplifyColumnOfMatrix(List<MatrixRow> rowsForSolving, ref int rowNumber)
        {
            //if root can be calculated with stated row
            if (rowsForSolving[rowNumber].Coefficients[rowNumber].Numerator != 0)
            {
                ClearUnwantedCoefficients(rowsForSolving, rowNumber);
            }
            else
            {
                //SwitchRows(rowsForSolving, rowNumber);
                if (SwitchRows(rowsForSolving, rowNumber))
                {
                    rowNumber--;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Clears coefficients above and under certains row coefficient.
        /// </summary>
        /// <param name="rowsForSolving">List of all rows of matrix</param>
        /// <param name="row">Row, whose multiplication will be subtracted from the other rows</param>
        protected void ClearUnwantedCoefficients(List<MatrixRow> rowsForSolving, int row)
        {
            //go through all lines of matrix and substract multiplication of row being adjusted from them
            for (int line = 0; line < rowsForSolving.Count; line++)
            {
                //don't adjust row with the same row
                if (line != row)
                {
                    Rational divider = rowsForSolving[line].Coefficients[row] / rowsForSolving[row].Coefficients[row];
                    rowsForSolving[line] = rowsForSolving[line] - rowsForSolving[row] * divider;
                }
            }
        }

        private List<MatrixRow> GetRowsCopy()
        {
            return this.Rows.DeepClone();
        }

        private void TransformToMatrixOfOnes(List<MatrixRow> rowsCopy)
        {
            for (int i = 0; i < RowCount; i++)
            {
                rowsCopy[i] = rowsCopy[i] * (new Rational(1, 1) / rowsCopy[i].Coefficients[i]);
            }
        }

        /// <summary>
        /// Switches position of row in List on position rowPosition with row on position where
        /// rowsForSolving[position].Coefficients[rowposition] is not equal to zero.
        /// </summary>
        /// <param name="rowsForSolving">List of all rows of matrix</param>
        /// /// <param name="rowPosition">Row whom we want to switch.</param>
        /// <returns>True if row was switched with another one, which can be used
        /// for solving the matrix.</returns>
        private bool SwitchRows(List<MatrixRow> rowsForSolving, int rowPosition)
        {
            for (int position = rowPosition; position < rowsForSolving.Count; position++)
            {
                if (rowsForSolving[position] != rowsForSolving[rowPosition])
                {
                    if (rowsForSolving[position].Coefficients[rowPosition].Numerator != 0)
                    {
                        MatrixRow backup = rowsForSolving[rowPosition];
                        rowsForSolving[rowPosition] = rowsForSolving[position];
                        rowsForSolving[position] = backup;
                        return true;
                    }
                }
            }
            //There is no row for switching which could help in matrix.
            return false;
        }

        /// <summary>
        /// Verifies if entered matrix results are correct.
        /// </summary>
        /// <param name="results">Roots of matrix</param>
        /// <returns>Boolean value, if entered matrix results are correct</returns>
        public bool Verify(Rational[] results)
        {
            Rational equationResult;
            foreach (MatrixRow row in this.Rows)
            {
                equationResult = row.Compute(results);

                if (equationResult != row.Result)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// ToString method.
        /// </summary>
        /// <returns>String representation of matrix</returns>
        public override string ToString()
        {
            Table table = new Table(this.Rows);
            return table.GetTableString();
        }

        /// <summary>
        /// GetHashCode method.
        /// </summary>
        /// <returns>Integer representation of matrix</returns>
        public override int GetHashCode()
        {
            return Rows.GetHashCode() ^ RowCount.GetHashCode();
        }

        /// <summary>
        /// Equals method.
        /// </summary>
        /// <param name="obj">Input matrix of type object</param>
        /// <returns>Boolean value, if current matrix matches obj.</returns>
        public override bool Equals(object obj)
        {
            Matrix matrix = obj as Matrix;
            if (matrix == null)
            {
                return false;
            }
            for (int i = 0; i < this.RowCount; i++)
            {
                if (!this.Rows[i].Equals(matrix.Rows[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}