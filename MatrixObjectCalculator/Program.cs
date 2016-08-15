using MatrixLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using RationalLib;
using MatrixReaderLib;
using Common;

namespace MatrixObjectCalculator
{
    class Program
    {
        const string fileName = "AppSettings.bin";

        static void Main(string[] args)
        {
            string pathSource = @"C:\Users\trubac\Desktop\matrix.bin";
            //string pathSource = @"C:\Users\trubac\Desktop\matrix.json";
            Loader matrixReader = new Loader(pathSource);
            Matrix matrix;
            Rational[] matrixResult;

            try
            {
                matrix = matrixReader.Load();

                //Console.WriteLine(matrix.ToString());
                var solution = matrix.Solve(out matrixResult);

                foreach (string matrixCalculation in matrix.ComputeHistory)
                {
                    Console.WriteLine(matrixCalculation);
                    Console.WriteLine("-----------------------------------");
                }

                Console.WriteLine("Solution: " + solution);
                if (solution == Matrix.MatrixResult.Exact)
                {
                    //Console.WriteLine("Results of matrix: " + Utils.DoubleArrayToString(matrixResult));

                    if (matrix.Verify(matrixResult))
                    {
                        Console.WriteLine("Correct result.");
                    }
                    else
                    {
                        Console.WriteLine("Error in calculation.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //MojaTrieda obj = new MojaTrieda(1,2);
            //Console.WriteLine("Pred funkciou: " +obj);
            //Funkcia(obj);
            //Console.WriteLine("Za funkciou: " + obj);


            //WriteDefaultValues();

            Console.ReadKey();

            //***** CHECKSUM CALCULATION *****//
            //Int16 checkSum = 0;

            //Utils.AddBytesToInt16(BitConverter.GetBytes(4), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes(4), ref checkSum);

            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)1), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)2), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)3), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)4), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)5), ref checkSum);

            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)-1), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)-2), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)-3), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)-4), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)-5), ref checkSum);

            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)1), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)-2), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)3), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)-4), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)-5), ref checkSum);

            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)4.5), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)2.6), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)1.7), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)6.5), ref checkSum);
            //Utils.AddBytesToInt16(BitConverter.GetBytes((double)3), ref checkSum);

            //Console.WriteLine("CheckSum: " + checkSum);
        }

        //public static void WriteDefaultValues()
        //{
        //    using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
        //    {
        //        writer.Write(1);
        //        writer.Write(2);
        //        writer.Write(3);
        //        writer.Write(4);
        //        writer.Write(5);
        //    }
        //}

        //public static void Funkcia(MojaTrieda a)
        //{
        //    Console.WriteLine("Zaciatok funkcie: " + a);
        //    //a = new MojaTrieda(2, 3);
        //    a.a = 2;
        //    a.b = 3;
        //    Console.WriteLine("Koniec funkcie: " + a);
        //}
    }

    //class MojaTrieda
    //{
    //    public int a;
    //    public int b;

    //    public MojaTrieda(int a, int b)
    //    {
    //        this.a = a;
    //        this.b = b;
    //    }

    //    public override string ToString()
    //    {
    //        string str = a.ToString() + b.ToString();
    //        return str;
    //    }
    //}
}

