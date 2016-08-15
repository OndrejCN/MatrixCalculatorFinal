using MatrixLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixReaderLib
{
    interface IMatrixReader
    {
        Matrix Load(Stream stream);
        void Save(Matrix matrix, Stream stream);
    }
}
