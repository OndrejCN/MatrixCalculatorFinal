using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixLib;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RationalLib;
using MatrixReaderLib;

namespace MatrixLibUnitTest
{
    /// <summary>
    /// Main class for execution of tests of Matrix class.
    /// </summary>
    [TestClass]
    public class MainMatrixTest
    {
        const int CYCLES = 50;                //kolko matic sa ma vygenerovat a overit
        const int MIN_COEF_VALUE = 1;        //minimalna hodnota koeficientu
        const int MAX_COEF_VALUE = 5;       //maximalna hodnota koeficientu
        const int MIN_PARAM_VALUE = 1;       //minimalna hodnota premennej      MUSI BYT NENULOVA!!!
        const int MAX_PARAM_VALUE = 5;      //maximalna hodnota premennej
        //const int COEF_MAX_PLACES = 2;       //maximalny pocet desatinnych miest v koeficiente ... uz nepouzivam
        //const int PARAM_MAX_PLACES = 2;      //maximalny pocet desatinnych miest v premennej ... uz nepouzivam
        const int MIN_SIZE = 2;              //minimalna velkost matice
        const int MAX_SIZE = 4;              //maximalna velkost matice
        Random rand = new Random();

        private Matrix CreateRandomMatrix(out Rational[] setResults)
        {
            Matrix matrix = new Matrix();
            int matrixSize = rand.Next(MIN_SIZE, MAX_SIZE);
            Rational[] parameters;
            Rational[] coefficients;
            Rational result;
            setResults = new Rational[matrixSize];

            //create parameters
            parameters = CreateParameters(matrixSize);
            //for each new MatrixRow
            for (int l = 0; l < matrixSize; l++)
            {
                //create coefficients
                coefficients = CreateCoefficients(matrixSize);
                //calculate result
                result = CalculateResult(matrixSize, coefficients, parameters);
                //add new row
                matrix.Rows.Add(new MatrixRow(coefficients, result));
            }
            setResults = parameters;
            return matrix;
        }

        private List<Matrix> CreateRandomMatrixList(out List<Rational[]> setResults)
        {
            List<Matrix> randomMatrixList = new List<Matrix>();
            setResults = new List<Rational[]>();

            for (int k = 0; k < CYCLES; k++)
            {
                Rational[] matrixResults;
                randomMatrixList.Add(CreateRandomMatrix(out matrixResults));
                setResults.Add(matrixResults);
            }
            return randomMatrixList;
        }

        private Rational GenerateRandomParameter(int minValue, int maxValue)
        {
            int numerator = rand.Next(minValue, maxValue);
            int denominator = rand.Next(minValue, maxValue);

            return new Rational(numerator, denominator);
        }

        private Rational[] CreateParameters(int matrixSize)
        {
            Rational[] parameters = new Rational[matrixSize];
            for (int i = 0; i < matrixSize; i++)
            {
                parameters[i] = GenerateRandomParameter(MIN_PARAM_VALUE, MAX_PARAM_VALUE);
            }
            return parameters;
        }

        private Rational[] CreateCoefficients(int matrixSize)
        {
            Rational[] coefficients = new Rational[matrixSize];
            for (int i = 0; i < matrixSize; i++)
            {
                coefficients[i] = GenerateRandomParameter(MIN_COEF_VALUE, MAX_COEF_VALUE);
            }
            return coefficients;
        }

        private Rational CalculateResult(int matrixSize, Rational[] coefficients, Rational[] parameters)
        {
            Rational result = new Rational(0, 1);
            for (int j = 0; j < matrixSize; j++)
            {
                result += coefficients[j] * parameters[j];
            }
            return result;
        }

        private List<Rational[]> SolveListOfMatrixes(List<Matrix> matrixList, out Matrix.MatrixResult[] solutionsResults)
        {
            List<Rational[]> solvedResults = new List<Rational[]>();
            solutionsResults = new Matrix.MatrixResult[matrixList.Count];

            //go through all matrixes and solve them
            for (int i = 0; i < matrixList.Count; i++)
            {
                Rational[] result = new Rational[matrixList[i].RowCount];
                solutionsResults[i] = matrixList[i].Solve(out result);
                //matrixResults.Add(matrixList[i].Solve(out result));
                solvedResults.Add(result);
            }
            return solvedResults;
        }

