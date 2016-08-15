using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RationalLib;
using MatrixLib;
using Common;
using MatrixReaderLib;
using System.IO;

namespace SimpleMatrixCalculator
{
    public partial class MainForm : Form
    {
        RationalTextBox[,] matrixTextBox;
        Label[] resultsLabel;
        Label[,] variableDescriptionsLabels;
        bool LoadingFileFlag = false;

        public MainForm()
        {
            InitializeComponent();

            matrixTextBox = new RationalTextBox[1, 2];
            variableDescriptionsLabels = new Label[1, 1];
            int xBeginingPosition = 50;

            matrixTextBox[0, 0] = new RationalTextBox(new Rational(0, 1));
            matrixTextBox[0, 0].Location = new Point(xBeginingPosition, VERTICAL_SPACING);

            matrixTextBox[0, 1] = new RationalTextBox(new Rational(0, 1));
            matrixTextBox[0, 1].Location = new Point(xBeginingPosition + HORIZONTAL_SPACING, VERTICAL_SPACING);

            this.Controls.Add(matrixTextBox[0, 0]);
            this.Controls.Add(matrixTextBox[0, 1]);
            CreateVariableDescriptions();
        }

        protected override void OnClick(EventArgs e)
        {
            for (int i = 0; i < matrixTextBox.GetLength(0); i++)
            {
                for (int j = 0; j < matrixTextBox.GetLength(1); j++)
                {
                    var textBox = matrixTextBox[i, j];
                    textBox.Validate();
                }
            }
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            bool wrongInput;
            Rational[] results;
            Matrix matrix = ReadTextBoxMatrix();

            RemoveAllResultLabels();
            if (matrix != null)
            {
                try
                {
                    Matrix.MatrixResult matrixResult = matrix.Solve(out results);
                    ProcessResults(matrixResult, results);
                }
                catch (OverflowException)
                {
                    this.StatusLbl.ForeColor = Color.Red;
                    this.StatusLbl.Text = "System overflow. Try to simplify input numbers.";
                }
                //catch (DivideByZeroException)
                //{
                //    this.StatusLbl.ForeColor = Color.Red;
                //    this.StatusLbl.Text = "Division by zero. Check, if all inputs are correct.";
                //}
            }
            else
            {
                this.StatusLbl.ForeColor = Color.Red;
                this.StatusLbl.Text = "Wrong input.";
            }
        }

        private Matrix ReadTextBoxMatrix()
        {
            Matrix matrix = new Matrix();
            var error = false;

            //all lines
            for (int i = 0; i < matrixTextBox.GetLength(0); i++)
            {
                Rational[] coefficients = new Rational[matrixTextBox.GetLength(1) - 1];
                Rational result;

                //all columns
                for (int j = 0; j < matrixTextBox.GetLength(1); j++)
                {
                    var textBox = matrixTextBox[i, j];
                    if (!textBox.Validate())
                    {
                        error = true;
                        continue;
                    }
                    int numerator = textBox.Value.Numerator;
                    int denominator = textBox.Value.Denominator;

                    //if (j < matrixTextBox.GetLength(1) - 1 && zeroDenominatorInput == false)
                    if (j < matrixTextBox.GetLength(1) - 1)
                    {
                        coefficients[j] = new Rational(numerator, denominator);
                    }
                    //else if (zeroDenominatorInput == false)
                    else
                    {
                        result = new Rational(numerator, denominator);
                        matrix.Rows.Add(new MatrixRow(coefficients, result));
                    }
                }
            }
            return (error) ? null : matrix;
        }

        private void ProcessResults(Matrix.MatrixResult matrixResult, Rational[] results)
        {
            //RemoveAllResultLabels();

            if (matrixResult == Matrix.MatrixResult.Exact)
            {
                resultsLabel = PrintResults(results);
                this.StatusLbl.ForeColor = Color.Green;
                this.StatusLbl.Text = "Matrix has one solution.";
                return;
            }
            else if (matrixResult == Matrix.MatrixResult.Infinite)
            {
                this.StatusLbl.ForeColor = Color.Red;
                this.StatusLbl.Text = "Matrix has infinite solutions.";
                return;
            }
            this.StatusLbl.ForeColor = Color.Red;
            this.StatusLbl.Text = "Matrix has no solution.";
        }

