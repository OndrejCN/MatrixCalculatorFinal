using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Class with common methods being used in entire solution.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Sums array of bytes and adds it to 16 bits variable.
        /// </summary>
        /// <param name="data">Array of bytes/</param>
        /// <param name="checkSum">Input and output of summing</param>
        public static void AddBytesToInt16(byte[] data, ref Int16 checkSum)
        {
            long longSum = data.Sum(x => (long)x);
            checkSum += unchecked((Int16)longSum);
        }

        /// <summary>
        /// Gets amount of digits in a number.
        /// </summary>
        /// <param name="number">Input value</param>
        /// <returns>Amount of digits</returns>
        public static int GetDigitsAmount(int number)
        {
            return Math.Abs(number).ToString().Length;
        }

        /// <summary>
        /// Gets smallest common multible of two integer numbers.
        /// </summary>
        /// <param name="r1">First number</param>
        /// <param name="r2">Second number</param>
        /// <returns>Smallest common multiple</returns>
        public static int GetSmallestCommonMultiple(int r1, int r2)
        {
            int smallestCommonMultiple = 1;
            List<int> innerDenominatorPartialDividors = GetPartialDividors(r1);
            List<int> outerDenominatorPartialDividors = GetPartialDividors(r2);
            List<int> commonList = GetListOfCommonPartialDividors(r1, r2);

            foreach (int commonDividor in commonList)
            {
                for (int i = 0; i < innerDenominatorPartialDividors.Count; i++)
                {
                    if (commonDividor == innerDenominatorPartialDividors[i])
                    {
                        innerDenominatorPartialDividors.RemoveAt(i);
                        break;
                    }
                }
                for (int j = 0; j < outerDenominatorPartialDividors.Count; j++)
                {
                    if (commonDividor == outerDenominatorPartialDividors[j])
                    {
                        outerDenominatorPartialDividors.RemoveAt(j);
                        break;
                    }
                }
                smallestCommonMultiple *= commonDividor;
            }
            foreach (int innerDividor in innerDenominatorPartialDividors)
            {
                smallestCommonMultiple *= innerDividor;
            }
            foreach (int outerDividor in outerDenominatorPartialDividors)
            {
                smallestCommonMultiple *= outerDividor;
            }
            return smallestCommonMultiple;
        }

        /// <summary>
        /// Gets list of base partial dividors of a number.
        /// </summary>
        /// <param name="number">Input number</param>
        /// <returns>List of base partial dividors</returns>
        public static List<int> GetPartialDividors(int number)
        {
            int divisor = 2;
            List<int> partialDividors = new List<int>();
            number = Math.Abs(number);
            while (number >= divisor)
            {
                if (number % divisor == 0)
                {
                    partialDividors.Add(divisor);
                    number /= divisor;
                }
                else
                {
                    divisor++;
                }
            }
            return partialDividors;
        }

        /// <summary>
        /// Gets list of common partial dividors of two numbers.
        /// </summary>
        /// <param name="r1">First input number</param>
        /// <param name="r2">Second input number</param>
        /// <returns>List of common partial dividors</returns>
        public static List<int> GetListOfCommonPartialDividors(int r1, int r2)
        {
            List<int> innerDenominatorPartialDividors = GetPartialDividors(r1);
            List<int> outerDenominatorPartialDividors = GetPartialDividors(r2);

            List<int> commonList = new List<int>() { 1 };

            for (int i = 0; i < innerDenominatorPartialDividors.Count; i++)
            {
                for (int j = 0; j < outerDenominatorPartialDividors.Count; j++)
                {
                    if (innerDenominatorPartialDividors[i] == outerDenominatorPartialDividors[j])
                    {
                        commonList.Add(innerDenominatorPartialDividors[i]);
                        innerDenominatorPartialDividors.RemoveAt(i);
                        outerDenominatorPartialDividors.RemoveAt(j);
                        i--;
                        break;
                    }
                }
            }
            return commonList;
        }

        /// <summary>
        /// Gets highest common dividor of two numbers.
        /// </summary>
        /// <param name="r1">First input number</param>
        /// <param name="r2">Second input number</param>
        /// <returns>Highest common dividor</returns>
        public static int GetHighestCommonDividor(int r1, int r2)
        {
            return (GetListOfCommonPartialDividors(r1, r2).Aggregate((a, x) => a * x));
        }

        /// <summary>
        /// Creates a deep clone of an object using BinaryFormatter.
        /// </summary>
        /// <typeparam name="T">Object for cloning</typeparam>
        /// <param name="obj"></param>
        /// <returns>Deep clone of an object</returns>
        public static T DeepClone<T>(this T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Reads all characters from the current position of Stream to the end.
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>String content of stream</returns>
        public static string TxtToString(Stream stream)
        {
            var rdr = new StreamReader(stream);
            return rdr.ReadToEnd();
        }

        //***** EDUCATION EXAMPLES ***//

        /// <summary>
        /// Gets string representation of array of doubles.
        /// </summary>
        /// <param name="numbers">Array of doubles</param>
        /// <returns>String representation</returns>
        public static string DoubleArrayToString(double[] numbers)
        {
            string output = "\t";
            foreach (double number in numbers)
            {
                output += String.Format("{0:0.##}\t", number);
            }
            return output;
        }

        /// <summary>
        /// Creates a deep clone of an object using BinaryFormatter.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Deep clone of an object</returns>
        public static object DeepClone2(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Finds bigger double number.
        /// </summary>
        /// <param name="x">First doublenumber</param>
        /// <param name="y">Second double number</param>
        /// <returns>Bigger double number out of two</returns>
        public static int Max(int x, int y)
        {
            return (x > y) ? x : y;
        }

        /// <summary>
        /// Finds bigger int number.
        /// </summary>
        /// <param name="x">First int number</param>
        /// <param name="y">Second int number</param>
        /// <returns>Bigger int number out of two</returns>
        public static double Max(double x, double y)
        {
            return (x > y) ? x : y;
        }

        /// <summary>
        /// Finds bigger number out of two in any format.
        /// </summary>
        /// <param name="x">First number</param>
        /// <param name="y">Second number</param>
        /// <returns>Bigger number out of two</returns>
        public static T Max<T>(T x, T y) where T : IComparable
        {
            return (x.CompareTo(y) > 0) ? x : y;
        }

        /// <summary>
        /// Random method
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Abreviate(this string text, int length)
        {
            List<int> list = new List<int>();
            list.Add(3);
            //list.Add("fgsafgsa");
            ArrayList list2 = new ArrayList();
            list2.Add(5523);
            list2.Add("sfafas");

            //MatrixRow row = new MatrixRow(null, new Rational(1, 1));
            //var row2 = DeepClone<object>(row);
            //var row3 = (MatrixRow)DeepClone2(row);
            double d1 = 2.3;
            double d2 = 33.11;
            Utils.Max(d1, d2);

            return (text.Length > length - 3) ? text.Remove(length - 3) + "..." : text;
        }

        
        //public static int TryParseStringToInt(string str)
        //{
        //    int number;
        //    try
        //    {
        //        number = Int32.Parse(str);
        //    }
        //    catch (FormatException)
        //    {
        //        return PARSING_ERROR;
        //    }
        //    catch (OverflowException)
        //    {
        //        return PARSING_ERROR;
        //    }
        //    return number;
        //}

        /// <summary>
        /// Constant used to declare error occurance during parsing string into int.
        /// </summary>
        public const int PARSING_ERROR = 21845;
    }
}
