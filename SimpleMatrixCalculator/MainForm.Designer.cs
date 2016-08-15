namespace SimpleMatrixCalculator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.CalculateButton = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.MatrixDimensionSetter = new System.Windows.Forms.NumericUpDown();
            this.Heading = new System.Windows.Forms.Label();
            this.StatusLbl = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.LoadOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.LoadButton = new System.Windows.Forms.Button();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SaveButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixDimensionSetter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CalculateButton
            // 
            this.CalculateButton.Location = new System.Drawing.Point(222, 40);
            this.CalculateButton.Name = "CalculateButton";
            this.CalculateButton.Size = new System.Drawing.Size(75, 23);
            this.CalculateButton.TabIndex = 2;
            this.CalculateButton.Text = "Calculate";
            this.CalculateButton.UseVisualStyleBackColor = true;
            this.CalculateButton.Click += new System.EventHandler(this.CalculateButton_Click);
            // 
            // MatrixDimensionSetter
            // 
            this.MatrixDimensionSetter.Location = new System.Drawing.Point(96, 40);
            this.MatrixDimensionSetter.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.MatrixDimensionSetter.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MatrixDimensionSetter.Name = "MatrixDimensionSetter";
            this.MatrixDimensionSetter.Size = new System.Drawing.Size(120, 22);
            this.MatrixDimensionSetter.TabIndex = 8;
            this.MatrixDimensionSetter.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MatrixDimensionSetter.ValueChanged += new System.EventHandler(this.MatrixDimensionSetter_ValueChanged);
            // 
            // Heading
            // 
            this.Heading.AutoSize = true;
            this.Heading.Location = new System.Drawing.Point(12, 40);
            this.Heading.Name = "Heading";
            this.Heading.Size = new System.Drawing.Size(78, 17);
            this.Heading.TabIndex = 9;
            this.Heading.Text = "Matrix size:";
            // 
            // StatusLbl
            // 
            this.StatusLbl.AutoSize = true;
            this.StatusLbl.ForeColor = System.Drawing.Color.LimeGreen;
            this.StatusLbl.Location = new System.Drawing.Point(497, 43);
            this.StatusLbl.Name = "StatusLbl";
            this.StatusLbl.Size = new System.Drawing.Size(76, 17);
            this.StatusLbl.TabIndex = 10;
            this.StatusLbl.Text = "Status: OK";
            // 
            // LoadOpenFileDialog
            // 
            this.LoadOpenFileDialog.FileName = "openFileDialog1";
            this.LoadOpenFileDialog.Filter = "Text Files|*.txt|Xml Files|*.xml|Json Files|*.json|Bin files|*.bin";
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(303, 40);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(93, 23);
            this.LoadButton.TabIndex = 11;
            this.LoadButton.Text = "Load Matrix";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.Filter = "Text Files|*.txt|Xml Files|*.xml|Json Files|*.json|Bin files|*.bin";
            this.SaveFileDialog.RestoreDirectory = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(402, 40);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(89, 23);
            this.SaveButton.TabIndex = 12;
            this.SaveButton.Text = "Save Matrix";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1044, 28);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 497);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.StatusLbl);
            this.Controls.Add(this.Heading);
            this.Controls.Add(this.MatrixDimensionSetter);
            this.Controls.Add(this.CalculateButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Matrix Calculator";
            ((System.ComponentModel.ISupportInitialize)(this.MatrixDimensionSetter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button CalculateButton;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.NumericUpDown MatrixDimensionSetter;
        private System.Windows.Forms.Label Heading;
        private System.Windows.Forms.Label StatusLbl;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.OpenFileDialog LoadOpenFileDialog;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    }
}