        private void MatrixDimensionSetter_ValueChanged(object sender, EventArgs e)
        {
            if (!LoadingFileFlag)
            {
                RemoveAllResultLabels();
                RemoveAllRationalTextBoxes();
                RemoveAllVariableDescriptions();


                if (matrixTextBox.GetLength(0) < MatrixDimensionSetter.Value)
                {
                    IncreaseMatrixTextBoxDimension();
                }
                else
                {
                    LowerMatrixTextBoxDimension();
                }


                CreateRationalTextBoxes(matrixTextBox.GetLength(0));
                variableDescriptionsLabels = CreateVariableDescriptions();
            }
        }

        private Label PrintResult(string id, Rational result, int xPosition, int yPosition)
        {
            Label resultLabel = new Label();
            resultLabel.AutoSize = true;
            resultLabel.Location = new Point(xPosition, yPosition);
            resultLabel.Name = "resultLabel" + id;
            resultLabel.Font = new Font(FontFamily.GenericSansSerif, 14);
            resultLabel.TabIndex = 9;
            resultLabel.Text = id + " = " + result.Numerator.ToString() + " / " + result.Denominator.ToString();
            this.Controls.Add(resultLabel);

            return resultLabel;
        }

        private Label[] PrintResults(Rational[] results)
        {
            Label[] printResults = new Label[results.Length];

            for (int i = 0; i < results.Length; i++)
            {
                var rightBox = matrixTextBox[i, matrixTextBox.GetLength(0)];
                int xPosition = rightBox.Right + 40;
                int yPosition = (rightBox.Top + rightBox.Bottom) / 2; ;
                var id = Encoding.ASCII.GetString(new byte[] { (byte)('K' + i) });
                var result = PrintResult(id, results[i], xPosition, yPosition);
                result.Top = result.Top - result.Height / 2;
                printResults[i] = result;
                yPosition += HORIZONTAL_SPACING;
            }
            return printResults;
        }

        private Label CreateVariableDescription(byte id, int xPosition, int yPosition, bool isLast)
        {
            Label variableDescriptionLabel = new Label();
            variableDescriptionLabel.AutoSize = true;

            variableDescriptionLabel.Name = "descriptionLabel" + id;
            variableDescriptionLabel.Font = new Font(FontFamily.GenericSansSerif, 12);

            if (!isLast)
            {
                variableDescriptionLabel.Text = ("x  " + Encoding.ASCII.GetString(new byte[] { id }) + "   +");
            }
            else
            {
                variableDescriptionLabel.Text = ("x  " + Encoding.ASCII.GetString(new byte[] { id }) + "   =");
            }

            variableDescriptionLabel.Location = new Point(xPosition, yPosition - variableDescriptionLabel.Height / 2);

            this.Controls.Add(variableDescriptionLabel);
            //variableDescriptionLabel.BackColor = Color.Blue;
            return variableDescriptionLabel;
        }

        private Label[,] CreateVariableDescriptions()
        {
            int dimmension = matrixTextBox.GetLength(0);
            variableDescriptionsLabels = new Label[dimmension, dimmension];

            //lines
            for (int i = 0; i < dimmension; i++)
            {
                //columns
                for (int j = 0; j < dimmension; j++)
                {
                    bool isLastInRow = false;
                    if (j == dimmension - 1)
                    {
                        isLastInRow = true;
                    }
                    var rightBox = matrixTextBox[i, j];
                    int xPosition = rightBox.Right + 5;
                    int yPosition = (rightBox.Top + rightBox.Bottom) / 2;

                    variableDescriptionsLabels[i, j] = CreateVariableDescription((byte)(j + 'K'), xPosition, yPosition, isLastInRow);
                }
            }
            return variableDescriptionsLabels;
        }

        private void CreateRationalTextBoxes(int dimmension)
        {
            int xPosition = 50;
            int yPosition = 80;

            //lines
            for (int i = 0; i < dimmension; i++)
            {
                //columns
                for (int j = 0; j < dimmension + 1; j++)
                {
                    if (matrixTextBox[i, j] == null)
                    {
                        matrixTextBox[i, j] = new RationalTextBox(new Rational(0, 1));
                    }
                    matrixTextBox[i, j].Location = new Point(xPosition, yPosition);
                    this.Controls.Add(matrixTextBox[i, j]);

                    xPosition += HORIZONTAL_SPACING;
                }
                xPosition = 50;
                yPosition += VERTICAL_SPACING;
            }
        }

