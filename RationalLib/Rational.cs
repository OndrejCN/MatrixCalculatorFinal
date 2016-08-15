using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace RationalLib
{
    /// <summary>
    /// Struct being used for calculation in rational numbers.
    /// </summary>
    [Serializable]
    public struct Rational
    {
        /// <summary>Numerator of fracion</summary>
        public int Numerator { get; set; }
        /// <summary>Denominator of fraction</summary>
        public int Denominator { get; set; }

        bool IsNegative
        {
            get
            {
                if (Numerator < 0 && Denominator >= 0 || Numerator >= 0 && Denominator < 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Constructs a rational number in base cast.
        /// </summary>
        /// <param name="numerator">Numerator of fraction</param>
        /// <param name="denominator">Denominator of fraction</param>
        public Rational(int numerator, int denominator)
        {
            this.Numerator = numerator;
            if (denominator == 0)
            {
                throw new DivideByZeroException();
                //denominator = 1;
            }
            this.Denominator = denominator;
            this.TransformToBaseCast();
        }

        /// <summary>
        /// Parses string into rational number.
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Rational number</returns>
        public static Rational Parse(string str)
        {
            str = str.Replace(",", ".");
            string number = str.Replace(".", "");
            int index = str.LastIndexOf(".");
            int length = (index == -1) ? -1 : (str.Length - 1 - index);

            if (length == -1)
            {
                return new Rational(Int32.Parse(number), 1);
            }

            return new Rational(Int32.Parse(number), (int)Math.Pow(10, length));
        }
        
        /// <summary>
        /// Sums two rational numbers.
        /// </summary>
        /// <param name="r1">First number</param>
        /// <param name="r2">Second number</param>
        /// <returns>Summarized rational number</returns>
        public static Rational operator +(Rational r1, Rational r2)
        {
            checked
            {
                int smallestDenominatorMultiple =
                    Utils.GetSmallestCommonMultiple(r1.Denominator, r2.Denominator);

                int partialNumerator = smallestDenominatorMultiple / r1.Denominator * r1.Numerator +
                    smallestDenominatorMultiple / r2.Denominator * r2.Numerator;

                return new Rational(partialNumerator, smallestDenominatorMultiple);
            }
        }

        /// <summary>
        /// Subtracts two rational numbers.
        /// </summary>
        /// <param name="r1">First number</param>
        /// <param name="r2">Second number</param>
        /// <returns>Subtracted rational number</returns>
        public static Rational operator -(Rational r1, Rational r2)
        {
            checked
            {
                int smallestDenominatorMultiple =
                    Utils.GetSmallestCommonMultiple(r1.Denominator, r2.Denominator);

                int partialNumerator = ((smallestDenominatorMultiple / r1.Denominator) * r1.Numerator) - ((smallestDenominatorMultiple / r2.Denominator) * r2.Numerator);

                return new Rational(partialNumerator, smallestDenominatorMultiple);
            }
        }

        /// <summary>
        /// Multiplies two rational numbers.
        /// </summary>
        /// <param name="r1">First number</param>
        /// <param name="r2">Second number</param>
        /// <returns>Multiplied rational number</returns>
        public static Rational operator *(Rational r1, Rational r2)
        {
            checked
            {
                return new Rational(r1.Numerator * r2.Numerator, r1.Denominator * r2.Denominator);
            }
        }

        /// <summary>
        /// Divides two rational numbers.
        /// </summary>
        /// <param name="r1">First number</param>
        /// <param name="r2">Second number</param>
        /// <returns>Divided rational number</returns>
        public static Rational operator /(Rational r1, Rational r2)
        {
            checked
            {
                return new Rational(r1.Numerator * r2.Denominator, r1.Denominator * r2.Numerator);
            }
        }

        /// <summary>
        /// Compares values of two rational numbers.
        /// </summary>
        /// <param name="r1">First number</param>
        /// <param name="r2">Second number</param>
        /// <returns>Confirmation if values of two rational numbers are equal.</returns>
        public static bool operator ==(Rational r1, Rational r2)
        {
            r1.TransformToBaseCast();
            r2.TransformToBaseCast();

            if (r1.Numerator == r2.Numerator && r1.Denominator == r2.Denominator)
            {
                return true;
            }
            if ((r1.IsNegative && r2.IsNegative) && 
                (Math.Abs(r1.Numerator) == Math.Abs(r2.Numerator)) && 
                (Math.Abs(r1.Denominator) == Math.Abs(r2.Denominator)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Compares values of two rational numbers.
        /// </summary>
        /// <param name="r1">First number</param>
        /// <param name="r2">Second number</param>
        /// <returns>Confirmation if values of two rational numbers are not equal.</returns>
        public static bool operator !=(Rational r1, Rational r2)
        {
            if (!(r1 == r2))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Compares values of two rational numbers.
        /// </summary>
        /// <param name="r1">First number</param>
        /// <param name="r2">Second number</param>
        /// <returns>Confirmation if value of first rational number is higher than the other one.</returns>
        public static bool operator >(Rational r1, Rational r2)
        {
            int smallestDenominatorMultiple =
                Utils.GetSmallestCommonMultiple(r1.Denominator, r2.Denominator);
            if (smallestDenominatorMultiple / r1.Denominator * r1.Numerator > smallestDenominatorMultiple / r2.Denominator * r2.Numerator)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Compares values of two rational numbers.
        /// </summary>
        /// <param name="r1">First number</param>
        /// <param name="r2">Second number</param>
        /// <returns>Confirmation if value of first rational number is lower than the other one.</returns>
        public static bool operator <(Rational r1, Rational r2)
        {
            if (!(r1 > r2))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Implicit operator of Rational struct.
        /// </summary>
        /// <param name="value">Integer value of rational number</param>
        public static implicit operator Rational(int value)
        {
            return new Rational(value, 1);
        }

        /// <summary>
        /// Implicit operator of Rational struct.
        /// </summary>
        /// <param name="value">Double value of rational number</param>
        public static implicit operator Rational(double value)
        {
            int decimalPlaces = 5;
            int denumerator = (int)Math.Pow(10, decimalPlaces);

            double roundedNumber = Math.Round(value, decimalPlaces);
            roundedNumber *= denumerator;

            return new Rational((int)roundedNumber, denumerator);
        }

        /// <summary>
        /// Compares rational number to other object.
        /// </summary>
        /// <param name="obj">Object to compare with</param>
        /// <returns>Confirmation if object is instance of Rational and has same values as 
        /// this rational number.</returns>
        public override bool Equals(object obj)
        {
            if (obj is int)
            {
                obj = new Rational((int)obj, 1);
            }
            else if (!(obj is Rational))
            {
                return false;
            }
            Rational rationalToCompare = (Rational)obj;

            this.TransformToBaseCast();
            rationalToCompare.TransformToBaseCast();

            if (this.Numerator == rationalToCompare.Numerator && this.Denominator == rationalToCompare.Denominator)
            {
                return true;
            }
            if ((this.IsNegative && rationalToCompare.IsNegative) && (Math.Abs(this.Numerator) == Math.Abs(rationalToCompare.Numerator)) && (Math.Abs(this.Denominator) == Math.Abs(rationalToCompare.Denominator)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets hash code of rational number.
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return Numerator.GetHashCode() ^ Denominator.GetHashCode();
        }

        /// <summary>
        /// Gets string representation of rational number.
        /// </summary>
        /// <returns>String representation of rational number</returns>
        public override string ToString()
        {
            if (this.Denominator == 1)
            {
                return this.Numerator.ToString();
            }

            int digitCount = Utils.GetDigitsAmount(Numerator) > Utils.GetDigitsAmount(Denominator) ?
                Utils.GetDigitsAmount(Numerator) : Utils.GetDigitsAmount(Denominator);

            string numeratorString = NumberToStringWithSpaces(Numerator, digitCount);
            string denominatorString = NumberToStringWithSpaces(Denominator, digitCount);
            string returnString = numeratorString + System.Environment.NewLine;

            if (this.IsNegative)
            {
                returnString += "- ";
            }
            for (int i = 0; i < digitCount; i++)
            {
                returnString += "-";
            }
            return returnString + System.Environment.NewLine + denominatorString;
        }

        private string NumberToStringWithSpaces(int number, int digitCount)
        {
            if (this.IsNegative)
            {
                return Math.Abs(number).ToString().PadLeft(digitCount + 2);
            }
            return number.ToString().PadLeft(digitCount);
        }

        private void TransformToBaseCast()
        {
            if (this.Numerator == 0)
            {
                this.Denominator = 1;
            }

            int dividor = Utils.GetHighestCommonDividor(this.Numerator, this.Denominator);
            Numerator /= dividor;
            Denominator /= dividor;

            ProcessNegative();
        }

        /// <summary>
        /// get rid of negative sign if both numerator and denominator are negative
        /// </summary>
        private void ProcessNegative()
        {
            if (this.Numerator < 0 && this.Denominator < 0)
            {
                this.Numerator *= -1;
                this.Denominator *= -1;
            }
        }

        /// <summary>
        /// Transforms Rational number into double.
        /// </summary>
        /// <param name="number">Rational number to tranform.</param>
        /// /// <param name="decimalPlaces">Amount of decimal places of double output.</param>
        /// <returns>Double representation of Rational number.</returns>
        public static double RationalToDouble(Rational number, int decimalPlaces)
        {
            double numberR = (double)number.Numerator / (double)number.Denominator;
            return Math.Round(numberR, decimalPlaces);
        }
    }
}