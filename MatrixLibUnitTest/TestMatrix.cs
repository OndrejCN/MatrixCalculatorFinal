using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLibUnitTest
{
    class TestMatrix : MatrixLib.Matrix
    {
        public new void ClearUnwantedCoefficients(List<MatrixLib.MatrixRow> rowsForSolving, int row)
        {
            base.ClearUnwantedCoefficients(rowsForSolving, row);
        }
    }
}
