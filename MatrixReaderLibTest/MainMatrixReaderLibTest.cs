using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixReaderLib;
using MatrixLib;
using System.IO;
using System.Text;
using RationalLib;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace MatrixReaderLibTest
{
    /// <summary>
    /// Class made for creation of Rows in JSON file.
    /// </summary>
    public class Row
    {
        /// <summary>Coefficients of a MatrixRow</summary>
        public double[] Coefs;
        /// <summary>Result of a MatrixRow</summary>
        public double Result;
    }

    /// <summary>
    /// Class made for creation of JSON file.
    /// </summary>
    public class GetMatrixJson
    {
        /// <summary>Height of a Matrix</summary>
        public int Height;
        /// <summary>Rows of a Matrix</summary>
        public Row[] Rows;
        /// <summary>Width of a Matrix</summary>
        public int Width;
    }

    /// <summary>
    /// Main class for execution of tests of MatrixReaderLib namespace.
    /// </summary>
    [TestClass]
    public class MainMatrixReaderLibTest
    {
        /// <summary>
        /// Tests Load of matrix with TxtMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_TxtMatrixReader_LoadEqualMatrixes()
        {
            TxtMatrixReader matrixLoader = new TxtMatrixReader();
            Matrix matrix = new Matrix();
            string matrixString = "1 2 3 4 5\n-1 -2 -3 -4 -5\n1 -2 3 -4 -5\n4,5 2,6 1,8 6,5 3";
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(matrixString));

            matrix.Rows.Add(new MatrixRow(new Rational[] { 1, 2, 3, 4 }, 5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { -1, -2, -3, -4 }, -5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { 1, -2, 3, -4 }, -5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { 4.5, 2.6, 1.8, 6.5 }, 3));

            Matrix loadMatrix = matrixLoader.Load(stream);
            Assert.AreEqual(matrix, loadMatrix);
        }

        /// <summary>
        /// Tests Load of matrix with TxtMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_TxtMatrixReader_LoadDifferentMatrixes()
        {
            TxtMatrixReader matrixLoader = new TxtMatrixReader();
            Matrix matrix = new Matrix();
            string matrixString = "1 2 3 4 5\n-1 -2 -3 -4 -5\n1 -2 3 -4 -5\n4,5 2,6 1,8 6,5 3";
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(matrixString));

            matrix.Rows.Add(new MatrixRow(new Rational[] { 1, 2, 3, 4 }, 5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { -1, -2, -3, -4 }, -5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { 1, -2, 3, -4 }, -5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { 4.5, 2.6, 1.7, 6.5 }, 3));

            Matrix loadMatrix = matrixLoader.Load(stream);
            Assert.AreNotEqual(matrix, loadMatrix);
        }

        /// <summary>
        /// Tests Load of matrix with TxtMatrixReader from wrong format string.
        /// </summary>
        [TestMethod]
        public void Test_MatrixLoad_WrongFormat()
        {
            TxtMatrixReader matrixLoader = new TxtMatrixReader();
            Matrix matrix;
            string badMatrix = "1 2 3 4 -3\n-3 8 -9 10 -34\n4 9 -6 5 -14,5\n8 1 3 3 ";
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(badMatrix));

            try
            {
                matrix = matrixLoader.Load(stream);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        /// <summary>
        /// Tests Load of matrix with TxtMatrixReader from string with missing number.
        /// </summary>
        [TestMethod]
        public void Test_MatrixLoad_MissingNumber()
        {
            TxtMatrixReader matrixLoader = new TxtMatrixReader();
            Matrix matrix;
            string badMatrix = "1 2 3 4 -3\n-3 8 -9 10 -34\n4 9 -6 5 -14,5\n8 1 3 3";
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(badMatrix));
            try
            {
                matrix = matrixLoader.Load(stream);
                Assert.Fail();
            }
            catch (ApplicationException)
            {
            }
        }

        /// <summary>
        /// Tests Load of matrix with BinMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_BinMatrixReader_LoadEqualMatrixes()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            double[,] matrixLines = new double[4, 5] {
                { 1, 2, 3, 4, 5 },
                { -1, -2, -3, -4, -5 },
                { 1, -2, 3, -4, -5 },
                { 4.5, 2.6, 1.7, 6.5, 3 }
            };

            writer.Write((Int32)4);
            writer.Write((Int32)4);
            writer.Write((byte)0x01);
            //lines
            for (int i = 0; i < 4; i++)
            {
                //columns
                for (int j = 0; j < 5; j++)
                {
                    writer.Write((double)matrixLines[i, j]);
                }
                writer.Write((byte)0xCC);
            }
            writer.Write((byte)0xEE);
            writer.Write((byte)0xFF);
            memoryStream.Position = 0;

            BinMatrixReader matrixLoader = new BinMatrixReader();
            Matrix loadMatrix = matrixLoader.Load(memoryStream);

            Matrix matrix = new Matrix();
            matrix.Rows.Add(new MatrixRow(new Rational[] { 1, 2, 3, 4 }, 5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { -1, -2, -3, -4 }, -5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { 1, -2, 3, -4 }, -5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { 4.5, 2.6, 1.7, 6.5 }, 3));

            Assert.AreEqual(matrix, loadMatrix);
        }

        /// <summary>
        /// Tests Validation of MatrixVersion 1 with BinMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_BinMatrixReader_ValidateMatrixVersion_CorrectVersion_Version1()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            double[,] matrixLines = new double[4, 5] {
                { 1, 2, 3, 4, 5 },
                { -1, -2, -3, -4, -5 },
                { 1, -2, 3, -4, -5 },
                { 4.5, 2.6, 1.7, 6.5, 3 }
            };

            writer.Write((Int32)4);
            writer.Write((Int32)4);
            writer.Write((byte)0x01);
            //lines
            for (int i = 0; i < 4; i++)
            {
                //columns
                for (int j = 0; j < 5; j++)
                {
                    writer.Write((double)matrixLines[i, j]);
                }
                writer.Write((byte)0xCC);
            }
            writer.Write((byte)0xEE);
            writer.Write((byte)0xFF);
            memoryStream.Position = 0;

            BinMatrixReader matrixLoader = new BinMatrixReader();
            try
            {
                Matrix loadMatrix = matrixLoader.Load(memoryStream);
            }
            catch (Exception)
            {
                //if (e.Message == "Wrong matrix version.")
                //{
                    Assert.Fail();
                //}
            }
        }

        /// <summary>
        /// Tests Validation of MatrixVersion 2 with BinMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_BinMatrixReader_ValidateMatrixVersion_CorrectVersion_Version2()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            double[,] matrixLines = new double[4, 5] {
                { 1, 2, 3, 4, 5 },
                { -1, -2, -3, -4, -5 },
                { 1, -2, 3, -4, -5 },
                { 4.5, 2.6, 1.7, 6.5, 3 }
            };

            writer.Write((Int32)4);
            writer.Write((Int32)4);
            writer.Write((byte)0x02);
            //lines
            for (int i = 0; i < 4; i++)
            {
                //columns
                for (int j = 0; j < 5; j++)
                {
                    writer.Write((double)matrixLines[i, j]);
                }
                writer.Write((byte)0xC0);
            }
            writer.Write((Int16)0x1386);
            writer.Write((byte)0xEE);
            writer.Write((byte)0xFF);
            memoryStream.Position = 0;

            BinMatrixReader matrixLoader = new BinMatrixReader();
            try
            {
                Matrix loadMatrix = matrixLoader.Load(memoryStream);
            }
            catch (Exception)
            {
                //if (e.Message == "Wrong matrix version.")
                //{
                    Assert.Fail();
                //}
            }
        }

        /// <summary>
        /// Tests Validation of wrong MatrixVersion with BinMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_BinMatrixReader_ValidateMatrixVersion_IncorrectVersion()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            double[,] matrixLines = new double[4, 5] {
                { 1, 2, 3, 4, 5 },
                { -1, -2, -3, -4, -5 },
                { 1, -2, 3, -4, -5 },
                { 4.5, 2.6, 1.7, 6.5, 3 }
            };

            writer.Write((Int32)4);
            writer.Write((Int32)4);
            writer.Write((byte)0x03);
            //lines
            for (int i = 0; i < 4; i++)
            {
                //columns
                for (int j = 0; j < 5; j++)
                {
                    writer.Write((double)matrixLines[i, j]);
                }
                writer.Write((byte)0xCC);
            }
            writer.Write((byte)0xEE);
            writer.Write((byte)0xFF);
            memoryStream.Position = 0;

            BinMatrixReader matrixLoader = new BinMatrixReader();
            try
            {
                Matrix loadMatrix = matrixLoader.Load(memoryStream);
            }
            catch (Exception)
            {
                //if (e.Message == "Wrong matrix version.")
                //{
                    return;
                //}
            }
            Assert.Fail();
        }

        /// <summary>
        /// Tests Validation of RowFooter with BinMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_BinMatrixReader_ValidateRowFooter_CorrectVersion()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            double[,] matrixLines = new double[4, 5] {
                { 1, 2, 3, 4, 5 },
                { -1, -2, -3, -4, -5 },
                { 1, -2, 3, -4, -5 },
                { 4.5, 2.6, 1.7, 6.5, 3 }
            };

            writer.Write((Int32)4);
            writer.Write((Int32)4);
            writer.Write((byte)0x01);
            //lines
            for (int i = 0; i < 4; i++)
            {
                //columns
                for (int j = 0; j < 5; j++)
                {
                    writer.Write((double)matrixLines[i, j]);
                }
                writer.Write((byte)0xCC);
            }
            writer.Write((byte)0xEE);
            writer.Write((byte)0xFF);
            memoryStream.Position = 0;

            BinMatrixReader matrixLoader = new BinMatrixReader();
            try
            {
                Matrix loadMatrix = matrixLoader.Load(memoryStream);
            }
            catch (Exception)
            {
                //if (e.Message == "Row validation not successful.")
                //{
                    Assert.Fail();
                //}
            }
        }

        /// <summary>
        /// Tests Validation of RowFooter with BinMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_BinMatrixReader_ValidateRowFooter_IncorrectVersion()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            double[,] matrixLines = new double[4, 5] {
                { 1, 2, 3, 4, 5 },
                { -1, -2, -3, -4, -5 },
                { 1, -2, 3, -4, -5 },
                { 4.5, 2.6, 1.7, 6.5, 3 }
            };

            writer.Write((Int32)4);
            writer.Write((Int32)4);
            writer.Write((byte)0x01);
            //lines
            for (int i = 0; i < 4; i++)
            {
                //columns
                for (int j = 0; j < 5; j++)
                {
                    writer.Write((double)matrixLines[i, j]);
                }
                writer.Write((byte)0xCD);
            }
            writer.Write((byte)0xEE);
            writer.Write((byte)0xFF);
            memoryStream.Position = 0;

            BinMatrixReader matrixLoader = new BinMatrixReader();
            try
            {
                Matrix loadMatrix = matrixLoader.Load(memoryStream);
            }
            catch (Exception)
            {
                //if (e.Message == "Row validation not successful.")
                //{
                    return;
                //}
            }
            Assert.Fail();
        }

        /// <summary>
        /// Tests Validation of MatrixFooter with BinMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_BinMatrixReader_ValidateMatrixFooter_CorrectVersion()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            double[,] matrixLines = new double[4, 5] {
                { 1, 2, 3, 4, 5 },
                { -1, -2, -3, -4, -5 },
                { 1, -2, 3, -4, -5 },
                { 4.5, 2.6, 1.7, 6.5, 3 }
            };

            writer.Write((Int32)4);
            writer.Write((Int32)4);
            writer.Write((byte)0x01);
            //lines
            for (int i = 0; i < 4; i++)
            {
                //columns
                for (int j = 0; j < 5; j++)
                {
                    writer.Write((double)matrixLines[i, j]);
                }
                writer.Write((byte)0xCC);
            }
            writer.Write((byte)0xEE);
            writer.Write((byte)0xFF);
            memoryStream.Position = 0;


            BinMatrixReader matrixLoader = new BinMatrixReader();
            try
            {
                Matrix loadMatrix = matrixLoader.Load(memoryStream);
            }
            catch (Exception)
            {
                //if (e.Message == "Wrong matrix footer.")
                //{
                    Assert.Fail();
                //}
            }
        }

        /// <summary>
        /// Tests Validation of MatrixFooter with BinMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_BinMatrixReader_ValidateMatrixFooter_IncorrectVersion()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            double[,] matrixLines = new double[4, 5] {
                { 1, 2, 3, 4, 5 },
                { -1, -2, -3, -4, -5 },
                { 1, -2, 3, -4, -5 },
                { 4.5, 2.6, 1.7, 6.5, 3 }
            };

            writer.Write((Int32)4);
            writer.Write((Int32)4);
            writer.Write((byte)0x01);
            //lines
            for (int i = 0; i < 4; i++)
            {
                //columns
                for (int j = 0; j < 5; j++)
                {
                    writer.Write((double)matrixLines[i, j]);
                }
                writer.Write((byte)0xCC);
            }
            writer.Write((byte)0xEF);
            writer.Write((byte)0xFF);
            memoryStream.Position = 0;

            BinMatrixReader matrixLoader = new BinMatrixReader();
            try
            {
                Matrix loadMatrix = matrixLoader.Load(memoryStream);
            }
            catch (Exception)
            {
                //if (e.Message == "Wrong matrix footer.")
                //{
                    return;
                //}
            }
            Assert.Fail();
        }

        /// <summary>
        /// Tests Validation of CheckSum with BinMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_BinMatrixReader_ValidateCheckSum_CorrectVersion()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            double[,] matrixLines = new double[4, 5] {
                { 1, 2, 3, 4, 5 },
                { -1, -2, -3, -4, -5 },
                { 1, -2, 3, -4, -5 },
                { 4.5, 2.6, 1.7, 6.5, 3 }
            };

            writer.Write((Int32)4);
            writer.Write((Int32)4);
            writer.Write((byte)0x02);
            //lines
            for (int i = 0; i < 4; i++)
            {
                //columns
                for (int j = 0; j < 5; j++)
                {
                    writer.Write((double)matrixLines[i, j]);
                }
                writer.Write((byte)0xC0);
            }
            writer.Write((Int16)0x1386);
            writer.Write((byte)0xEE);
            writer.Write((byte)0xFF);
            memoryStream.Position = 0;

            BinMatrixReader matrixLoader = new BinMatrixReader();
            try
            {
                Matrix loadMatrix = matrixLoader.Load(memoryStream);
            }
            catch (Exception)
            {
                //if (e.Message == "CheckSum value doesn't match.")
                //{
                    Assert.Fail();
                //}
            }
        }

        /// <summary>
        /// Tests Validation of CheckSum with BinMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_BinMatrixReader_ValidateCheckSum_IncorrectVersion()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            double[,] matrixLines = new double[4, 5] {
                { 1, 2, 3, 4, 5 },
                { -1, -2, -3, -4, -5 },
                { 1, -2, 3, -4, -5 },
                { 4.5, 2.6, 1.7, 6.5, 3 }
            };

            writer.Write((Int32)4);
            writer.Write((Int32)4);
            writer.Write((byte)0x02);
            //lines
            for (int i = 0; i < 4; i++)
            {
                //columns
                for (int j = 0; j < 5; j++)
                {
                    writer.Write((double)matrixLines[i, j]);
                }
                writer.Write((byte)0xC0);
            }
            writer.Write((Int16)0x1387);
            writer.Write((byte)0xEE);
            writer.Write((byte)0xFF);
            memoryStream.Position = 0;

            BinMatrixReader matrixLoader = new BinMatrixReader();
            try
            {
                Matrix loadMatrix = matrixLoader.Load(memoryStream);
            }
            catch (Exception)
            {
                //if (e.Message == "CheckSum value doesn't match.")
                //{
                    return;
                //}
            }
            Assert.Fail();
        }

        /// <summary>
        /// Tests Load of matrix with XmlMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_XmlMatrixReader_Load()
        {
            XmlMatrixReader matrixLoader = new XmlMatrixReader();
            Matrix matrix = new Matrix();

            XDocument doc = new XDocument(new XElement("Matrix",
                                            new XElement("Width", (Int32)4),
                                            new XElement("Height", (Int32)4),
                                            new XElement("Rows",
                                                new XElement("MatrixRow",
                                                    new XElement("Coefs",
                                                        new XElement("double", (double)1),
                                                        new XElement("double", (double)2),
                                                        new XElement("double", (double)3),
                                                        new XElement("double", (double)4)),
                                                    new XElement("Result", (double)5)),
                                                new XElement("MatrixRow",
                                                    new XElement("Coefs",
                                                        new XElement("double", (double)-1),
                                                        new XElement("double", (double)-2),
                                                        new XElement("double", (double)-3),
                                                        new XElement("double", (double)-4)),
                                                    new XElement("Result", (double)-5)),
                                                new XElement("MatrixRow",
                                                    new XElement("Coefs",
                                                        new XElement("double", (double)1),
                                                        new XElement("double", (double)-2),
                                                        new XElement("double", (double)3),
                                                        new XElement("double", (double)-4)),
                                                    new XElement("Result", (double)-5)),
                                                new XElement("MatrixRow",
                                                    new XElement("Coefs",
                                                        new XElement("double", (double)4.5),
                                                        new XElement("double", (double)2.6),
                                                        new XElement("double", (double)1.8),
                                                        new XElement("double", (double)6.5)),
                                                    new XElement("Result", (double)3))
                                            )));

            //string matrixString = "<matrix><rows></rows></matrix>";
            //MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(matrixString));


            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            stream.Position = 0;

            matrix.Rows.Add(new MatrixRow(new Rational[] { 1, 2, 3, 4 }, 5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { -1, -2, -3, -4 }, -5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { 1, -2, 3, -4 }, -5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { 4.5, 2.6, 1.8, 6.5 }, 3));

            Matrix loadMatrix = matrixLoader.Load(stream);
            Assert.AreEqual(matrix, loadMatrix);
        }

        /// <summary>
        /// Tests Load of matrix with JsonMatrixReader.
        /// </summary>
        [TestMethod]
        public void Test_JsonMatrixReader_Load()
        {
            JsonMatrixReader matrixLoader = new JsonMatrixReader();
            Matrix matrix = new Matrix();

            GetMatrixJson gm = new GetMatrixJson()
            {
                Height = 4,
                Rows = new Row[4]
                {
                    new Row { Coefs = new double[4] { 1, 2, 3, 4 }, Result = 5 },
                    new Row { Coefs = new double[4] { 1, 2, 3, 4 }, Result = 5 },
                    new Row { Coefs = new double[4] { 1, 2, 3, 4 }, Result = 5 },
                    new Row { Coefs = new double[4] { 1, 2, 3, 4 }, Result = 5 }
                },
                Width = 4
            };

            string matrixJsonString = JsonConvert.SerializeObject(gm);
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(matrixJsonString));
            
            matrix.Rows.Add(new MatrixRow(new Rational[] { 1, 2, 3, 4 }, 5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { 1, 2, 3, 4 }, 5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { 1, 2, 3, 4 }, 5));
            matrix.Rows.Add(new MatrixRow(new Rational[] { 1, 2, 3, 4 }, 5));

            Matrix loadMatrix = matrixLoader.Load(stream);
            Assert.AreEqual(matrix, loadMatrix);
        }
    }
}