        /// <summary>
        /// Tests Solve method on randomly made Matrix.
        /// </summary>
        [TestMethod]
        public void Test_SolveMethodWithSingleRandomMatrix()
        {
            Rational[] setResults;
            Matrix.MatrixResult solutionResult;
            Matrix matrix = CreateRandomMatrix(out setResults);

            Rational[] results = new Rational[matrix.RowCount];
            solutionResult = matrix.Solve(out results);

            if (solutionResult != Matrix.MatrixResult.Infinite)
            {
                //go through all rows of matrix
                for (int i = 0; i < matrix.RowCount; i++)
                {
                    if (!results[i].Equals(setResults[i]))
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        /// <summary>
        /// Tests Solve method on list of randomly made Matrixes.
        /// </summary>
        [TestMethod]
        public void Test_SolveMethodWithListOfRandomMatrixes()
        {
            List<Rational[]> setResults;
            Matrix.MatrixResult[] solutionsResult;
            List<Matrix> matrixList = CreateRandomMatrixList(out setResults);
            List<Rational[]> solvedResults = SolveListOfMatrixes(matrixList, out solutionsResult);

            //go through all matrixes and compare setResults with solvedResults
            for (int i = 0; i < matrixList.Count; i++)
            {
                if (solutionsResult[i] != Matrix.MatrixResult.Infinite)
                {
                    //go through all rows of matrix
                    for (int j = 0; j < matrixList[i].RowCount; j++)
                    {
                        if (!solvedResults[i][j].Equals(setResults[i][j]))
                        {
                            Assert.Fail();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tests Validate method on Matrix, which is missing one coefficient.
        /// </summary>
        [TestMethod]
        public void Test_MatrixValidate_MissingOneCoefficient()
        {
            MatrixRow row1 = new MatrixRow(new Rational[3] { new Rational(1, 1), new Rational(1, 1), new Rational(1, 1) }, new Rational(1, 1));
            MatrixRow row2 = new MatrixRow(new Rational[2] { new Rational(1, 1), new Rational(1, 1) }, new Rational(1, 1));
            MatrixRow row3 = new MatrixRow(new Rational[3] { new Rational(1, 1), new Rational(1, 1), new Rational(1, 1) }, new Rational(1, 1));

            Matrix matrix = new Matrix();
            matrix.Rows.Add(row1);
            matrix.Rows.Add(row2);
            matrix.Rows.Add(row3);

            if (matrix.Validate())
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests Validate method on Matrix, which is missing one row.
        /// </summary>
        [TestMethod]
        public void Test_MatrixValidate_MissingOneRow()
        {
            MatrixRow row1 = new MatrixRow(new Rational[3] { new Rational(1, 1), new Rational(1, 1), new Rational(1, 1) }, new Rational(1, 1));
            MatrixRow row2 = new MatrixRow(new Rational[3] { new Rational(1, 1), new Rational(1, 1), new Rational(1, 1) }, new Rational(1, 1));

            Matrix matrix = new Matrix();
            matrix.Rows.Add(row1);
            matrix.Rows.Add(row2);

            if (matrix.Validate())
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests ClearUnwantedCoefficients method on Matrix.
        /// </summary>
        [TestMethod]
        public void Test_ClearUnwantedCoefficients()
        {
            MatrixRow row1 = new MatrixRow(new Rational[3] { new Rational(1, 1), new Rational(2, 1), new Rational(1, 1) }, new Rational(1, 1));
            MatrixRow row2 = new MatrixRow(new Rational[3] { new Rational(0, 1), new Rational(1, 1), new Rational(1, 1) }, new Rational(1, 1));
            MatrixRow row3 = new MatrixRow(new Rational[3] { new Rational(0, 1), new Rational(2, 1), new Rational(1, 1) }, new Rational(1, 1));

            TestMatrix matrix = new TestMatrix();
            matrix.Rows.Add(row1);
            matrix.Rows.Add(row2);
            matrix.Rows.Add(row3);

            matrix.ClearUnwantedCoefficients(matrix.Rows, 1);

            if (matrix.Rows[0].Coefficients[1].Numerator != 0 || matrix.Rows[2].Coefficients[1].Numerator != 0)
            {
                Assert.Fail();
            }
        }
    }
}