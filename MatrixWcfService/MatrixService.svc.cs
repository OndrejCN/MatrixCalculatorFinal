using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MatrixLib;
using MatrixReaderLib;
using RationalLib;

namespace MatrixWcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class MatrixService : IMatrixService
    {
        public int AddNumbers(int x, int y)
        {
            return (x + y + 100);
        }

        public int AddNumbers2(Container container)
        {
            return container.X + container.Y;
        }

        public string SolveMatrix(MatrixJson matrixJson)
        {
            Rational[] results;
            Matrix matrix = JsonMatrixReader.MatrixJsonToMatrix(matrixJson);
            matrix.Solve(out results);

            //string outputHtml = "<html> <head></head> <body> <h1> This is a Heading </h1> <p> This is a paragraph.</p> </body></html>";
            string output = "Results of matrix: [&nbsp&nbsp&nbsp&nbsp&nbsp";
            for (int i = 0; i < results.Length; i++)
            {
                //output += results[i].Numerator.ToString();
                output += results[i].Numerator.ToString() + " | " + results[i].Denominator.ToString();
                //output += Rational.RationalToDouble(results[i], 2).ToString();
                output += "&nbsp&nbsp&nbsp&nbsp&nbsp";
            }
            output += "]";
            //return string.Join("***", results);\
            return output;
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
