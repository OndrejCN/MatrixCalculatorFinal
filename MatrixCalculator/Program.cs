using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCalculator
{
    class Program
    {
        public static void Main()
        {
            double [,] matrix = ReadMatrixFile("matrix5.txt");
            double[,] originalMatrix = (double[,])matrix.Clone();
            PrintEquations(matrix);

            try
            {
                SolveMatrix(matrix);
            }
            catch (MatrixHasInfiniteSolutionsException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (MatrixHasNoSolutionException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine();
            PrintEquations(matrix);

            if (TestRootsCorrectness(originalMatrix, matrix))
            {
                Console.WriteLine("Vypocet je spravny.");
            }
            else
            {
                Console.WriteLine("Chyba vo vypocte.");
            }

            Console.ReadKey();
        }        
        public static double[,] ReadMatrixFile(string fileName)
        {
            ReplaceUnwantedSymbols(fileName);
            IEnumerable<string> lines = File.ReadLines(@"C:\Users\trubac\Desktop\" + fileName);

            try
            {
                return ParseLines(lines);
            }
            catch (MatrixFileNotCompleteException e)
            {
                Console.WriteLine(e.Message);
                return new double[0, 0];
            }
        }
        public static void ReplaceUnwantedSymbols(string fileName)
        {
            string text = File.ReadAllText(@"C:\Users\trubac\Desktop\" + fileName);
            text = text.Replace(".", ",");
            File.WriteAllText(@"C:\Users\trubac\Desktop\" + fileName, text);
        }
        public static double[,] ParseLines(IEnumerable<string> lines)
        {
            int rowNumber = 0;
            int rowLength = lines.Count() + 1;
            double[,] doubleMatrix = new double[lines.Count(), rowLength];

            foreach (string line in lines)
            {
                string[] stringNumbers = ParseLine(line);
                if (stringNumbers.Length == rowLength)
                {
                    for (int i = 0; i < stringNumbers.Count(); i++)
                    {
                        doubleMatrix[rowNumber, i] = Double.Parse(stringNumbers[i]);
                    }
                    rowNumber++;
                }
                else
                {
                    throw new ApplicationException("Neuplny riadok matice.");
                }
            }
            return doubleMatrix;
        }
        public static string[] ParseLine(string line)
        {
            char[] delimiterChars = { ' ', '\t' };
            string[] stringNumbers = line.Split(delimiterChars);
            return stringNumbers;
        }
        public static void PrintEquations(double[,] matrix)
        {
            int variableCode = 109;
            int matrixColumnCount = matrix.GetLength(1);

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrixColumnCount; j++)
                {
                    if (j < matrixColumnCount - 1)
                    {
                        Console.Write("{0:0.##}" + "" + (char)variableCode + "\t", matrix[i, j]);
                        //Console.Write(matrix[i, j] + "" + (char)variableCode + "\t");
                        variableCode++;
                    }
                    else
                    {
                        Console.Write("{0:0.##}", matrix[i, j]);
                        //Console.Write(matrix[i, j]);
                    }
                    if (j == matrixColumnCount - 2)
                    {
                        Console.Write("=\t");
                    }
                }
                Console.WriteLine();
                variableCode = 109;
            }
        }
        public static void SolveMatrix(double [,] matrix)
        {
            double[] switchLine = new double[matrix.GetLength(1)];
            bool isZero = true;
            bool noResult = false;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                isZero = true;

                for (int j = 0; j < matrix.GetLength(1); j++)   //zisti, ci je cely riadok nulovy
                {
                    if (matrix[i, j] != 0)
                    {
                        isZero = false;
                    }
                    if (j == matrix.GetLength(1) - 2 && isZero == true && matrix[i, matrix.GetLength(1)-1] != 0)
                    {
                        noResult = true;
                    }
                }

                if (matrix[i, i] != 0)              //ak je pozicia pre hladany koren nenulova
                {
                    SubtractThisLine(i, matrix);
                }

                else if (isZero)                    //ak je cely riadok nulovy
                {
                    throw new MatrixHasInfiniteSolutionsException("Matica ma nekonecne vela rieseni.");
                }
                else if (matrix[i, i] == 0 && i < (matrix.GetLength(0) - 1))   //ak je pozicia pre hladany koren nulova, ale riadok nie je posledny v matici a nie je uplne nulovy
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        switchLine[j] = matrix[i, j];
                    }
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        matrix[i, j] = matrix[i + 1, j];
                    }
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        matrix[i + 1, j] = switchLine[j];
                    }
                    i--;    //vrat sa k tomu istemu uz vymenenemu riadku
                }
                else if (noResult)
                {
                    throw new MatrixHasNoSolutionException("Matica nema ziadne riesenie.");
                }
            }
            FindRoots(matrix);
        }
        public static void SubtractThisLine(int line, double[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)   //chod cez vsetky riadky
            {
                if (i != line)  //ak sa nachadzas v riadku, ktory chces vynulovat
                {
                    double ratio = matrix[i, line] / matrix[line, line];
                    for (int j = 0; j < matrix.GetLength(1); j++)   //chod cez vsetky splpce
                    {
                        matrix[i, j] = matrix[i, j] - ratio * matrix[line, j];
                    }
                }
            }
        }
        public static void FindRoots(double[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                double divider = matrix[i, i];
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = matrix[i, j] / divider;
                }
            }
        }
        public static bool TestRootsCorrectness(double[,] originalMatrix, double[,] resultMatrix)
        {
            for (int i = 0; i < originalMatrix.GetLength(0); i++)
            {
                double lineResult = 0;
                for (int j = 0; j < originalMatrix.GetLength(1)-1; j++)
                {
                    double a = originalMatrix[i, j];
                    double b = resultMatrix[j, resultMatrix.GetLength(1) - 1];
                    lineResult += a * b;
                }
                if (Math.Round(lineResult, 5, MidpointRounding.AwayFromZero) != originalMatrix[i, originalMatrix.GetLength(1) - 1])
                {
                    return false;
                }
            }
            return true;
        }
    }
    public class MatrixFileNotCompleteException : Exception
    {
        public MatrixFileNotCompleteException(string message)
            : base(message)
        {
        }
    }
    public class MatrixHasInfiniteSolutionsException : Exception
    {
        public MatrixHasInfiniteSolutionsException(string message)
            : base(message)
        {
        }
    }
    public class MatrixHasNoSolutionException : Exception
    {
        public MatrixHasNoSolutionException(string message)
            : base(message)
        {
        }
    }
}