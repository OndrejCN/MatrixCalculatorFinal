using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLib
{
    class Field
    {
        public string[] linesOfField;

        public Field()
        {
            linesOfField = new string[3];
        }

        public void Fill(RationalLib.Rational number, int fieldWidth)
        {
            int spacesAmount;
            int widthOfRational = Table.GetWidthOfRational(number);

            if (number.Denominator != 1)
            {
                //numerator
                {
                    spacesAmount = fieldWidth - number.Numerator.ToString().Length;
                    FillWithSpaces(spacesAmount, 0);
                    linesOfField[0] += number.Numerator.ToString();
                }
                //middle line
                {
                    spacesAmount = fieldWidth - widthOfRational;
                    FillWithSpaces(spacesAmount, 1);
                    for (int i = 0; i < widthOfRational; i++)
                    {
                        linesOfField[1] += "-";
                    }
                }
                //denumerator
                {
                    spacesAmount = fieldWidth - number.Denominator.ToString().Length;
                    FillWithSpaces(spacesAmount, 2);
                    linesOfField[2] += number.Denominator.ToString();
                }
            }

            //denumerator == 1 --> no middle line
            else
            {
                spacesAmount = fieldWidth - number.Numerator.ToString().Length;
                FillWithSpaces(spacesAmount, 1);
                linesOfField[1] += number.Numerator.ToString();
            }
        }

        private void FillWithSpaces(int spacesAmount, int position)
        {
            for (int j = 0; j < spacesAmount; j++)
            {
                linesOfField[position] += " ";
            }
        }
    }
}
