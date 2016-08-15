using MatrixLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MatrixReaderLib
{
    /// <summary>
    /// General Class for reading various filetype content and turning it into Matrix class instance.
    /// </summary>
    public class Loader
    {
        string filePath;
        string format;
        IMatrixReader matrixReader;

        /// <summary>
        /// Constructor of Loader
        /// </summary>
        /// <param name="path">Path of file we want to read Matrix from</param>
        public Loader(string path)
        {
            filePath = path;
            //var ext = Path.GetExtension(path);
            format = GetFormat(path);

            switch (format.ToLower())
            {
                case "txt":
                    matrixReader = new TxtMatrixReader();
                    break;
                case "bin":
                    matrixReader = new BinMatrixReader();
                    break;
                case "xml":
                    matrixReader = new XmlMatrixReader();
                    break;
                case "json":
                    matrixReader = new JsonMatrixReader();
                    break;
            }
        }

        /// <summary>
        /// Uses MatrixReader to reads stream and turns it into Matrix class instance.
        /// </summary>
        /// <returns>Matrix instance</returns>
        public Matrix Load()
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return matrixReader.Load(fileStream);
            }
        }

        /// <summary>
        /// Saves Matrix object into a file in a demanded format.
        /// </summary>
        public void Save(Matrix matrix, Stream stream)
        {
            matrixReader.Save(matrix, stream);
            stream.Close();
        }
        
        private string GetFormat(string path)
        {
            // "Temp\global.xxx.bak"
            var index = path.LastIndexOf(".");
            var ext = path.Substring(index + 1);
            // .; +, *
            var match = Regex.Match(path, @"\.([^\/.]+)$", RegexOptions.IgnoreCase);
            var ext2 = match.Groups[1].Value;
            //
            char[] delimiterChars = { '.' };
            string[] parts = path.Split(delimiterChars);
            return parts[parts.Length - 1];
        }
    }
}
