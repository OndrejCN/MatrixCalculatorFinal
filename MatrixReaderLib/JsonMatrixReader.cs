using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLib;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using RationalLib;
using Newtonsoft.Json;

namespace MatrixReaderLib
{
    /// <summary>
    /// Class for reading JSON file content and turning it into Matrix class instance.
    /// </summary>
    public class JsonMatrixReader : IMatrixReader
    {
        /// <summary>
        /// Reads stream and turns it into Matrix class instance.
        /// </summary>
        /// <param name="stream">Stream, which contains Matrix data</param>
        /// <returns>Matrix instance</returns>
        public Matrix Load(Stream stream)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MatrixJson));

            //stream.Position = 0;
            MatrixJson myObject = (MatrixJson)serializer.ReadObject(stream);

            return MatrixJsonToMatrix(myObject);
        }

        /// <summary>
        /// Converts object of type MatrixJson into Matrix instance.
        /// </summary>
        /// <param name="matrixJson">Input object of type MatrixJson</param>
        /// <returns>new instance of Matrix class</returns>
        public static Matrix MatrixJsonToMatrix(MatrixJson matrixJson)
        {
            Matrix matrix = new Matrix();

            foreach (MatrixRowJson matrixRowJson in matrixJson.Rows)
            {
                Rational[] coefs = new Rational[matrixRowJson.Coefs.Length];
                for (int i = 0; i < coefs.Length; i++)
                {
                    coefs[i] = (Rational)matrixRowJson.Coefs[i];
                }
                matrix.Rows.Add(new MatrixRow(coefs, matrixRowJson.Result));
            }
            return matrix;
        }

        /// <summary>
        /// Saves Matrix object into a file in a .Json format.
        /// </summary>
        /// <param name="matrix">Matrix to save</param>
        /// <param name="stream">Stream used for writing data to a file.</param>
        public void Save(Matrix matrix, Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream))
            {
                using (var writer = new JsonTextWriter(streamWriter))
                {
                    writer.Formatting = Formatting.Indented;

                    writer.WriteStartObject();
                    {
                        writer.WritePropertyName("Height");
                        writer.WriteValue(matrix.RowCount);

                        writer.WritePropertyName("Rows");
                        writer.WriteStartArray();
                        {
                            for (int i = 0; i < matrix.RowCount; i++)
                            {
                                writer.WriteStartObject();
                                {
                                    writer.WritePropertyName("Coefs");
                                    writer.WriteStartArray();
                                    {
                                        for (int j = 0; j<matrix.RowCount;  j++)
                                        {
                                            writer.WriteValue(Rational.RationalToDouble(matrix.Rows[i].Coefficients[j],2));
                                        }
                                    }
                                    writer.WriteEndArray();
                                    writer.WritePropertyName("Result");
                                    writer.WriteValue(Rational.RationalToDouble(matrix.Rows[i].Result, 2));
                                }
                                writer.WriteEndObject();
                            }
                        }
                        writer.WriteEndArray();

                        writer.WritePropertyName("Width");
                        writer.WriteValue(matrix.RowCount);
                    }
                    writer.WriteEndObject();
                }
            }
        }
    }

    /// <summary>
    /// Class used for reading MatrixRows from XML file
    /// </summary>
    [DataContract]
    public class MatrixJson
    {
        /// <summary>Variable for storing Height value of Matrix</summary>
        [DataMember(Name = "Height")]
        public int Height { get; set; }

        /// <summary>Array for storing Rows values of Matrix</summary>
        [DataMember(Name = "Rows")]
        public MatrixRowJson[] Rows { get; set; }

        /// <summary>Variable for storing Width value of Matrix</summary>
        [DataMember(Name = "Width")]
        public int Width { get; set; }
    }

    /// <summary>
    /// Class used for reading MatrixRows from JSON file
    /// </summary>
    [DataContract]
    public class MatrixRowJson
    {
        /// <summary>Array for storing Coefficients values of each Matrix row</summary>
        [DataMember(Name = "Coefs")]
        public double[] Coefs { get; set; }

        /// <summary>Variable for storing Result value of each Matrix row</summary>
        [DataMember(Name = "Result")]
        public double Result { get; set; }
    }
}