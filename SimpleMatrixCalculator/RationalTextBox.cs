using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RationalLib;

namespace SimpleMatrixCalculator
{
    public class RationalTextBox : Panel
    {
        // --> private
        private TextBox Numerator { get; set; }
        private TextBox Denominator { get; set; }
        public bool WrongInputNumerator = false;
        public bool WrongInputDenominator = false;

        private bool showDenominator;
        public bool ShowDenominator
        {
            get { return showDenominator; }
            set
            {
                showDenominator = value;
                this.UpdateState();
            }
        }

        //parsovat hodnoty
        //private Rational value; // pojde prec
        public Rational Value
        {
            get
            {
                int numerator = Int32.Parse(Numerator.Text);
                int denominator = Int32.Parse(Denominator.Text);
                return new Rational(numerator, denominator);
            }
            set
            {
                Numerator.Text = value.Numerator.ToString();
                Denominator.Text = value.Denominator.ToString();
            }
        }

        public RationalTextBox(Rational number)
        {
            //this.BackColor = Color.Red;
            var size = new Size(35, 22);
            Numerator = new TextBox();
            //numerator.Location = new Point(5, 0);
            Numerator.Location = new Point(5, 20);
            Numerator.Size = size;
            Numerator.TabIndex = 1;
            Numerator.TextAlign = HorizontalAlignment.Right;

            Denominator = new TextBox();
            Denominator.Left = Numerator.Left;
            Denominator.Top = Numerator.Bottom - 7;
            Denominator.Size = size;
            Denominator.TabIndex = 1;
            Denominator.TextAlign = HorizontalAlignment.Right;

            this.Height = Denominator.Bottom;
            this.Width = Denominator.Right + 5;

            Value = number;

            this.Controls.Add(Numerator);
            this.Controls.Add(Denominator);
            this.Enter += RationalTextBox_Enter;
            this.Leave += RationalTextBox_Leave;
            this.Numerator.Click += RationalTextBox_Click;

            this.ShowDenominator = (this.Denominator.Text != "1");
        }

        private void RationalTextBox_Click(object sender, EventArgs e)
        {
            this.ShowDenominator = true;
        }

        private void RationalTextBox_Leave(object sender, EventArgs e)
        {
            this.ShowDenominator = (this.Denominator.Text != "1");
            this.Validate();
        }

        private void RationalTextBox_Enter(object sender, EventArgs e)
        {
            this.ShowDenominator = true;
        }

        private void UpdateState()
        {
            if (this.ShowDenominator)
            {
                Numerator.Top = 0;
                this.Denominator.Visible = true;
            }
            else
            {
                Numerator.Top = this.Height / 2 - Numerator.Height / 2;
                this.Denominator.Visible = false;
            }
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.ShowDenominator)
            {
                Pen myPen = new Pen(Color.Black);
                myPen.Width = 3;
                e.Graphics.DrawLine(myPen, 0, Numerator.Bottom + 5, this.Width, Numerator.Bottom + 5);
            }
        }

        public bool Validate()
        {
            int numberator, denominator;
            var numeratorValid = Int32.TryParse(this.Numerator.Text, out numberator);
            var denominatorValid = (Int32.TryParse(this.Denominator.Text, out denominator) && denominator != 0);
            this.Numerator.ForeColor = (!numeratorValid) ? Color.Red : Color.Empty;
            this.Denominator.ForeColor = (!denominatorValid) ? Color.Red : Color.Empty;
            return (numeratorValid && denominatorValid);
        }

    }
}