        private void LowerMatrixTextBoxDimension()
        {
            RationalTextBox[,] backupTextBoxes = new RationalTextBox[matrixTextBox.GetLength(0) - 1, matrixTextBox.GetLength(1) - 1];
            for (int i = 0; i < backupTextBoxes.GetLength(0); i++)
            {
                for (int j = 0; j < backupTextBoxes.GetLength(1); j++)
                {
                    backupTextBoxes[i, j] = matrixTextBox[i, j];
                }
            }
            matrixTextBox = backupTextBoxes;
        }

        private void IncreaseMatrixTextBoxDimension()
        {
            RationalTextBox[,] backupTextBoxes = new RationalTextBox[matrixTextBox.GetLength(0) + 1, matrixTextBox.GetLength(1) + 1];
            for (int i = 0; i < backupTextBoxes.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < backupTextBoxes.GetLength(1) - 1; j++)
                {
                    backupTextBoxes[i, j] = matrixTextBox[i, j];
                }
            }
            matrixTextBox = backupTextBoxes;
        }

        private void RemoveAllVariableDescriptions()
        {
            int dimmension = variableDescriptionsLabels.GetLength(0);
            for (int i = 0; i < dimmension; i++)
            {
                for (int j = 0; j < dimmension; j++)
                {
                    this.Controls.Remove(variableDescriptionsLabels[i, j]);
                }
            }
        }

        private void RemoveAllRationalTextBoxes()
        {
            for (int i = 0; i < matrixTextBox.GetLength(0); i++)
            {
                for (int j = 0; j < matrixTextBox.GetLength(1); j++)
                {
                    this.Controls.Remove(matrixTextBox[i, j]);
                }
            }
        }

        private void RemoveAllResultLabels()
        {
            if (resultsLabel != null)
            {
                for (int i = 0; i < resultsLabel.Length; i++)
                {
                    this.Controls.Remove(resultsLabel[i]);
                }
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (this.LoadOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string pathSource = LoadOpenFileDialog.InitialDirectory + LoadOpenFileDialog.FileName;
                    Loader matrixReader = new Loader(pathSource);

                    Matrix matrix = matrixReader.Load();
                    StatusLbl.ForeColor = Color.Green;
                    StatusLbl.Text = pathSource;

                    RemoveAllResultLabels();
                    RemoveAllRationalTextBoxes();
                    RemoveAllVariableDescriptions();

                    LoadMatrixToMatrixTextBox(matrix);

                    LoadingFileFlag = true;
                    this.MatrixDimensionSetter.Value = (matrix.RowCount);
                    LoadingFileFlag = false;
                    CreateRationalTextBoxes(matrixTextBox.GetLength(0));
                    variableDescriptionsLabels = CreateVariableDescriptions();
                }
                catch (Exception exception)
                {
                    StatusLbl.ForeColor = Color.Red;
                    this.StatusLbl.Text = exception.Message;
                }

            }
        }

        private void LoadMatrixToMatrixTextBox(Matrix matrix)
        {
            RationalTextBox[,] loadedMatrixTextBox = new RationalTextBox[matrix.RowCount, matrix.RowCount + 1];

            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.RowCount; j++)
                {
                    loadedMatrixTextBox[i, j] = new RationalTextBox(matrix.Rows[i].Coefficients[j]);
                }
                loadedMatrixTextBox[i, matrix.RowCount] = new RationalTextBox(matrix.Rows[i].Result);
            }
            matrixTextBox = loadedMatrixTextBox;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Matrix matrix = ReadTextBoxMatrix();

                if (matrix != null)
                {
                    string pathSource = SaveFileDialog.InitialDirectory + SaveFileDialog.FileName;
                    Loader matrixSaver = new Loader(pathSource);

                    matrixSaver.Save(matrix, SaveFileDialog.OpenFile());
                }
                else
                {
                    this.StatusLbl.Text = "Bad inputs.";
                }

            }
        }

        public const int VERTICAL_SPACING = 80;
        public const int HORIZONTAL_SPACING = 130;
    }
}
