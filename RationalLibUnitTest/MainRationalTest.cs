using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RationalLib;
using System.Collections.Generic;
using Common;

namespace RationalLibUnitTest
{
    /// <summary>
    /// Main class for execution of tests of Rational class.
    /// </summary>
    [TestClass]
    public class MainRationalTest
    {
        /// <summary>
        /// Tests Parse method of Rational class.
        /// </summary>
        [TestMethod]
        public void TestRationalPrecision()
        {
            var r1 = Rational.Parse("1.000001");
            var r2 = Rational.Parse("0.000001");

            Assert.AreEqual(new Rational(1, 1), (r1 - r2));
        }
        /// <summary>
        /// Tests Parse method of Rational class with implicit Rational operator used.
        /// </summary>
        [TestMethod]
        public void TestRationalPrecisionInt()
        {
            var r1 = Rational.Parse("1.000001");
            var r2 = Rational.Parse("0.000001");

            Assert.AreEqual(1, (r1 - r2));
        }

        /// <summary>
        /// Tests plus operator of Rational class with two positive rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_PlusOperatorPositive()
        {
            Rational number1 = new Rational(5, 6);
            Rational number2 = new Rational(2, 12);

            Assert.AreEqual(new Rational(1, 1), number1 + number2);
        }

        /// <summary>
        /// Tests plus operator of Rational class with positive and negative rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_PlusOperatorNegative()
        {
            Rational number1 = new Rational(5, 6);
            Rational number2 = new Rational(-2, 12);

            Assert.AreEqual(new Rational(2, 3), number1 + number2);
        }

        /// <summary>
        /// Tests plus operator of Rational class with two positive rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_MinusOperatorPositive()
        {
            Rational number1 = new Rational(10, 6);
            Rational number2 = new Rational(2, 12);

            Assert.AreEqual(new Rational(3, 2), number1 - number2);
        }

        /// <summary>
        /// Tests minus operator of Rational class with positive and negative rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_MinusOperatorNegative()
        {
            Rational number1 = new Rational(10, 6);
            Rational number2 = new Rational(-2, 12);

            Assert.AreEqual(new Rational(11, 6), number1 - number2);
        }

        /// <summary>
        /// Tests multiply operator of Rational class with two positive rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_MulOperatorPositive()
        {
            Rational number1 = new Rational(10, 7);
            Rational number2 = new Rational(2, 12);

            Assert.AreEqual(new Rational(5, 21), number1 * number2);
        }

        /// <summary>
        /// Tests multiply operator of Rational class with positive and negative rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_MulOperatorNegative()
        {
            Rational number1 = new Rational(10, 7);
            Rational number2 = new Rational(-2, 12);

            Assert.AreEqual(new Rational(-5, 21), number1 * number2);
        }

        /// <summary>
        /// Tests division operator of Rational class with two positive rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_DivisionOperatorPositive()
        {
            Rational number1 = new Rational(10, 6);
            Rational number2 = new Rational(2, 12);

            Assert.AreEqual(new Rational(10, 1), number1 / number2);
        }

        /// <summary>
        /// Tests division operator of Rational class with positive and negative rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_DivisionOperatorNegative()
        {
            Rational number1 = new Rational(10, 6);
            Rational number2 = new Rational(-2, 12);

            Assert.AreEqual(new Rational(10, -1), number1 / number2);
        }

        /// <summary>
        /// Tests compare operator of Rational class with two positive rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_CompareOperatorPositive()
        {
            Rational number1 = new Rational(10, 6);
            Rational number2 = new Rational(5, 3);

            if (number1 == number2)
            {
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests compare operator of Rational class with negative rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_EqualOperatorNegative()
        {
            Rational number1 = new Rational(-10, 6);
            Rational number2 = new Rational(5, -3);

            if (number1 == number2)
            {
                return;
            }
            Assert.Fail();
        }

        /// <summary>
        /// Tests negated compare operator of Rational class with positive rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_UnequalOperatorPositive()
        {
            Rational number1 = new Rational(9, 6);
            Rational number2 = new Rational(5, 3);

            if (number1 != number2)
            {
                return;
            }
            Assert.Fail();
        }

        /// <summary>
        /// Tests negated compare operator of Rational class with positive and negative rational number.
        /// </summary>
        [TestMethod]
        public void Test_UnequalOperatorNegative()
        {
            Rational number1 = new Rational(10, 6);
            Rational number2 = new Rational(5, -3);

            if (number1 != number2)
            {
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests is bigger operator of Rational class with positive rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_BiggerOperatorPositive()
        {
            Rational number1 = new Rational(5, 3);
            Rational number2 = new Rational(9, 6);

            if (number1 > number2)
            {
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests is bigger operator of Rational class with positive and negative rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_BiggerOperatorNegative()
        {
            Rational number1 = new Rational(10, 6);
            Rational number2 = new Rational(5, -3);

            if (number1 > number2)
            {
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests is smaller operator of Rational class with positive rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_SmallerOperatorPositive()
        {
            Rational number1 = new Rational(9, 6);
            Rational number2 = new Rational(5, 3);

            if (number1 < number2)
            {
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests is smaller operator of Rational class with positive and negative rational numbers.
        /// </summary>
        [TestMethod]
        public void Test_SmallerOperatorNegative()
        {
            Rational number1 = new Rational(5, -3);
            Rational number2 = new Rational(10, 6);

            if (number1 < number2)
            {
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests GetDigitsAmount method of Utils class with five digit number.
        /// </summary>
        [TestMethod]
        public void Test_GetDigitsAmount_FiveDigitNumber()
        {
            Assert.AreEqual(5, Utils.GetDigitsAmount(12345));
        }

        /// <summary>
        /// Tests GetSmallestCommonMultiple method of Utils class with two prime numbers.
        /// </summary>
        [TestMethod]
        public void Test_GetSmallestCommonMultiple_TwoPrimeNumbers()
        {
            Assert.AreEqual(143, Utils.GetSmallestCommonMultiple(11, 13));
        }

        /// <summary>
        /// Tests GetSmallestCommonMultiple method of Utils class with five two splittable numbers.
        /// </summary>
        [TestMethod]
        public void Test_GetSmallestCommonMultiple_TwoSplittableNumbers()
        {
            Assert.AreEqual(120, Utils.GetSmallestCommonMultiple(60, 24));
        }

        /// <summary>
        /// Tests GetPartialDividors method of Utils class with splittable number.
        /// </summary>
        [TestMethod]
        public void Test_GetPartialDividors()
        {
            List<int> partialDividors = new List<int>
            {
                2, 2, 2, 3, 5
            };
            CollectionAssert.AreEqual(partialDividors, Utils.GetPartialDividors(120));
        }

        /// <summary>
        /// Tests GetListOfCommonPartialDividors method of Utils class with two splittable numbers.
        /// </summary>
        [TestMethod]
        public void Test_GetListOfCommonPartialDividors()
        {
            List<int> commonPartialDividors = new List<int>
            {
                1, 2, 2, 3, 5, 7
            };
            CollectionAssert.AreEqual(commonPartialDividors, Utils.GetListOfCommonPartialDividors(5460, 7140));
        }

        /// <summary>
        /// Tests GetHighestCommonDividor method of Utils class with two splittable numbers.
        /// </summary>
        [TestMethod]
        public void Test_GetHighestCommonDividor()
        {
            Assert.AreEqual(84, Utils.GetHighestCommonDividor(840, 12012));
        }
    }
}