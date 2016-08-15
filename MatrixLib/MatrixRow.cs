using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using RationalLib;

namespace MatrixLib
{
    /// <summary>
    /// MatrixRow class which defines one row of a Matrix class.
    /// </summary>
    [Serializable]
    public class MatrixRow
    {
        /// <summary>
        /// Enumeration used to show how many solutions can MatrixRow have.
        /// </summary>
        public enum ValidationResult
        {
            /// <summary>Exactly one solution</summary>
            OK,
            /// <summary>No solution</summary>
            NoSolution,
            /// <summary>Infinite solutions</summary>
            InfiniteSolutions
        }

        /// <summary>Coefficients of MatrixRow</summary>
        public Rational[] Coefficients { get; private set; }
        /// <summary>Result of MatrixRow</summary>
        public Rational Result { get; private set; }

        /// <summary>
        /// Constructor of MatrixRow
        /// </summary>
        /// <param name="coefficients">Rational coefficients of MatrixRow</param>
        /// <param name="result">Rational result of MatrixRow</param>
        public MatrixRow(Rational[] coefficients, Rational result)
        {
            this.Coefficients = coefficients;
            this.Result = result;
        }

        /// <summary>
        /// Sums two MatrixRows.
        /// </summary>
        /// <param name="row1">First MatrixRow</param>
        /// <param name="row2">Second MatrixRow</param>
        /// <returns>Summarized MatrixRow</returns>
        public static MatrixRow operator +(MatrixRow row1, MatrixRow row2)
        {
            Rational[] addCoefficientResult = new Rational[row1.Coefficients.Length];
            int position = 0;
            foreach (Rational coefficient in row1.Coefficients)
            {
                addCoefficientResult[position] = coefficient + row2.Coefficients[position];
                position++;
            }
            return new MatrixRow(addCoefficientResult, row1.Result + row2.Result);
        }

        /// <summary>
        /// Subtracts two MatrixRows.
        /// </summary>
        /// <param name="row1">First MatrixRow</param>
        /// <param name="row2">Second MatrixRow</param>
        /// <returns>Subtracted MatrixRow</returns>
        public static MatrixRow operator -(MatrixRow row1, MatrixRow row2)
        {
            Rational[] subCoefficientResult = new Rational[row1.Coefficients.Length];
            int position = 0;
            foreach (Rational coefficient in row1.Coefficients)
            {
                subCoefficientResult[position] = coefficient - row2.Coefficients[position];
                position++;
            }
            return new MatrixRow(subCoefficientResult, row1.Result - row2.Result);
        }

        /// <summary>
        /// Multiplies two MatrixRows.
        /// </summary>
        /// <param name="row1">MatrixRow to be multiplied</param>
        /// <param name="num">Multiplier</param>
        /// <returns>Multiplied MatrixRow</returns>
        public static MatrixRow operator *(MatrixRow row1, Rational num)
        {
            Rational[] mulCoefficientResult = new Rational[row1.Coefficients.Length];
            int position = 0;
            foreach (Rational coefficient in row1.Coefficients)
            {
                mulCoefficientResult[position] = coefficient * num;
                position++;
            }
            return new MatrixRow(mulCoefficientResult, row1.Result * num);
        }

        /// <summary>
        /// Compares values of two MatrixRows.
        /// </summary>
        /// <param name="row1">First MatrixRow</param>
        /// <param name="row2">Second MatrixRow</param>
        /// <returns>Confirmation if values of two MatrixRows are equal.</returns>
        public static bool operator ==(MatrixRow row1, MatrixRow row2)
        {
            //if (System.Object.ReferenceEquals(row1, row2))
            //{
            //    return true;
            //}
            // If at least one is null, return false.
            if (((object)row1 == null) || ((object)row2 == null))
            {
                return false;
            }

            for (int i = 0; i < row1.Coefficients.Length; i++)
            {
                if (row1.Coefficients[i] != row2.Coefficients[i])
                {
                    return false;
                }
            }
            if (row1.Result != row2.Result)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Compares values of two MatrixRows.
        /// </summary>
        /// <param name="row1">First MatrixRow</param>
        /// <param name="row2">Second MatrixRow</param>
        /// <returns>Confirmation if values of two MatrixRows are not equal.</returns>
        public static bool operator !=(MatrixRow row1, MatrixRow row2)
        {
            return !(row1 == row2);
        }

        /// <summary>
        /// Gets hash code of MatrixRow.
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return Coefficients.GetHashCode() ^ Result.GetHashCode();
        }

        /// <summary>
        /// Compares MatrixRow to other object.
        /// </summary>
        /// <param name="obj">Object to compare with</param>
        /// <returns>Confirmation if object is instance of Rational and has same values as 
        /// this MatrixRow.</returns>
        public override bool Equals(object obj)
        {
            MatrixRow matrixRowToCompare = obj as MatrixRow;
            if (matrixRowToCompare == null)
            {
                return false;
            }
            for (int i = 0; i < this.Coefficients.Length; i++)
            {
                if (Coefficients[i] != matrixRowToCompare.Coefficients[i])
                {
                    return false;
                }
            }
            if (this.Result != matrixRowToCompare.Result)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Finds out, if row has no solution, infinite solutions or one solution.
        /// </summary>
        /// <returns>ValidationResult</returns>
        public ValidationResult Validate()
        {
            bool zeroCoefficients = true;

            //var allZero = this.Coefficients.All(x => x != 0);
            //var allZero2 = this.Coefficients.All(delegate (double x) { return (x != 0); });

            foreach (Rational coefficient in this.Coefficients)
            {
                if (coefficient.Numerator != 0)
                {
                    zeroCoefficients = false;
                }
            }
            if (zeroCoefficients && this.Result.Numerator != 0)
            {
                return ValidationResult.NoSolution;
            }
            else if (zeroCoefficients && this.Result.Numerator == 0)
            {
                return ValidationResult.InfiniteSolutions;
            }
            return ValidationResult.OK;
        }

        /// <summary>
        /// Computes result of equation after substitution of []results.
        /// </summary>
        /// <param name="results">array of root results</param>
        /// <returns></returns>
        internal Rational Compute(Rational[] results)
        {
            Rational returnResult = new Rational(0, 1);
            for (int i = 0; i < results.Length; i++)
            {
                returnResult += results[i] * Coefficients[i];
            }
            return returnResult;
        }
    }
}
