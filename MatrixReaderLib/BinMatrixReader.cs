using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLib;
using RationalLib;
using Common;

namespace MatrixReaderLib
{
    /// <summary>
    /// Class for reading BIN file content and turning it into Matrix class instance.
    /// </summary>
    public class BinMatrixReader : IMatrixReader
    {
        /// <summary>
        /// Reads stream and turns it into Matrix class instance.
        /// </summary>
        /// <param name="stream">Stream, which contains Matrix data</param>
        /// <returns>Matrix instance</returns>
        public Matrix Load(Stream stream)
        {
            var bin = new BinaryReader(stream);
            int width, height;
            byte matrixVersion;
            short checkSum = 0;

            GetDimensions(bin, out width, out height, ref checkSum);
            matrixVersion = bin.ReadByte();
            if (matrixVersion != MATRIX_VERSION1 && matrixVersion != MATRIX_VERSION2)
            {
                throw new ApplicationException("Wrong matrix version.");
            }

            Matrix matrix = GetBinMatrix(width, height, bin, matrixVersion, ref checkSum);

            if (matrixVersion == MATRIX_VERSION2)
            {
                if (checkSum != bin.ReadInt16())
                {
                    throw new ApplicationException("CheckSum value doesn't match.");
                }
            }

            if (!ValidateMatrixFooter(bin))
            {
                throw new ApplicationException("Wrong matrix footer.");
            }
            return matrix;
        }

        private Matrix GetBinMatrix(int width, int height, BinaryReader bin, byte matrixVersion, ref Int16 checkSum)
        {
            Matrix matrix = new Matrix();
            for (int i = 0; i < height; i++)
            {
                AddRow(matrix, width, bin, matrixVersion, ref checkSum);
            }
            return matrix;
        }

        private void AddRow(Matrix matrix, int width, BinaryReader bin, byte matrixVersion, ref Int16 checkSum)
        {
            Rational[] coefficients = new Rational[width];
            double result;
            
            //read row
            for (int i = 0; i < width; i++)
            {
                double coefficient = bin.ReadDouble();
                coefficients[i] = coefficient;
                Utils.AddBytesToInt16(BitConverter.GetBytes(coefficient), ref checkSum);
            }
            result = bin.ReadDouble();
            Utils.AddBytesToInt16(BitConverter.GetBytes(result), ref checkSum);

            //validate row
            if (!ValidateRowFooter(bin, matrixVersion))
            {
                throw new ApplicationException("Row validation not successful.");
            }
            matrix.Rows.Add(new MatrixRow(coefficients, result));
        }


        private void GetDimensions(BinaryReader bin, out int width, out int height, ref Int16 checkSum)
        {
            width = bin.ReadInt32();
            height = bin.ReadInt32();

            Utils.AddBytesToInt16(BitConverter.GetBytes(width), ref checkSum);
            Utils.AddBytesToInt16(BitConverter.GetBytes(height), ref checkSum);
        }

        private bool ValidateRowFooter(BinaryReader bin, byte matrixVersion)
        {
            byte rowFooter = bin.ReadByte();
            return ((rowFooter == ROW_FOOTER1 && matrixVersion == MATRIX_VERSION1) ||
                (rowFooter == ROW_FOOTER2 && matrixVersion == MATRIX_VERSION2));
        }

        private bool ValidateMatrixFooter(BinaryReader bin)
        {
            for (int i = 0; i < MATRIX_FOOTER.Length; i++)
            {
                if (bin.ReadByte() != MATRIX_FOOTER[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Saves Matrix object into a file in a .txt format.
        /// </summary>
        /// <param name="matrix">Matrix to save</param>
        /// <param name="stream">Stream used for writing data to a file.</param>
        public void Save(Matrix matrix, Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            short checkSum = 0;

            writer.Write((Int32)matrix.Rows[0].Coefficients.Length);
            Utils.AddBytesToInt16(BitConverter.GetBytes(matrix.Rows[0].Coefficients.Length), ref checkSum);

            writer.Write((Int32)matrix.RowCount);
            Utils.AddBytesToInt16(BitConverter.GetBytes(matrix.RowCount), ref checkSum);

            writer.Write((byte)0x02);
            //lines
            for (int i = 0; i < matrix.RowCount; i++)
            {
                //columns
                for (int j = 0; j < matrix.Rows[i].Coefficients.Length; j++)
                {
                    double coefficient = Rational.RationalToDouble(matrix.Rows[i].Coefficients[j],2);
                    writer.Write(coefficient);
                    Utils.AddBytesToInt16(BitConverter.GetBytes(coefficient), ref checkSum);
                }
                double result = Rational.RationalToDouble(matrix.Rows[i].Result, 2);
                writer.Write(result);
                Utils.AddBytesToInt16(BitConverter.GetBytes(result), ref checkSum);

                writer.Write((byte)0xC0);
            }
            
            writer.Write((Int16)checkSum);
            writer.Write((byte)0xEE);
            writer.Write((byte)0xFF);
        }

        private const byte MATRIX_VERSION1 = 0x01;
        private const byte MATRIX_VERSION2 = 0x02;
        private const byte ROW_FOOTER1 = 0xCC;
        private const byte ROW_FOOTER2 = 0xC0;
        private static readonly byte[] MATRIX_FOOTER = { 0xEE, 0xFF };
    }
}
